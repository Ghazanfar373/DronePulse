using System;
using DronePulse.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using DronePulse.Models;
using System.Diagnostics;
using System.ComponentModel;

namespace DronePulse.Controllers
{
    public class MavlinkController : IDisposable
    {
        private bool _disposed = false;
        private readonly object _disposeLock = new object();
        
        // MAVLink message IDs
        private const byte MSG_ID_ATTITUDE = 30;
        private const byte MSG_ID_GPS_RAW_INT = 24;
        private const byte MSG_ID_GLOBAL_POSITION_INT = 33;
        private const byte MSG_ID_VFR_HUD = 74;
        private const byte MSG_ID_HEARTBEAT = 0;
        private const byte MSG_ID_SYS_STATUS = 1;
        private const byte MSG_ID_MISSION_CURRENT = 42;

        private readonly TelemetryData _telemetryData;
        private readonly IConnection _connection;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _readTask;
        
        public MavlinkController(TelemetryData telemetryData, IConnection connection)
        {
            _telemetryData = telemetryData ?? throw new ArgumentNullException(nameof(telemetryData));
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        _cancellationTokenSource?.Cancel();
                        _cancellationTokenSource?.Dispose();
                        _connection?.Dispose();
                        
                        _readTask?.Wait(500);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error during dispose: {ex.Message}");
                    }
                }
                
