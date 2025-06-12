using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DronePulse.Controllers;
using DronePulse.Models;

#nullable enable

public class MainForm : Form
{
    private readonly TelemetryData _telemetryData = new TelemetryData();
    private readonly MavlinkController _mavlinkController;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();

    public string PortName { get; set; } = "";
    public int BaudRate { get; set; } = 0;

    public MainForm()
    {
        InitializeComponent();
        _mavlinkController = new MavlinkController(_telemetryData, PortName, BaudRate);
    }

    private void InitializeComponent()
    {
        this.Text = "DronePulse Telemetry";
        this.Size = new Size(600, 400);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        var attitudeBox = new GroupBox { Text = "Attitude", Location = new Point(10, 10), Size = new Size(250, 120) };
        var rollLabel = new Label { Location = new Point(10, 20), Width = 230 };
        var pitchLabel = new Label { Location = new Point(10, 50), Width = 230 };
        var yawLabel = new Label { Location = new Point(10, 80), Width = 230 };
        attitudeBox.Controls.Add(rollLabel);
        attitudeBox.Controls.Add(pitchLabel);
        attitudeBox.Controls.Add(yawLabel);

        var gpsBox = new GroupBox { Text = "GPS Raw", Location = new Point(10, 140), Size = new Size(250, 180) };
        var fixLabel = new Label { Location = new Point(10, 20), Width = 230 };
        var latLabel = new Label { Location = new Point(10, 50), Width = 230 };
        var lonLabel = new Label { Location = new Point(10, 80), Width = 230 };
        var altLabel = new Label { Location = new Point(10, 110), Width = 230 };
        gpsBox.Controls.Add(fixLabel);
        gpsBox.Controls.Add(latLabel);
        gpsBox.Controls.Add(lonLabel);
        gpsBox.Controls.Add(altLabel);

        var posBox = new GroupBox { Text = "Global Position", Location = new Point(270, 10), Size = new Size(300, 120) };
        var relAltLabel = new Label { Location = new Point(10, 20), Width = 280 };
        var headingLabel = new Label { Location = new Point(10, 50), Width = 280 };
        posBox.Controls.Add(relAltLabel);
        posBox.Controls.Add(headingLabel);

        var statusStrip = new StatusStrip();
        var statusLabel = new ToolStripStatusLabel();
        statusStrip.Items.Add(statusLabel);

        this.Controls.Add(attitudeBox);
        this.Controls.Add(gpsBox);
        this.Controls.Add(posBox);
        this.Controls.Add(statusStrip);

        // Data Bindings
        rollLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.RollText));
        pitchLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.PitchText));
        yawLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.YawText));

        fixLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.GpsStatusText));

        latLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.LatitudeText));
        lonLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.LongitudeText));
        altLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.AltitudeText));

        relAltLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.RelativeAltitudeText));
        headingLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.HeadingText));

        _telemetryData.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(TelemetryData.Status))
            {
                // Ensure UI updates are on the UI thread
                if (statusStrip.InvokeRequired)
                {
                    statusStrip.Invoke((MethodInvoker)delegate { statusLabel.Text = _telemetryData.Status; });
                }
                else
                {
                    statusLabel.Text = _telemetryData.Status;
                }
            }
        };

        this.Load += MainForm_Load;
        this.FormClosing += MainForm_FormClosing;
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        // Re-initialize controller with properties set by Program.cs
        var controller = new MavlinkController(_telemetryData, PortName, BaudRate);
        Task.Run(() => controller.Start(_cts.Token));
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _cts.Cancel();
    }
}
