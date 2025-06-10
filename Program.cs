using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    // MAVLink message IDs
    const byte MSG_ID_GPS_RAW_INT = 24;
    const byte MSG_ID_GLOBAL_POSITION_INT = 33;
    const byte MSG_ID_GPS_STATUS = 25;
    const byte MSG_ID_ATTITUDE = 30;
    const byte MSG_ID_HEARTBEAT = 0;

    static async Task Main(string[] args)
    {
        string portName = "COM8"; // Change this to your port
        int baudRate = 115200;

        Console.WriteLine($"Connecting to {portName} at {baudRate} baud...");

        try
        {
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("\nDisconnecting...");
            };

            using var serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();
            
            if (!serialPort.IsOpen)
            {
                Console.WriteLine($"Failed to open port {portName}");
                return;
            }

            Console.WriteLine("Connected. Waiting for MAVLink messages (GPS & Attitude) (press Ctrl+C to exit)...");

            var buffer = new byte[1024];
            int bufferPosition = 0;

            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        int bytesRead = await serialPort.BaseStream.ReadAsync(
                            buffer, bufferPosition, 
                            buffer.Length - bufferPosition, 
                            cts.Token);
                        
                        bufferPosition += bytesRead;

                        // Process messages in buffer
                        int processedBytes = ProcessMAVLinkBuffer(buffer, bufferPosition);
                        
                        // Shift remaining data to start of buffer
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
                        await Task.Delay(10, cts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    await Task.Delay(1000, cts.Token);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex}");
        }
        
        Console.WriteLine("Disconnected");
    }

    static int ProcessMAVLinkBuffer(byte[] buffer, int bufferLength)
    {
        int processedBytes = 0;
        
        for (int i = 0; i < bufferLength; i++)
        {
            // Look for MAVLink start marker (0xFE for MAVLink v1)
            if (buffer[i] == 0xFE && i + 5 < bufferLength)
            {
                // Extract payload length
                int payloadLength = buffer[i + 1];
                int totalMessageLength = payloadLength + 8; // Header(6) + Payload + Checksum(2)
                
                // Check if we have the complete message
                if (i + totalMessageLength <= bufferLength)
                {
                    // Extract the complete message
                    var message = new byte[totalMessageLength];
                    Array.Copy(buffer, i, message, 0, totalMessageLength);
                    
                    // Parse the message
                    ParseMAVLinkMessage(message);
                    
                    processedBytes = i + totalMessageLength;
                    i += totalMessageLength - 1; // -1 because loop will increment
                }
                else
                {
                    // Incomplete message, stop processing
                    break;
                }
            }
        }
        
        return processedBytes;
    }

    static void ParseMAVLinkMessage(byte[] message)
    {
        if (message.Length < 8) return;
        //Console.WriteLine($"Message Length: {message.Length}");
        string hex = BitConverter.ToString(message, 0, Math.Min(32, message.Length)).Replace("-", " ");
        byte messageId = message[5]; // Message ID is at index 5
        //Console.WriteLine($"{hex,-48}");
        //Console.WriteLine($"ID: {messageId}");
        switch (messageId)
        { 
            // case MSG_ID_GPS_RAW_INT:
            //     Console.WriteLine($"ID: {messageId}");
            //     ParseGpsRawInt(message);
            //     break;
        //     case MSG_ID_GLOBAL_POSITION_INT:
        //         ParseGlobalPositionInt(message);
        //         break;
        //     case MSG_ID_GPS_STATUS:
        //         ParseGpsStatus(message);
        //         break;
            case MSG_ID_ATTITUDE:
        Console.WriteLine($"ID: {messageId}");
                ParseAttitude(message);
                break;
            // case MSG_ID_HEARTBEAT:
            //     Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] HEARTBEAT received");
            //     break;
        }
    }

    static void ParseGpsRawInt(byte[] message)
    {
        if (message.Length < 38) return; // GPS_RAW_INT payload is 30 bytes + 8 header/checksum

        try
        {
            // Extract GPS data from payload (starting at index 6)
            byte[] payload = new byte[message.Length - 8];
            Array.Copy(message, 6, payload, 0, payload.Length);

            // Parse GPS_RAW_INT fields (little-endian)
            ulong timeUsec = BitConverter.ToUInt64(payload, 0);
            int lat = BitConverter.ToInt32(payload, 8);  // latitude in 1E7 degrees
            int lon = BitConverter.ToInt32(payload, 12); // longitude in 1E7 degrees
            int alt = BitConverter.ToInt32(payload, 16); // altitude in millimeters
            ushort eph = BitConverter.ToUInt16(payload, 20); // GPS HDOP
            ushort epv = BitConverter.ToUInt16(payload, 22); // GPS VDOP
            ushort vel = BitConverter.ToUInt16(payload, 24); // GPS ground speed
            ushort cog = BitConverter.ToUInt16(payload, 26); // Course over ground
            byte fixType = payload[28];
            byte satellitesVisible = payload[29];

            // Convert to human-readable format
            double latitude = lat / 1e7;
            double longitude = lon / 1e7;
            double altitude = alt / 1000.0; // Convert mm to meters
            double speed = vel / 100.0; // Convert cm/s to m/s
            double course = cog / 100.0; // Convert cdeg to degrees

            Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] GPS_RAW_INT:");
            Console.WriteLine($"  Fix Type: {GetGpsFixType(fixType)}");
            Console.WriteLine($"  Satellites: {satellitesVisible}");
            Console.WriteLine($"  Latitude: {latitude:F7}°");
            Console.WriteLine($"  Longitude: {longitude:F7}°");
            Console.WriteLine($"  Altitude: {altitude:F1} m");
            Console.WriteLine($"  Speed: {speed:F2} m/s");
            Console.WriteLine($"  Course: {course:F1}°");
            Console.WriteLine($"  HDOP: {eph / 100.0:F2}");
            Console.WriteLine($"  VDOP: {epv / 100.0:F2}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing GPS_RAW_INT: {ex.Message}");
        }
    }

    static void ParseGlobalPositionInt(byte[] message)
    {
        if (message.Length < 36) return; // GLOBAL_POSITION_INT payload is 28 bytes

        try
        {
            byte[] payload = new byte[message.Length - 8];
            Array.Copy(message, 6, payload, 0, payload.Length);

            uint timeBootMs = BitConverter.ToUInt32(payload, 0);
            int lat = BitConverter.ToInt32(payload, 4);
            int lon = BitConverter.ToInt32(payload, 8);
            int alt = BitConverter.ToInt32(payload, 12);
            int relativeAlt = BitConverter.ToInt32(payload, 16);
            short vx = BitConverter.ToInt16(payload, 20);
            short vy = BitConverter.ToInt16(payload, 22);
            short vz = BitConverter.ToInt16(payload, 24);
            ushort hdg = BitConverter.ToUInt16(payload, 26);

            double latitude = lat / 1e7;
            double longitude = lon / 1e7;
            double altitude = alt / 1000.0;
            double relAltitude = relativeAlt / 1000.0;
            double heading = hdg / 100.0;

            Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] GLOBAL_POSITION_INT:");
            Console.WriteLine($"  Latitude: {latitude:F7}°");
            Console.WriteLine($"  Longitude: {longitude:F7}°");
            Console.WriteLine($"  Altitude (MSL): {altitude:F1} m");
            Console.WriteLine($"  Altitude (Rel): {relAltitude:F1} m");
            Console.WriteLine($"  Heading: {heading:F1}°");
            Console.WriteLine($"  Velocity: X={vx / 100.0:F2} Y={vy / 100.0:F2} Z={vz / 100.0:F2} m/s");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing GLOBAL_POSITION_INT: {ex.Message}");
        }
    }

    static void ParseGpsStatus(byte[] message)
    {
        if (message.Length < 109) return; // GPS_STATUS has variable length

        try
        {
            byte[] payload = new byte[message.Length - 8];
            Array.Copy(message, 6, payload, 0, payload.Length);

            byte satellitesVisible = payload[0];
            
            Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] GPS_STATUS:");
            Console.WriteLine($"  Satellites visible: {satellitesVisible}");
            
            // Parse individual satellite info if needed
            for (int i = 0; i < Math.Min((int)satellitesVisible, 20); i++)
            {
                if (payload.Length > 1 + i * 4)
                {
                    byte satId = payload[1 + i];
                    byte used = payload[21 + i];
                    byte elevation = payload[41 + i];
                    byte azimuth = payload[61 + i];
                    byte snr = payload[81 + i];
                    
                    if (used > 0)
                    {
                        Console.WriteLine($"    Sat {satId}: Used, Elev={elevation}°, Az={azimuth}°, SNR={snr}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing GPS_STATUS: {ex.Message}");
        }
    }

    static void ParseAttitude(byte[] message)
    {
        if (message.Length < 36) return; // ATTITUDE payload is 28 bytes + 8 header/checksum

        try
        {
            byte[] payload = new byte[message.Length - 8];
            Array.Copy(message, 6, payload, 0, payload.Length);

            // Parse ATTITUDE fields (little-endian)
            uint timeBootMs = BitConverter.ToUInt32(payload, 0);
            float roll = BitConverter.ToSingle(payload, 4);     // Roll angle in radians
            float pitch = BitConverter.ToSingle(payload, 8);    // Pitch angle in radians
            float yaw = BitConverter.ToSingle(payload, 12);     // Yaw angle in radians
            float rollspeed = BitConverter.ToSingle(payload, 16);  // Roll angular speed in rad/s
            float pitchspeed = BitConverter.ToSingle(payload, 20); // Pitch angular speed in rad/s
            float yawspeed = BitConverter.ToSingle(payload, 24);   // Yaw angular speed in rad/s

            // Convert radians to degrees
            double rollDeg = roll * 180.0 / Math.PI;
            double pitchDeg = pitch * 180.0 / Math.PI;
            double yawDeg = yaw * 180.0 / Math.PI;
            
            // Normalize yaw to 0-360 degrees
            if (yawDeg < 0) yawDeg += 360.0;

            Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] ATTITUDE:");
            Console.WriteLine($"  Roll:  {rollDeg:F2}° ({rollspeed:F3} rad/s)");
            Console.WriteLine($"  Pitch: {pitchDeg:F2}° ({pitchspeed:F3} rad/s)");
            Console.WriteLine($"  Yaw:   {yawDeg:F2}° ({yawspeed:F3} rad/s)");
            
            // Show aircraft orientation in simple terms
            string rollStatus = Math.Abs(rollDeg) < 5 ? "Level" : 
                               rollDeg > 0 ? $"Right bank {Math.Abs(rollDeg):F1}°" : 
                               $"Left bank {Math.Abs(rollDeg):F1}°";
            
            string pitchStatus = Math.Abs(pitchDeg) < 5 ? "Level" : 
                                pitchDeg > 0 ? $"Nose up {pitchDeg:F1}°" : 
                                $"Nose down {Math.Abs(pitchDeg):F1}°";
            
            Console.WriteLine($"  Status: {rollStatus}, {pitchStatus}");
            Console.WriteLine($"  Heading: {GetCardinalDirection(yawDeg)} ({yawDeg:F1}°)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing ATTITUDE: {ex.Message}");
        }
    }

    static string GetGpsFixType(byte fixType)
    {
        return fixType switch
        {
            0 => "No GPS",
            1 => "No Fix",
            2 => "2D Fix",
            3 => "3D Fix",
            4 => "DGPS",
            5 => "RTK Float",
            6 => "RTK Fixed",
            7 => "Static",
            8 => "PPP",
            _ => $"Unknown ({fixType})"
        };
    }

    static string GetCardinalDirection(double heading)
    {
        string[] directions = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", 
                               "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
        int index = (int)Math.Round(heading / 22.5) % 16;
        return directions[index];
    }
}