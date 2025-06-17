using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DronePulse.Connections;
using DronePulse.Controllers;
using DronePulse.Interfaces;
using DronePulse.Models;
using System.IO.Ports;

#nullable enable

namespace DronePulse
{
    public partial class MainForm : Form
    {
        private readonly TelemetryData _telemetryData = new TelemetryData();
        private MavlinkController? _mavlinkController;
        private IConnection? _connection;
        private  CancellationTokenSource _cts = new CancellationTokenSource();

        public string PortName { get; set; } = "";
        public int BaudRate { get; set; } = 0;

        public MainForm()
        {
            InitializeComponent();

            connectButton.Click += ConnectButton_Click;
            disconnectButton.Click += DisconnectButton_Click;

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

            connectionTypeComboBox.Items.Add("Serial");
            connectionTypeComboBox.Items.Add("UDP");
            connectionTypeComboBox.Items.Add("TCP");
            connectionTypeComboBox.SelectedIndex = 0;
            connectionTypeComboBox.SelectedIndexChanged += ConnectionTypeComboBox_SelectedIndexChanged;

            comPortComboBox.Items.AddRange(SerialPort.GetPortNames());
            if (comPortComboBox.Items.Contains(PortName))
            {
                comPortComboBox.SelectedItem = PortName;
            }
            else if (comPortComboBox.Items.Count > 0)
            {
                comPortComboBox.SelectedIndex = 0;
            }
            baudRateTextBox.Text = BaudRate.ToString();
        }

        private void ConnectButton_Click(object? sender, EventArgs e)
        {
            try
            {
                _connection?.Dispose(); // Dispose previous connection if any
                IConnection? connection = null;
                string selectedType = connectionTypeComboBox.SelectedItem.ToString() ?? "Serial";

                switch (selectedType)
                {
                    case "Serial":
                        PortName = comPortComboBox.Text;
                        BaudRate = int.Parse(baudRateTextBox.Text);
                        connection = new SerialConnection(PortName, BaudRate);
                        break;
                    case "UDP":
                        int udpPort = int.Parse(udpPortTextBox.Text);
                        connection = new UdpConnection(udpPort);
                        break;
                    case "TCP":
                        string host = tcpIpAddressTextBox.Text;
                        int port = int.Parse(tcpPortTextBox.Text);
                        connection = new TcpConnection(host, port);
                        break;
                    default:
                        PortName = comPortComboBox.Text;
                        BaudRate = int.Parse(baudRateTextBox.Text);
                        connection = new SerialConnection(PortName, BaudRate);
                        break;
                }

                _connection = connection;
                _mavlinkController = new MavlinkController(_telemetryData, _connection);
                _cts = new CancellationTokenSource();
                Task.Run(() => _mavlinkController.Start(_cts.Token));

                connectButton.Enabled = false;
                disconnectButton.Enabled = true;
                connectionTypeComboBox.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConnectionTypeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            string selectedType = connectionTypeComboBox.SelectedItem.ToString() ?? "Serial";
            serialPanel.Visible = selectedType == "Serial";
            udpPanel.Visible = selectedType == "UDP";
            tcpPanel.Visible = selectedType == "TCP";
        }

        private void DisconnectButton_Click(object? sender, EventArgs e)
        {
            _cts.Cancel();
            _connection?.Dispose();
            _connection = null;
            connectButton.Enabled = true;
            disconnectButton.Enabled = false;
            connectionTypeComboBox.Enabled = true;
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
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
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _cts.Cancel();
            _connection?.Dispose();
        }
    }
}