                _disposed = true;
            }
        }
        
        ~MavlinkController()
        {
            Dispose(false);
        }

        public async Task Start(CancellationToken token)
        {
            _telemetryData.Status = "Connecting...";
            try
            {
                _connection.Open();

                if (!_connection.IsOpen)
                {
                    _telemetryData.Status = "Connection failed.";
                    return;
                }

                _telemetryData.Status = "Connected. Receiving data...";

                var buffer = new byte[1024];
                int bufferPosition = 0;

                while (!token.IsCancellationRequested)
                {
                    if (_connection.BytesToRead > 0)
                    {
                        int bytesRead = await _connection.ReadAsync(buffer, bufferPosition, buffer.Length - bufferPosition, token);
                        bufferPosition += bytesRead;

                        int processedBytes = ProcessMAVLinkBuffer(buffer, ref bufferPosition);
                        
                        if (processedBytes > 0 && processedBytes < bufferPosition)
                        {
                            Array.Copy(buffer, processedBytes, buffer, 0, bufferPosition - processedBytes);
                            bufferPosition -= processedBytes;
                        }
                        else if (processedBytes >= bufferPosition)
                        {
                            bufferPosition = 0;
                        }
                    }
                    else
                    {
                        await Task.Delay(10, token);
                    }
                }
            }
            catch (Exception ex)
            {
                _telemetryData.Status = $"Error: {ex.Message}";
            }
            finally
            {
                _connection.Dispose();
                _telemetryData.Status = "Disconnected";
            }
        }

        private int ProcessMAVLinkBuffer(byte[] buffer, ref int bufferLength)
        {
            int processedBytes = 0;
            for (int i = 0; i < bufferLength; i++)
            {
                if (buffer[i] == 0xFE && i + 5 < bufferLength)
                {
                    int payloadLength = buffer[i + 1];
                    int totalMessageLength = payloadLength + 8;

                    if (i + totalMessageLength <= bufferLength)
                    {
                        var message = new byte[totalMessageLength];
                        Array.Copy(buffer, i, message, 0, totalMessageLength);
                        ParseMAVLinkMessage(message);
                        processedBytes = i + totalMessageLength;
                        i += totalMessageLength - 1;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return processedBytes;
        }

        private void ParseMAVLinkMessage(byte[] message)
        {
            if (message.Length < 8) return;
            byte messageId = message[5];

            switch (messageId)
            {
                case MSG_ID_ATTITUDE:
                    ParseAttitude(message);
                    break;
                case MSG_ID_GPS_RAW_INT:
                    ParseGpsRawInt(message);
                    break;
                case MSG_ID_GLOBAL_POSITION_INT:
                    ParseGlobalPositionInt(message);
                    break;
                case MSG_ID_VFR_HUD:
                    ParseVfrHud(message);
                    break;
                case MSG_ID_HEARTBEAT:
                    ParseHeartbeat(message);
                    break;
                case MSG_ID_SYS_STATUS:
                    ParseSysStatus(message);
                    break;
                case MSG_ID_MISSION_CURRENT:
                    ParseMissionCurrent(message);
                    break;
            }
        }

        private void ParseAttitude(byte[] message)
        {
            if (message.Length < 36) return;
            byte[] payload = new byte[message.Length - 8];
            Array.Copy(message, 6, payload, 0, payload.Length);

            float roll = BitConverter.ToSingle(payload, 4);
            float pitch = BitConverter.ToSingle(payload, 8);
            float yaw = BitConverter.ToSingle(payload, 12);

            _telemetryData.Roll = roll * 180.0 / Math.PI;
            _telemetryData.Pitch = pitch * 180.0 / Math.PI;
            _telemetryData.Yaw = yaw * 180.0 / Math.PI;
            if (_telemetryData.Yaw < 0) _telemetryData.Yaw += 360.0;
        }

        private void ParseGpsRawInt(byte[] message)
        {
            if (message.Length < 38) return;
            byte[] payload = new byte[message.Length - 8];
            Array.Copy(message, 6, payload, 0, payload.Length);

            int lat = BitConverter.ToInt32(payload, 8);
            int lon = BitConverter.ToInt32(payload, 12);
            int alt = BitConverter.ToInt32(payload, 16);
            byte fixType = payload[28];
            byte satellitesVisible = payload[29];

            _telemetryData.Latitude = lat / 1e7;
            _telemetryData.Longitude = lon / 1e7;
            _telemetryData.Altitude = alt / 1000.0;
            _telemetryData.FixType = GetGpsFixType(fixType);
            _telemetryData.SatellitesVisible = satellitesVisible;
        }

        private void ParseGlobalPositionInt(byte[] message)
        {
            if (message.Length < 36) return;
            byte[] payload = new byte[message.Length - 8];
            Array.Copy(message, 6, payload, 0, payload.Length);

            int relativeAlt = BitConverter.ToInt32(payload, 16);
            ushort hdg = BitConverter.ToUInt16(payload, 26);

            _telemetryData.RelativeAltitude = relativeAlt / 1000.0;
            _telemetryData.Heading = hdg / 100.0;
            _telemetryData.CardinalDirection = GetCardinalDirection(_telemetryData.Heading);
        }

        private string GetGpsFixType(byte fixType)
        {
            return fixType switch
            {
                0 => "No GPS", 1 => "No Fix", 2 => "2D Fix", 3 => "3D Fix",
                4 => "DGPS", 5 => "RTK Float", 6 => "RTK Fixed",
                _ => "Unknown"
            };
        }

        private string GetCardinalDirection(double heading)
        {
            string[] directions = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
            int index = (int)Math.Round(heading / 22.5) % 16;
            return directions[index];
        }

        private void ParseVfrHud(byte[] message)
        {
            if (message.Length < 20) return;
            byte[] payload = new byte[message.Length - 8];
            Array.Copy(message, 6, payload, 0, payload.Length);

            float airSpeed = BitConverter.ToSingle(payload, 0);
            float groundSpeed = BitConverter.ToSingle(payload, 4);
            float climbRate = BitConverter.ToSingle(payload, 12);
            float heading = BitConverter.ToSingle(payload, 8);
            float throttle = BitConverter.ToSingle(payload, 16);

            _telemetryData.AirSpeed = airSpeed;
            _telemetryData.GroundSpeed = groundSpeed;
            _telemetryData.ClimbRate = climbRate;
            _telemetryData.Heading = heading;
            _telemetryData.Throttle = throttle;
        }

        private void ParseHeartbeat(byte[] message)
        {
            if (message.Length < 9) return;
            
            // The flight mode is in the 7th byte of the payload (index 6)
            byte baseMode = message[6];
            byte customMode = message[7];
            
            // Simple flight mode detection based on custom mode
            // This is a simplified version - you might need to adjust based on your autopilot
            string flightMode = "UNKNOWN";
            
            if ((baseMode & 0x80) != 0) // Check if system has GPS
            {
                flightMode = customMode switch
                {
                    0 => "STABILIZE",
                    1 => "ACRO",
                    2 => "ALT_HOLD",
                    3 => "AUTO",
                    4 => "GUIDED",
                    5 => "LOITER",
                    6 => "RTL",
                    7 => "CIRCLE",
                    8 => "POSITION",
                    9 => "LAND",
                    10 => "OF_LOITER",
                    11 => "DRIFT",
                    13 => "SPORT",
                    14 => "FLIP",
                    15 => "AUTOTUNE",
                    16 => "POSHOLD",
                    17 => "BRAKE",
                    18 => "THROW",
                    19 => "AVOID_ADSB",
                    20 => "GUIDED_NOGPS",
                    _ => "UNKNOWN"
                };
            }
            
            _telemetryData.FlightMode = flightMode;
            _telemetryData.SystemStatus = GetSystemStatus(message[8]);
        }

        private string GetSystemStatus(byte systemStatus)
        {
            return systemStatus switch
            {
                0 => "UNINIT",
                1 => "BOOT",
                2 => "CALIBRATING",
                3 => "STANDBY",
                4 => "ACTIVE",
                5 => "CRITICAL",
                6 => "EMERGENCY",
                7 => "POWEROFF",
                _ => "UNKNOWN"
            };
        }

        private void ParseSysStatus(byte[] message)
        {
            if (message.Length < 23) return;
            
            // Battery voltage is in the first 2 bytes (uint16_t, in 10mV)
            ushort voltage = BitConverter.ToUInt16(message, 6);
            float voltageV = voltage / 100.0f; // Convert to volts
            
            // Battery remaining is in the 21st byte (uint8_t, in %)
            byte batteryRemaining = message[20];
            
            _telemetryData.BatteryVoltage = voltageV;
            _telemetryData.BatteryRemaining = batteryRemaining / 100.0; // Convert to 0-1 range
        }

        private void ParseMissionCurrent(byte[] message)
        {
            if (message.Length < 9) return;
            
            // Current waypoint is in the first 2 bytes (uint16_t)
            ushort currentWp = BitConverter.ToUInt16(message, 6);
            
            // We don't get total waypoints in this message, so we'll just show the current one
            _telemetryData.CurrentWaypoint = currentWp;
            
            // Note: You might want to request the mission count separately if you need the total
            // This is just a simple example
        }
    }
}