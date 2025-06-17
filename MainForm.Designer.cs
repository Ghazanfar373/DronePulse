#nullable enable

using System.Drawing;
using System.Windows.Forms;


namespace DronePulse
{
    public partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.attitudeBox = new System.Windows.Forms.GroupBox();
            this.rollLabel = new System.Windows.Forms.Label();
            this.pitchLabel = new System.Windows.Forms.Label();
            this.yawLabel = new System.Windows.Forms.Label();
            this.gpsBox = new System.Windows.Forms.GroupBox();
            this.fixLabel = new System.Windows.Forms.Label();
            this.latLabel = new System.Windows.Forms.Label();
            this.lonLabel = new System.Windows.Forms.Label();
            this.altLabel = new System.Windows.Forms.Label();
            this.posBox = new System.Windows.Forms.GroupBox();
            this.relAltLabel = new System.Windows.Forms.Label();
            this.headingLabel = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.connectionBox = new System.Windows.Forms.GroupBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.connectionTypeComboBox = new System.Windows.Forms.ComboBox();
            this.serialPanel = new System.Windows.Forms.Panel();
            this.comPortLabel = new System.Windows.Forms.Label();
            this.comPortComboBox = new System.Windows.Forms.ComboBox();
            this.baudRateLabel = new System.Windows.Forms.Label();
            this.baudRateTextBox = new System.Windows.Forms.TextBox();
            this.serialPanel = new System.Windows.Forms.Panel();
            this.udpPanel = new System.Windows.Forms.Panel();
            this.tcpPanel = new System.Windows.Forms.Panel();
            this.udpPortLabel = new System.Windows.Forms.Label();
            this.udpPortTextBox = new System.Windows.Forms.TextBox();
            this.tcpIpAddressLabel = new System.Windows.Forms.Label();
            this.tcpIpAddressTextBox = new System.Windows.Forms.TextBox();
            this.tcpPortLabel = new System.Windows.Forms.Label();
            this.tcpPortTextBox = new System.Windows.Forms.TextBox();
            this.attitudeBox.SuspendLayout();
            this.connectionBox.SuspendLayout();
            this.serialPanel.SuspendLayout();
            this.udpPanel.SuspendLayout();
            this.tcpPanel.SuspendLayout();
            this.gpsBox.SuspendLayout();
            this.posBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // attitudeBox
            // 
            this.attitudeBox.Controls.Add(this.rollLabel);
            this.attitudeBox.Controls.Add(this.pitchLabel);
            this.attitudeBox.Controls.Add(this.yawLabel);
            this.attitudeBox.Location = new System.Drawing.Point(10, 10);
            this.attitudeBox.Name = "attitudeBox";
            this.attitudeBox.Size = new System.Drawing.Size(250, 120);
            this.attitudeBox.TabIndex = 0;
            this.attitudeBox.TabStop = false;
            this.attitudeBox.Text = "Attitude";
            // 
            // rollLabel
            // 
            this.rollLabel.Location = new System.Drawing.Point(10, 20);
            this.rollLabel.Name = "rollLabel";
            this.rollLabel.Size = new System.Drawing.Size(230, 23);
            this.rollLabel.TabIndex = 0;
            // 
            // pitchLabel
            // 
            this.pitchLabel.Location = new System.Drawing.Point(10, 50);
            this.pitchLabel.Name = "pitchLabel";
            this.pitchLabel.Size = new System.Drawing.Size(230, 23);
            this.pitchLabel.TabIndex = 1;
            // 
            // yawLabel
            // 
            this.yawLabel.Location = new System.Drawing.Point(10, 80);
            this.yawLabel.Name = "yawLabel";
            this.yawLabel.Size = new System.Drawing.Size(230, 23);
            this.yawLabel.TabIndex = 2;
            // 
            // gpsBox
            // 
            this.gpsBox.Controls.Add(this.fixLabel);
            this.gpsBox.Controls.Add(this.latLabel);
            this.gpsBox.Controls.Add(this.lonLabel);
            this.gpsBox.Controls.Add(this.altLabel);
            this.gpsBox.Location = new System.Drawing.Point(10, 140);
            this.gpsBox.Name = "gpsBox";
            this.gpsBox.Size = new System.Drawing.Size(250, 180);
            this.gpsBox.TabIndex = 1;
            this.gpsBox.TabStop = false;
            this.gpsBox.Text = "GPS Raw";
            // 
            // fixLabel
            // 
            this.fixLabel.Location = new System.Drawing.Point(10, 20);
            this.fixLabel.Name = "fixLabel";
            this.fixLabel.Size = new System.Drawing.Size(230, 23);
            this.fixLabel.TabIndex = 0;
            // 
            // latLabel
            // 
            this.latLabel.Location = new System.Drawing.Point(10, 50);
            this.latLabel.Name = "latLabel";
            this.latLabel.Size = new System.Drawing.Size(230, 23);
            this.latLabel.TabIndex = 1;
            // 
            // lonLabel
            // 
            this.lonLabel.Location = new System.Drawing.Point(10, 80);
            this.lonLabel.Name = "lonLabel";
            this.lonLabel.Size = new System.Drawing.Size(230, 23);
            this.lonLabel.TabIndex = 2;
            // 
            // altLabel
            // 
            this.altLabel.Location = new System.Drawing.Point(10, 110);
            this.altLabel.Name = "altLabel";
            this.altLabel.Size = new System.Drawing.Size(230, 23);
            this.altLabel.TabIndex = 3;
            // 
            // posBox
            // 
            this.posBox.Controls.Add(this.relAltLabel);
            this.posBox.Controls.Add(this.headingLabel);
            this.posBox.Location = new System.Drawing.Point(270, 10);
            this.posBox.Name = "posBox";
            this.posBox.Size = new System.Drawing.Size(300, 120);
            this.posBox.TabIndex = 2;
            this.posBox.TabStop = false;
            this.posBox.Text = "Global Position";
            // 
            // relAltLabel
            // 
            this.relAltLabel.Location = new System.Drawing.Point(10, 20);
            this.relAltLabel.Name = "relAltLabel";
            this.relAltLabel.Size = new System.Drawing.Size(280, 23);
            this.relAltLabel.TabIndex = 0;
            // 
            // headingLabel
            // 
            this.headingLabel.Location = new System.Drawing.Point(10, 50);
            this.headingLabel.Name = "headingLabel";
            this.headingLabel.Size = new System.Drawing.Size(280, 23);
            this.headingLabel.TabIndex = 1;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 338);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(584, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // connectionBox
            // 
            this.connectionBox.Controls.Add(this.connectButton);
            this.connectionBox.Controls.Add(this.disconnectButton);
            this.connectionBox.Controls.Add(this.connectionTypeComboBox);
            this.connectionBox.Controls.Add(this.serialPanel);
            this.connectionBox.Controls.Add(this.udpPanel);
            this.connectionBox.Controls.Add(this.tcpPanel);
            this.connectionBox.Location = new System.Drawing.Point(270, 140);
            this.connectionBox.Name = "connectionBox";
            this.connectionBox.Size = new System.Drawing.Size(300, 180);
            this.connectionBox.TabIndex = 4;
            this.connectionBox.TabStop = false;
            this.connectionBox.Text = "Connection Settings";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(10, 140);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 7;
            this.connectButton.Text = "Connect";
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(215, 140);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(75, 23);
            this.disconnectButton.TabIndex = 6;
            this.disconnectButton.Text = "Disconnect";
            // 
            // connectionTypeComboBox
            // 
            this.connectionTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.connectionTypeComboBox.FormattingEnabled = true;
            this.connectionTypeComboBox.Location = new System.Drawing.Point(10, 20);
            this.connectionTypeComboBox.Name = "connectionTypeComboBox";
            this.connectionTypeComboBox.Size = new System.Drawing.Size(280, 23);
            this.connectionTypeComboBox.TabIndex = 5;
            // 
            // serialPanel
            // 
            this.serialPanel.Controls.Add(this.comPortLabel);
            this.serialPanel.Controls.Add(this.comPortComboBox);
            this.serialPanel.Controls.Add(this.baudRateLabel);
            this.serialPanel.Controls.Add(this.baudRateTextBox);
            this.serialPanel.Location = new System.Drawing.Point(5, 50);
            this.serialPanel.Name = "serialPanel";
            this.serialPanel.Size = new System.Drawing.Size(290, 85);
            this.serialPanel.TabIndex = 8;
            // 
            // comPortLabel
            // 
            this.comPortLabel.AutoSize = true;
            this.comPortLabel.Location = new System.Drawing.Point(5, 10);
            this.comPortLabel.Name = "comPortLabel";
            this.comPortLabel.Size = new System.Drawing.Size(53, 15);
            this.comPortLabel.TabIndex = 4;
            this.comPortLabel.Text = "COM Port";
            // 
            // comPortComboBox
            // 
            this.comPortComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comPortComboBox.FormattingEnabled = true;
            this.comPortComboBox.Location = new System.Drawing.Point(5, 30);
            this.comPortComboBox.Name = "comPortComboBox";
            this.comPortComboBox.Size = new System.Drawing.Size(280, 23);
            this.comPortComboBox.TabIndex = 3;
            // 
            // baudRateLabel
            // 
            this.baudRateLabel.AutoSize = true;
            this.baudRateLabel.Location = new System.Drawing.Point(5, 60);
            this.baudRateLabel.Name = "baudRateLabel";
            this.baudRateLabel.Size = new System.Drawing.Size(64, 15);
            this.baudRateLabel.TabIndex = 2;
            this.baudRateLabel.Text = "Baud Rate";
            // 
            // baudRateTextBox
            // 
            this.baudRateTextBox.Location = new System.Drawing.Point(5, 80);
            this.baudRateTextBox.Name = "baudRateTextBox";
            this.baudRateTextBox.Size = new System.Drawing.Size(280, 23);
            this.baudRateTextBox.TabIndex = 1;
            // 
            // udpPanel
            // 
            this.udpPanel.Controls.Add(this.udpPortLabel);
            this.udpPanel.Controls.Add(this.udpPortTextBox);
            this.udpPanel.Location = new System.Drawing.Point(5, 50);
            this.udpPanel.Name = "udpPanel";
            this.udpPanel.Size = new System.Drawing.Size(290, 85);
            this.udpPanel.TabIndex = 9;
            this.udpPanel.Visible = false;
            // 
            // udpPortLabel
            // 
            this.udpPortLabel.AutoSize = true;
            this.udpPortLabel.Location = new System.Drawing.Point(5, 10);
            this.udpPortLabel.Name = "udpPortLabel";
            this.udpPortLabel.Size = new System.Drawing.Size(68, 15);
            this.udpPortLabel.TabIndex = 4;
            this.udpPortLabel.Text = "Listen Port:";
            // 
            // udpPortTextBox
            // 
            this.udpPortTextBox.Location = new System.Drawing.Point(100, 7);
            this.udpPortTextBox.Name = "udpPortTextBox";
            this.udpPortTextBox.Size = new System.Drawing.Size(180, 23);
            this.udpPortTextBox.TabIndex = 3;
            this.udpPortTextBox.Text = "14550";
            // 
            // tcpPanel
            // 
            this.tcpPanel.Controls.Add(this.tcpIpAddressLabel);
            this.tcpPanel.Controls.Add(this.tcpIpAddressTextBox);
            this.tcpPanel.Controls.Add(this.tcpPortLabel);
            this.tcpPanel.Controls.Add(this.tcpPortTextBox);
            this.tcpPanel.Location = new System.Drawing.Point(5, 50);
            this.tcpPanel.Name = "tcpPanel";
            this.tcpPanel.Size = new System.Drawing.Size(290, 85);
            this.tcpPanel.TabIndex = 10;
            this.tcpPanel.Visible = false;
            // 
            // tcpIpAddressLabel
            // 
            this.tcpIpAddressLabel.AutoSize = true;
            this.tcpIpAddressLabel.Location = new System.Drawing.Point(5, 10);
            this.tcpIpAddressLabel.Name = "tcpIpAddressLabel";
            this.tcpIpAddressLabel.Size = new System.Drawing.Size(65, 15);
            this.tcpIpAddressLabel.TabIndex = 4;
            this.tcpIpAddressLabel.Text = "IP Address:";
            // 
            // tcpIpAddressTextBox
            // 
            this.tcpIpAddressTextBox.Location = new System.Drawing.Point(100, 7);
            this.tcpIpAddressTextBox.Name = "tcpIpAddressTextBox";
            this.tcpIpAddressTextBox.Size = new System.Drawing.Size(180, 23);
            this.tcpIpAddressTextBox.TabIndex = 3;
            this.tcpIpAddressTextBox.Text = "127.0.0.1";
            // 
            // tcpPortLabel
            // 
            this.tcpPortLabel.AutoSize = true;
            this.tcpPortLabel.Location = new System.Drawing.Point(5, 40);
            this.tcpPortLabel.Name = "tcpPortLabel";
            this.tcpPortLabel.Size = new System.Drawing.Size(32, 15);
            this.tcpPortLabel.TabIndex = 2;
            this.tcpPortLabel.Text = "Port:";
            // 
            // tcpPortTextBox
            // 
            this.tcpPortTextBox.Location = new System.Drawing.Point(100, 37);
            this.tcpPortTextBox.Name = "tcpPortTextBox";
            this.tcpPortTextBox.Size = new System.Drawing.Size(180, 23);
            this.tcpPortTextBox.TabIndex = 1;
            this.tcpPortTextBox.Text = "5760";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.posBox);
            this.Controls.Add(this.gpsBox);
            this.Controls.Add(this.connectionBox);
            this.Controls.Add(this.attitudeBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "DronePulse Telemetry";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);

            this.attitudeBox.ResumeLayout(false);
            this.gpsBox.ResumeLayout(false);
            this.posBox.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.connectionBox.ResumeLayout(false);
            this.serialPanel.ResumeLayout(false);
            this.serialPanel.PerformLayout();
            this.udpPanel.ResumeLayout(false);
            this.udpPanel.PerformLayout();
            this.tcpPanel.ResumeLayout(false);
            this.tcpPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox attitudeBox;
        private Label rollLabel;
        private Label pitchLabel;
        private Label yawLabel;
        private GroupBox gpsBox;
        private Label fixLabel;
        private Label latLabel;
        private Label lonLabel;
        private Label altLabel;
        private GroupBox posBox;
        private Label relAltLabel;
        private Label headingLabel;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private GroupBox connectionBox;
        private Button connectButton;
        private Button disconnectButton;
        private ComboBox connectionTypeComboBox;
        private Panel serialPanel;
        private Label comPortLabel;
        private ComboBox comPortComboBox;
        private Label baudRateLabel;
        private TextBox baudRateTextBox;
        private Panel udpPanel;
        private Label udpPortLabel;
        private TextBox udpPortTextBox;
        private Panel tcpPanel;
        private Label tcpIpAddressLabel;
        private TextBox tcpIpAddressTextBox;
        private Label tcpPortLabel;
        private TextBox tcpPortTextBox;
    }
}
