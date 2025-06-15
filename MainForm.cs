using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DronePulse.Controllers;
using DronePulse.Models;

#nullable enable

namespace DronePulse
{
    public partial class MainForm : Form
    {
        private readonly TelemetryData _telemetryData = new TelemetryData();
        private MavlinkController? _mavlinkController;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public string PortName { get; set; } = "";
        public int BaudRate { get; set; } = 0;

        public MainForm()
        {
            InitializeComponent();

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
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            // Initialize controller with properties set by Program.cs
            _mavlinkController = new MavlinkController(_telemetryData, PortName, BaudRate);
            Task.Run(() => _mavlinkController.Start(_cts.Token));
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _cts.Cancel();
        }
    }
}
