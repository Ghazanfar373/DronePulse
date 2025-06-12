using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Terminal.Gui;

#nullable enable

class DronePulse
{
    // MAVLink message IDs
    const byte MSG_ID_ATTITUDE = 30;
    const byte MSG_ID_GPS_RAW_INT = 24;
    const byte MSG_ID_GLOBAL_POSITION_INT = 33;

    // UI Elements
    private Label? _rollLabel, _pitchLabel, _yawLabel;
    private Label? _latLabel, _lonLabel, _altLabel, _fixLabel, _satsLabel;
    private Label? _relAltLabel, _headingLabel;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    static void Main(string[] args)
    {
        var dronePulse = new DronePulse();
        dronePulse.Run(args);
    }

    private void Run(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string portName = configuration.GetValue<string>("SerialPortSettings:PortName");
        int baudRate = configuration.GetValue<int>("SerialPortSettings:BaudRate");

        if (string.IsNullOrEmpty(portName))
        {
            // Fallback for environments where a console is not available for direct output
            MessageBox.ErrorQuery("Configuration Error", "PortName is not configured in appsettings.json", "Ok");
            return;
        }

        Application.Init();

        var top = Application.Top;
        var topWindow = new Window("DronePulse Telemetry")
        {
            X = 0,
            Y = 1, // Leave space for the menu
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        CreateUI(topWindow);
        top.Add(topWindow);

        var menu = new MenuBar(new MenuBarItem[]
        {
            new MenuBarItem ("_File", new MenuItem []
            {
                new MenuItem ("_Quit", "", () => { _cts.Cancel(); Application.RequestStop(); }, null, null, Key.Q | Key.CtrlMask)
            })
        });
        top.Add(menu);

        var statusBar = new StatusBar(new StatusItem[]
        {
            new StatusItem(Key.CtrlMask | Key.Q, "~^Q~ Quit", () => { _cts.Cancel(); Application.RequestStop(); }),
            new StatusItem(Key.Null, $"Port: {portName}", null),
            new StatusItem(Key.Null, $"Baud: {baudRate}", null)
        });
        top.Add(statusBar);

        Task.Run(() => MavlinkReceiver(portName, baudRate, _cts.Token));

        Application.Run();
        Application.Shutdown();
    }

    private void CreateUI(Window topWindow)
    {
        // Attitude Frame
        var attitudeFrame = new FrameView("Attitude")
        {
            X = 1,
            Y = 1,
            Width = 35,
            Height = 5
        };
        _rollLabel = new Label("Roll:  ---") { X = 1, Y = 0 };
        _pitchLabel = new Label("Pitch: ---") { X = 1, Y = 1 };
        _yawLabel = new Label("Yaw:   ---") { X = 1, Y = 2 };
        attitudeFrame.Add(_rollLabel, _pitchLabel, _yawLabel);

        // GPS Frame
        var gpsFrame = new FrameView("GPS Raw")
        {
            X = 1,
            Y = Pos.Bottom(attitudeFrame) + 1,
            Width = 35,
            Height = 7
        };
        _fixLabel = new Label("Fix: ---, Sats: --") { X = 1, Y = 0 };
        _latLabel = new Label("Lat: ---") { X = 1, Y = 1 };
        _lonLabel = new Label("Lon: ---") { X = 1, Y = 2 };
        _altLabel = new Label("Alt: ---") { X = 1, Y = 3 };
        _satsLabel = new Label("") { X = 1, Y = 4 }; // For satellite details
        gpsFrame.Add(_fixLabel, _latLabel, _lonLabel, _altLabel, _satsLabel);
        
        // Global Position Frame
        var posFrame = new FrameView("Global Position")
        {
            X = Pos.Right(attitudeFrame) + 1,
            Y = 1,
            Width = 40,
            Height = 5
        };
        _relAltLabel = new Label("Rel Alt: ---") { X = 1, Y = 0 };
        _headingLabel = new Label("Heading: ---") { X = 1, Y = 1 };
        posFrame.Add(_relAltLabel, _headingLabel);

        topWindow.Add(attitudeFrame, gpsFrame, posFrame);
    }

    private async Task MavlinkReceiver(string portName, int baudRate, CancellationToken token)
    {
        try
        {
            using var serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();

            if (!serialPort.IsOpen) return;

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
            Application.MainLoop.Invoke(() => MessageBox.ErrorQuery("Connection Error", ex.Message, "Ok"));
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

        double rollDeg = roll * 180.0 / Math.PI;
        double pitchDeg = pitch * 180.0 / Math.PI;
        double yawDeg = yaw * 180.0 / Math.PI;
        if (yawDeg < 0) yawDeg += 360.0;

        Application.MainLoop.Invoke(() =>
        {
            _rollLabel?.setText($"Roll:  {rollDeg,7:F2}°");
            _pitchLabel?.setText($"Pitch: {pitchDeg,7:F2}°");
            _yawLabel?.setText($"Yaw:   {yawDeg,7:F2}°");
        });
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

        double latitude = lat / 1e7;
        double longitude = lon / 1e7;
        double altitude = alt / 1000.0;

        Application.MainLoop.Invoke(() =>
        {
            _fixLabel?.setText($"Fix: {GetGpsFixType(fixType)}, Sats: {satellitesVisible}");
            _latLabel?.setText($"Lat: {latitude,11:F7}°");
            _lonLabel?.setText($"Lon: {longitude,11:F7}°");
            _altLabel?.setText($"Alt: {altitude,9:F1} m");
        });
    }

    private void ParseGlobalPositionInt(byte[] message)
    {
        if (message.Length < 36) return;
        byte[] payload = new byte[message.Length - 8];
        Array.Copy(message, 6, payload, 0, payload.Length);

        int relativeAlt = BitConverter.ToInt32(payload, 16);
        ushort hdg = BitConverter.ToUInt16(payload, 26);

        double relAltitude = relativeAlt / 1000.0;
        double heading = hdg / 100.0;

        Application.MainLoop.Invoke(() =>
        {
            _relAltLabel?.setText($"Rel Alt: {relAltitude,7:F1} m");
            _headingLabel?.setText($"Heading: {heading,7:F1}° ({GetCardinalDirection(heading)})");
        });
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

public static class LabelExtensions
{
    public static void setText(this Label label, string text)
    {
        label.Text = text;
    }
}