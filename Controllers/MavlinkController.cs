using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using DronePulse.Models;

namespace DronePulse.Controllers
{
    public class MavlinkController
    {
        private const byte MSG_ID_ATTITUDE = 30;
        private const byte MSG_ID_GPS_RAW_INT = 24;
        private const byte MSG_ID_GLOBAL_POSITION_INT = 33;

        private readonly TelemetryData _telemetryData;
        private readonly string _portName;
        private readonly int _baudRate;

        public MavlinkController(TelemetryData telemetryData, string portName, int baudRate)
        {
            _telemetryData = telemetryData;
            _portName = portName;
            _baudRate = baudRate;
        }

        public async Task Start(CancellationToken token)
        {
            _telemetryData.Status = $"Connecting to {_portName}...";
            try
            {
                using var serialPort = new SerialPort(_portName, _baudRate);
                serialPort.Open();

                if (!serialPort.IsOpen) return;

                _telemetryData.Status = $"Connected to {_portName}. Receiving data...";

                var buffer = new byte[1024];
                int bufferPosition = 0;

                while (!token.IsCancellationRequested)
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        int bytesRead = await serialPort.BaseStream.ReadAsync(buffer, bufferPosition, buffer.Length - bufferPosition, token);
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
    }
}
