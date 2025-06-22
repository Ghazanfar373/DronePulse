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
            
            // Set up the form
            this.Text = "DronePulse - Telemetry Monitor";
            this.MinimumSize = new Size(1000, 700);
            
            // Initialize the HUD controls first
            InitializeHudControls();
            
            // Position other controls to make room for the HUD
            if (hudBox != null && attitudeBox != null)
            {
                attitudeBox.Location = new Point(hudBox.Right + 20, 12);
            }
            
            if (hudBox != null && gpsBox != null && attitudeBox != null)
            {
                gpsBox.Location = new Point(hudBox.Right + 20, attitudeBox.Bottom + 10);
            }
            
            if (hudBox != null && posBox != null && gpsBox != null)
            {
                posBox.Location = new Point(hudBox.Right + 20, gpsBox.Bottom + 10);
            }
            
            // Set up data bindings after controls are initialized and positioned
            SetupHudDataBindings();
            
            // Set up connection controls
            if (connectButton != null)
                connectButton.Click += ConnectButton_Click;
                
            if (disconnectButton != null)
                disconnectButton.Click += DisconnectButton_Click;

            // Initialize telemetry data bindings
            _telemetryData.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(TelemetryData.Status) && statusStrip != null && statusLabel != null)
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

            // Set up connection type combo box
            if (connectionTypeComboBox != null)
            {
                connectionTypeComboBox.Items.Add("Serial");
                connectionTypeComboBox.Items.Add("UDP");
                connectionTypeComboBox.Items.Add("TCP");
                connectionTypeComboBox.SelectedIndex = 0;
                connectionTypeComboBox.SelectedIndexChanged += ConnectionTypeComboBox_SelectedIndexChanged;
            }

            // Initialize COM port list and baud rate
            if (comPortComboBox != null && baudRateTextBox != null)
            {
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
            
            // Initialize with some test data (for debugging)
            //_telemetryData.GroundSpeed = 10.5;
            //_telemetryData.AirSpeed = 12.3;
            //_telemetryData.ClimbRate = 1.2;
            //_telemetryData.FlightMode = "STABILIZE";
            //_telemetryData.BatteryVoltage = 12.6;
            //_telemetryData.BatteryRemaining = 0.85;
            //_telemetryData.CurrentWaypoint = 3;
            //_telemetryData.TotalWaypoints = 10;
            //_telemetryData.DistanceToWaypoint = 125.7;
        }

        private void ConnectButton_Click(object? sender, EventArgs e)
        {
            if (connectionTypeComboBox == null) return;

            try
            {
                // Cancel any existing connection
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                IConnection? connection = null;
                string connectionType = connectionTypeComboBox.Text;

                switch (connectionType)
                {
                    case "Serial":
                        if (string.IsNullOrEmpty(comPortComboBox?.Text))
                        {
                            MessageBox.Show("Please select a COM port", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (!int.TryParse(baudRateTextBox?.Text, out int baudRate))
                        {
                            baudRate = 57600;
                        }
                        connection = new SerialConnection(comPortComboBox.Text, baudRate);
                        break;
                    case "UDP":
                        // if (string.IsNullOrEmpty(udpLocalPortTextBox?.Text))
                        // {
                        //     MessageBox.Show("Please enter a local port", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //     return;
                        // }
                        // if (!int.TryParse(udpLocalPortTextBox.Text, out int localPort))
                        // {
                        //     localPort = 14550;
                        // }
                        //connection = new UdpConnection(localPort);
                        break;
                    case "TCP":
                        if (string.IsNullOrEmpty(tcpIpAddressTextBox?.Text))
                        {
                            MessageBox.Show("Please enter an IP address", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (!int.TryParse(tcpPortTextBox?.Text, out int tcpPort))
                        {
                            tcpPort = 5760;
                        }
                        connection = new TcpConnection(tcpIpAddressTextBox.Text, tcpPort);
                        break;
                    default:
                        throw new NotSupportedException($"Connection type {connectionType} is not supported");
                }

                if (connection != null)
                {
                    // Dispose of any existing controller
                   // _mavlinkController?.Dispose();
                    
                    // Create new controller and start it
                    _mavlinkController = new MavlinkController(_telemetryData, connection);
                    
                    // Start telemetry reading in the background
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await _mavlinkController.Start(_cts.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            // This is expected when disconnecting
                            Console.WriteLine("Telemetry reading was cancelled");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error in telemetry reading: {ex.Message}");
                            // Update UI on the UI thread
                            // if (statusLabel != null && !statusLabel.IsDisposed)
                            // {
                            //     // statusLabel.Invoke((MethodInvoker)delegate {
                            //     //     statusLabel.Text = $"Error: {ex.Message}";
                            //     });
                            //}
                        }
            
                        finally
                        {
                            // Update UI on the UI thread
                            Invoke((MethodInvoker)delegate {
                                if (connectButton != null && !connectButton.IsDisposed) 
                                    connectButton.Enabled = true;
                                if (disconnectButton != null && !disconnectButton.IsDisposed) 
                                    disconnectButton.Enabled = false;
                                if (connectionTypeComboBox != null && !connectionTypeComboBox.IsDisposed)
                                    connectionTypeComboBox.Enabled = true;
                            });
                        }
                    }, _cts.Token);

                    // Update UI
                    if (connectButton != null) connectButton.Enabled = false;
                    if (disconnectButton != null) disconnectButton.Enabled = true;
                    if (connectionTypeComboBox != null) connectionTypeComboBox.Enabled = false;
                    
                    _telemetryData.Status = "Connected";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect: {ex.Message}");
                MessageBox.Show($"Failed to connect: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Reset UI on error
                if (connectButton != null) connectButton.Enabled = true;
                if (disconnectButton != null) disconnectButton.Enabled = false;
                if (connectionTypeComboBox != null) connectionTypeComboBox.Enabled = true;
                
                _telemetryData.Status = "Connection failed";
            }
        }

        private void ConnectionTypeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (connectionTypeComboBox == null || serialPanel == null || udpPanel == null || tcpPanel == null)
                return;
                
            int index = connectionTypeComboBox.SelectedIndex;
            serialPanel.Visible = index == 0;
            udpPanel.Visible = index == 1;
            tcpPanel.Visible = index == 2;
        }

        private async void DisconnectButton_Click(object? sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Disconnecting...");
                
                // Cancel the token to signal the telemetry reading to stop
                _cts?.Cancel();
                
                // Wait a moment for the connection to close cleanly
                await Task.Delay(500);
                
                // Dispose of the controller
               // _mavlinkController?.Dispose();
                _mavlinkController = null;
                
                // Update UI
                if (connectButton != null && !connectButton.IsDisposed) 
                    connectButton.Enabled = true;
                if (disconnectButton != null && !disconnectButton.IsDisposed) 
                    disconnectButton.Enabled = false;
                if (connectionTypeComboBox != null && !connectionTypeComboBox.IsDisposed)
                    connectionTypeComboBox.Enabled = true;
                
                _telemetryData.Status = "Disconnected";
                Console.WriteLine("Disconnected successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during disconnect: {ex.Message}");
                MessageBox.Show($"Error disconnecting: {ex.Message}", "Disconnect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeHudControls()
        {
            try
            {
                // Create and configure the HUD group box
                hudBox = new GroupBox();
                hudBox.SuspendLayout();
                hudBox.Location = new System.Drawing.Point(12, 12);
                hudBox.Name = "hudBox";
                hudBox.Size = new System.Drawing.Size(300, 250);
                hudBox.TabIndex = 6;
                hudBox.TabStop = false;
                hudBox.Text = "Flight HUD";
                
                // Create and configure HUD labels with improved styling
                groundSpeedLabel = CreateHudLabel("Ground: 0.0 m/s", 10, 25);
                airSpeedLabel = CreateHudLabel("Air: 0.0 m/s", 10, 55);
                climbRateLabel = CreateHudLabel("Climb: 0.0 m/s", 10, 85);
                flightModeLabel = CreateHudLabel("Mode: ---", 10, 115);
                batteryLabel = CreateHudLabel("Battery: 0.0V (0%)", 10, 145);
                waypointLabel = CreateHudLabel("Waypoint: 0/0", 10, 175);
                distanceToWaypointLabel = CreateHudLabel("Distance: 0.0 m", 10, 205);
                
                // Add labels to the HUD group box
                hudBox.Controls.Add(groundSpeedLabel);
                hudBox.Controls.Add(airSpeedLabel);
                hudBox.Controls.Add(climbRateLabel);
                hudBox.Controls.Add(flightModeLabel);
                hudBox.Controls.Add(batteryLabel);
                hudBox.Controls.Add(waypointLabel);
                hudBox.Controls.Add(distanceToWaypointLabel);
                
                // Add the HUD group box to the form
                this.Controls.Add(hudBox);
                
                // Ensure the HUD is visible and on top
                hudBox.BringToFront();
                hudBox.Visible = true;
                hudBox.ResumeLayout(false);
                
                // Force a refresh of the HUD
                hudBox.Refresh();
                
                Console.WriteLine("HUD controls initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing HUD controls: {ex.Message}");
                if (statusLabel != null)
                    statusLabel.Text = $"HUD Error: {ex.Message}";
            }
        }
        
        private Label CreateHudLabel(string text, int x, int y)
        {
            var label = new Label
            {
                Location = new System.Drawing.Point(x, y),
                Name = $"hudLabel_{x}_{y}",
                Size = new System.Drawing.Size(280, 25),
                Text = text,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            return label;
        }
        
        private void SetupHudDataBindings()
        {
            try
            {
                Console.WriteLine("Setting up HUD data bindings...");
                
                // Clear any existing bindings
                groundSpeedLabel?.DataBindings.Clear();
                airSpeedLabel?.DataBindings.Clear();
                climbRateLabel?.DataBindings.Clear();
                flightModeLabel?.DataBindings.Clear();
                batteryLabel?.DataBindings.Clear();
                waypointLabel?.DataBindings.Clear();
                distanceToWaypointLabel?.DataBindings.Clear();
                
                // Set up new bindings with formatting and null checking
                if (groundSpeedLabel != null)
                {
                    groundSpeedLabel.DataBindings.Add("Text", _telemetryData, "GroundSpeedText", false, DataSourceUpdateMode.OnPropertyChanged);
                    Console.WriteLine("Bound GroundSpeedText");
                }
                
                if (airSpeedLabel != null)
                {
                    airSpeedLabel.DataBindings.Add("Text", _telemetryData, "AirSpeedText", false, DataSourceUpdateMode.OnPropertyChanged);
                    Console.WriteLine("Bound AirSpeedText");
                }
                
                if (climbRateLabel != null)
                {
                    climbRateLabel.DataBindings.Add("Text", _telemetryData, "ClimbRateText", false, DataSourceUpdateMode.OnPropertyChanged);
                    Console.WriteLine("Bound ClimbRateText");
                }
                
                if (flightModeLabel != null)
                {
                    flightModeLabel.DataBindings.Add("Text", _telemetryData, "FlightModeText", false, DataSourceUpdateMode.OnPropertyChanged);
                    Console.WriteLine("Bound FlightModeText");
                }
                
                if (batteryLabel != null)
                {
                    batteryLabel.DataBindings.Add("Text", _telemetryData, "BatteryText", false, DataSourceUpdateMode.OnPropertyChanged);
                    Console.WriteLine("Bound BatteryText");
                }
                
                if (waypointLabel != null)
                {
                    waypointLabel.DataBindings.Add("Text", _telemetryData, "WaypointText", false, DataSourceUpdateMode.OnPropertyChanged);
                    Console.WriteLine("Bound WaypointText");
                }
                
                if (distanceToWaypointLabel != null)
                {
                    distanceToWaypointLabel.DataBindings.Add("Text", _telemetryData, "DistanceToWaypointText", false, DataSourceUpdateMode.OnPropertyChanged);
                    Console.WriteLine("Bound DistanceToWaypointText");
                }
                
                Console.WriteLine("HUD data bindings set up successfully");
                
                // Force an immediate refresh of all HUD elements
                RefreshHud();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up HUD data bindings: {ex.Message}");
                if (statusLabel != null)
                    statusLabel.Text = $"Binding Error: {ex.Message}";
            }
        }
        
        private void RefreshHud()
        {
            try
            {
                if (hudBox == null) return;
                
                // Force a refresh of the HUD controls
                if (hudBox.InvokeRequired)
                {
                    hudBox.Invoke((MethodInvoker)delegate {
                        hudBox.Refresh();
                        if (groundSpeedLabel != null) groundSpeedLabel.Refresh();
                        if (airSpeedLabel != null) airSpeedLabel.Refresh();
                        if (climbRateLabel != null) climbRateLabel.Refresh();
                        if (flightModeLabel != null) flightModeLabel.Refresh();
                        if (batteryLabel != null) batteryLabel.Refresh();
                        if (waypointLabel != null) waypointLabel.Refresh();
                        if (distanceToWaypointLabel != null) distanceToWaypointLabel.Refresh();
                    });
                }
                else
                {
                    hudBox.Refresh();
                    groundSpeedLabel?.Refresh();
                    airSpeedLabel?.Refresh();
                    climbRateLabel?.Refresh();
                    flightModeLabel?.Refresh();
                    batteryLabel?.Refresh();
                    waypointLabel?.Refresh();
                    distanceToWaypointLabel?.Refresh();
                }
                
                Console.WriteLine("HUD refreshed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing HUD: {ex.Message}");
            }
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            // Data Bindings for existing controls
            rollLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.RollText));
            pitchLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.PitchText));
            yawLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.YawText));
            fixLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.GpsStatusText));
            latLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.LatitudeText));
            lonLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.LongitudeText));
            altLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.AltitudeText));
            relAltLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.RelativeAltitudeText));
            headingLabel.DataBindings.Add("Text", _telemetryData, nameof(TelemetryData.HeadingText));

            // Initialize HUD with default values
            UpdateHudDisplay();
        }

        private void UpdateHudDisplay()
        {
            // This method can be used to update the HUD display if needed
            // The actual updates are handled through data bindings
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _cts.Cancel();
            _connection?.Dispose();
        }
    }
}
