using System;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable enable

public class MainForm : Form
{
    // MAVLink message IDs
    private const byte MSG_ID_ATTITUDE = 30;
    private const byte MSG_ID_GPS_RAW_INT = 24;
    private const byte MSG_ID_GLOBAL_POSITION_INT = 33;

    // UI Elements
    private Label _rollLabel, _pitchLabel, _yawLabel;
    private Label _latLabel, _lonLabel, _altLabel, _fixLabel, _satsLabel;
    private Label _relAltLabel, _headingLabel;
    private StatusStrip _statusStrip;
    private ToolStripStatusLabel _statusLabel;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    public string PortName { get; set; } = "";
    public int BaudRate { get; set; } = 0;

    public MainForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "DronePulse Telemetry";
        this.Size = new Size(600, 400);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        // Attitude GroupBox
        var attitudeBox = new GroupBox { Text = "Attitude", Location = new Point(10, 10), Size = new Size(250, 120) };
        _rollLabel = new Label { Text = "Roll:  ---", Location = new Point(10, 20), Width = 230 };
        _pitchLabel = new Label { Text = "Pitch: ---", Location = new Point(10, 50), Width = 230 };
        _yawLabel = new Label { Text = "Yaw:   ---", Location = new Point(10, 80), Width = 230 };
        attitudeBox.Controls.Add(_rollLabel);
        attitudeBox.Controls.Add(_pitchLabel);
        attitudeBox.Controls.Add(_yawLabel);

        // GPS GroupBox
        var gpsBox = new GroupBox { Text = "GPS Raw", Location = new Point(10, 140), Size = new Size(250, 180) };
        _fixLabel = new Label { Text = "Fix: ---, Sats: --", Location = new Point(10, 20), Width = 230 };
        _latLabel = new Label { Text = "Lat: ---", Location = new Point(10, 50), Width = 230 };
        _lonLabel = new Label { Text = "Lon: ---", Location = new Point(10, 80), Width = 230 };
        _altLabel = new Label { Text = "Alt: ---", Location = new Point(10, 110), Width = 230 };
        _satsLabel = new Label { Text = "", Location = new Point(10, 140), Width = 230 };
        gpsBox.Controls.Add(_fixLabel);
        gpsBox.Controls.Add(_latLabel);
        gpsBox.Controls.Add(_lonLabel);
        gpsBox.Controls.Add(_altLabel);
        gpsBox.Controls.Add(_satsLabel);

        // Global Position GroupBox
        var posBox = new GroupBox { Text = "Global Position", Location = new Point(270, 10), Size = new Size(300, 120) };
        _relAltLabel = new Label { Text = "Rel Alt: ---", Location = new Point(10, 20), Width = 280 };
        _headingLabel = new Label { Text = "Heading: ---", Location = new Point(10, 50), Width = 280 };
        posBox.Controls.Add(_relAltLabel);
        posBox.Controls.Add(_headingLabel);

        // Status Bar
        _statusStrip = new StatusStrip();
        _statusLabel = new ToolStripStatusLabel("Connecting...");
        _statusStrip.Items.Add(_statusLabel);

        this.Controls.Add(attitudeBox);
        this.Controls.Add(gpsBox);
        this.Controls.Add(posBox);
        this.Controls.Add(_statusStrip);

        this.Load += MainForm_Load;
        this.FormClosing += MainForm_FormClosing;
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        _statusLabel.Text = $"Connecting to {PortName} at {BaudRate} baud...";
        Task.Run(() => MavlinkReceiver(PortName, BaudRate, _cts.Token));
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _cts.Cancel();
    }

    private async Task MavlinkReceiver(string portName, int baudRate, CancellationToken token)
    {
        try
        {
            using var serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();

            if (!serialPort.IsOpen) return;

            this.Invoke((MethodInvoker)delegate { _statusLabel.Text = $"Connected to {portName}. Waiting for data..."; });

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
            this.Invoke((MethodInvoker)delegate { _statusLabel.Text = $"Error: {ex.Message}"; });
        }
        finally
        {
            if (this.IsHandleCreated)
            {
                this.Invoke((MethodInvoker)delegate { _statusLabel.Text = "Disconnected"; });
            }
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

        this.Invoke((MethodInvoker)delegate
        {
            _rollLabel.Text = $"Roll:  {rollDeg,7:F2}°";
            _pitchLabel.Text = $"Pitch: {pitchDeg,7:F2}°";
            _yawLabel.Text = $"Yaw:   {yawDeg,7:F2}°";
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

        this.Invoke((MethodInvoker)delegate
        {
            _fixLabel.Text = $"Fix: {GetGpsFixType(fixType)}, Sats: {satellitesVisible}";
            _latLabel.Text = $"Lat: {latitude,11:F7}°";
            _lonLabel.Text = $"Lon: {longitude,11:F7}°";
            _altLabel.Text = $"Alt: {altitude,9:F1} m";
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

        this.Invoke((MethodInvoker)delegate
        {
            _relAltLabel.Text = $"Rel Alt: {relAltitude,7:F1} m";
            _headingLabel.Text = $"Heading: {heading,7:F1}° ({GetCardinalDirection(heading)})";
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
