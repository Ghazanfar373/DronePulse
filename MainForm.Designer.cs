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
            attitudeBox = new GroupBox();
            rollLabel = new Label();
            pitchLabel = new Label();
            yawLabel = new Label();
            gpsBox = new GroupBox();
            fixLabel = new Label();
            latLabel = new Label();
            lonLabel = new Label();
            altLabel = new Label();
            posBox = new GroupBox();
            relAltLabel = new Label();
            headingLabel = new Label();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            connectionBox = new GroupBox();
            connectButton = new Button();
            disconnectButton = new Button();
            connectionTypeComboBox = new ComboBox();
            serialPanel = new Panel();
            comPortLabel = new Label();
            comPortComboBox = new ComboBox();
            baudRateLabel = new Label();
            baudRateTextBox = new TextBox();
            udpPanel = new Panel();
            udpPortLabel = new Label();
            udpPortTextBox = new TextBox();
            tcpPanel = new Panel();
            tcpIpAddressLabel = new Label();
            tcpIpAddressTextBox = new TextBox();
            tcpPortLabel = new Label();
            tcpPortTextBox = new TextBox();
            attitudeBox.SuspendLayout();
            gpsBox.SuspendLayout();
            posBox.SuspendLayout();
            statusStrip.SuspendLayout();
            connectionBox.SuspendLayout();
            serialPanel.SuspendLayout();
            udpPanel.SuspendLayout();
            tcpPanel.SuspendLayout();
            SuspendLayout();
            // 
            // attitudeBox
            // 
            attitudeBox.Controls.Add(rollLabel);
            attitudeBox.Controls.Add(pitchLabel);
            attitudeBox.Controls.Add(yawLabel);
            attitudeBox.Location = new Point(11, 13);
            attitudeBox.Margin = new Padding(3, 4, 3, 4);
            attitudeBox.Name = "attitudeBox";
            attitudeBox.Padding = new Padding(3, 4, 3, 4);
            attitudeBox.Size = new Size(286, 160);
            attitudeBox.TabIndex = 0;
            attitudeBox.TabStop = false;
            attitudeBox.Text = "Attitude";
            // 
            // rollLabel
            // 
            rollLabel.Location = new Point(11, 27);
            rollLabel.Name = "rollLabel";
            rollLabel.Size = new Size(263, 31);
            rollLabel.TabIndex = 0;
            // 
            // pitchLabel
            // 
            pitchLabel.Location = new Point(11, 67);
            pitchLabel.Name = "pitchLabel";
            pitchLabel.Size = new Size(263, 31);
            pitchLabel.TabIndex = 1;
            // 
            // yawLabel
            // 
            yawLabel.Location = new Point(11, 107);
            yawLabel.Name = "yawLabel";
            yawLabel.Size = new Size(263, 31);
            yawLabel.TabIndex = 2;
            // 
            // gpsBox
            // 
            gpsBox.Controls.Add(fixLabel);
            gpsBox.Controls.Add(latLabel);
            gpsBox.Controls.Add(lonLabel);
            gpsBox.Controls.Add(altLabel);
            gpsBox.Location = new Point(11, 187);
            gpsBox.Margin = new Padding(3, 4, 3, 4);
            gpsBox.Name = "gpsBox";
            gpsBox.Padding = new Padding(3, 4, 3, 4);
            gpsBox.Size = new Size(286, 240);
            gpsBox.TabIndex = 1;
            gpsBox.TabStop = false;
            gpsBox.Text = "GPS Raw";
            // 
            // fixLabel
            // 
            fixLabel.Location = new Point(11, 27);
            fixLabel.Name = "fixLabel";
            fixLabel.Size = new Size(263, 31);
            fixLabel.TabIndex = 0;
            // 
            // latLabel
            // 
            latLabel.Location = new Point(11, 67);
            latLabel.Name = "latLabel";
            latLabel.Size = new Size(263, 31);
            latLabel.TabIndex = 1;
            // 
            // lonLabel
            // 
            lonLabel.Location = new Point(11, 107);
            lonLabel.Name = "lonLabel";
            lonLabel.Size = new Size(263, 31);
            lonLabel.TabIndex = 2;
            // 
            // altLabel
            // 
            altLabel.Location = new Point(11, 147);
            altLabel.Name = "altLabel";
            altLabel.Size = new Size(263, 31);
            altLabel.TabIndex = 3;
            // 
            // posBox
            // 
            posBox.Controls.Add(relAltLabel);
            posBox.Controls.Add(headingLabel);
            posBox.Location = new Point(309, 13);
            posBox.Margin = new Padding(3, 4, 3, 4);
            posBox.Name = "posBox";
            posBox.Padding = new Padding(3, 4, 3, 4);
            posBox.Size = new Size(343, 160);
            posBox.TabIndex = 2;
            posBox.TabStop = false;
            posBox.Text = "Global Position";
            // 
            // relAltLabel
            // 
            relAltLabel.Location = new Point(11, 27);
            relAltLabel.Name = "relAltLabel";
            relAltLabel.Size = new Size(320, 31);
            relAltLabel.TabIndex = 0;
            // 
            // headingLabel
            // 
            headingLabel.Location = new Point(11, 67);
            headingLabel.Name = "headingLabel";
            headingLabel.Size = new Size(320, 31);
            headingLabel.TabIndex = 1;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 505);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(808, 22);
            statusStrip.TabIndex = 3;
            statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(0, 16);
            // 
            // connectionBox
            // 
            connectionBox.Controls.Add(connectButton);
            connectionBox.Controls.Add(disconnectButton);
            connectionBox.Controls.Add(connectionTypeComboBox);
            connectionBox.Controls.Add(serialPanel);
            connectionBox.Controls.Add(udpPanel);
            connectionBox.Controls.Add(tcpPanel);
            connectionBox.Location = new Point(309, 187);
            connectionBox.Margin = new Padding(3, 4, 3, 4);
            connectionBox.Name = "connectionBox";
            connectionBox.Padding = new Padding(3, 4, 3, 4);
            connectionBox.Size = new Size(343, 240);
            connectionBox.TabIndex = 4;
            connectionBox.TabStop = false;
            connectionBox.Text = "Connection Settings";
            // 
            // connectButton
            // 
            connectButton.Location = new Point(11, 204);
            connectButton.Margin = new Padding(3, 4, 3, 4);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(86, 31);
            connectButton.TabIndex = 7;
            connectButton.Text = "Connect";
            // 
            // disconnectButton
            // 
            disconnectButton.Location = new Point(241, 204);
            disconnectButton.Margin = new Padding(3, 4, 3, 4);
            disconnectButton.Name = "disconnectButton";
            disconnectButton.Size = new Size(91, 31);
            disconnectButton.TabIndex = 6;
            disconnectButton.Text = "Disconnect";
            // 
            // connectionTypeComboBox
            // 
            connectionTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            connectionTypeComboBox.FormattingEnabled = true;
            connectionTypeComboBox.Location = new Point(11, 27);
            connectionTypeComboBox.Margin = new Padding(3, 4, 3, 4);
            connectionTypeComboBox.Name = "connectionTypeComboBox";
            connectionTypeComboBox.Size = new Size(319, 28);
            connectionTypeComboBox.TabIndex = 5;
            // 
            // serialPanel
            // 
            serialPanel.Controls.Add(comPortLabel);
            serialPanel.Controls.Add(comPortComboBox);
            serialPanel.Controls.Add(baudRateLabel);
            serialPanel.Controls.Add(baudRateTextBox);
            serialPanel.Location = new Point(6, 62);
            serialPanel.Margin = new Padding(3, 4, 3, 4);
            serialPanel.Name = "serialPanel";
            serialPanel.Size = new Size(331, 138);
            serialPanel.TabIndex = 8;
            // 
            // comPortLabel
            // 
            comPortLabel.AutoSize = true;
            comPortLabel.Location = new Point(6, 13);
            comPortLabel.Name = "comPortLabel";
            comPortLabel.Size = new Size(72, 20);
            comPortLabel.TabIndex = 4;
            comPortLabel.Text = "COM Port";
            // 
            // comPortComboBox
            // 
            comPortComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comPortComboBox.FormattingEnabled = true;
            comPortComboBox.Location = new Point(6, 40);
            comPortComboBox.Margin = new Padding(3, 4, 3, 4);
            comPortComboBox.Name = "comPortComboBox";
            comPortComboBox.Size = new Size(319, 28);
            comPortComboBox.TabIndex = 3;
            // 
            // baudRateLabel
            // 
            baudRateLabel.AutoSize = true;
            baudRateLabel.Location = new Point(6, 80);
            baudRateLabel.Name = "baudRateLabel";
            baudRateLabel.Size = new Size(77, 20);
            baudRateLabel.TabIndex = 2;
            baudRateLabel.Text = "Baud Rate";
            // 
            // baudRateTextBox
            // 
            baudRateTextBox.Location = new Point(6, 107);
            baudRateTextBox.Margin = new Padding(3, 4, 3, 4);
            baudRateTextBox.Name = "baudRateTextBox";
            baudRateTextBox.Size = new Size(319, 27);
            baudRateTextBox.TabIndex = 1;
            // 
            // udpPanel
            // 
            udpPanel.Controls.Add(udpPortLabel);
            udpPanel.Controls.Add(udpPortTextBox);
            udpPanel.Location = new Point(6, 67);
            udpPanel.Margin = new Padding(3, 4, 3, 4);
            udpPanel.Name = "udpPanel";
            udpPanel.Size = new Size(331, 113);
            udpPanel.TabIndex = 9;
            udpPanel.Visible = false;
            // 
            // udpPortLabel
            // 
            udpPortLabel.AutoSize = true;
            udpPortLabel.Location = new Point(6, 13);
            udpPortLabel.Name = "udpPortLabel";
            udpPortLabel.Size = new Size(80, 20);
            udpPortLabel.TabIndex = 4;
            udpPortLabel.Text = "Listen Port:";
            // 
            // udpPortTextBox
            // 
            udpPortTextBox.Location = new Point(114, 9);
            udpPortTextBox.Margin = new Padding(3, 4, 3, 4);
            udpPortTextBox.Name = "udpPortTextBox";
            udpPortTextBox.Size = new Size(205, 27);
            udpPortTextBox.TabIndex = 3;
            udpPortTextBox.Text = "14550";
            // 
            // tcpPanel
            // 
            tcpPanel.Controls.Add(tcpIpAddressLabel);
            tcpPanel.Controls.Add(tcpIpAddressTextBox);
            tcpPanel.Controls.Add(tcpPortLabel);
            tcpPanel.Controls.Add(tcpPortTextBox);
            tcpPanel.Location = new Point(6, 67);
            tcpPanel.Margin = new Padding(3, 4, 3, 4);
            tcpPanel.Name = "tcpPanel";
            tcpPanel.Size = new Size(331, 113);
            tcpPanel.TabIndex = 10;
            tcpPanel.Visible = false;
            // 
            // tcpIpAddressLabel
            // 
            tcpIpAddressLabel.AutoSize = true;
            tcpIpAddressLabel.Location = new Point(6, 13);
            tcpIpAddressLabel.Name = "tcpIpAddressLabel";
            tcpIpAddressLabel.Size = new Size(81, 20);
            tcpIpAddressLabel.TabIndex = 4;
            tcpIpAddressLabel.Text = "IP Address:";
            // 
            // tcpIpAddressTextBox
            // 
            tcpIpAddressTextBox.Location = new Point(114, 9);
            tcpIpAddressTextBox.Margin = new Padding(3, 4, 3, 4);
            tcpIpAddressTextBox.Name = "tcpIpAddressTextBox";
            tcpIpAddressTextBox.Size = new Size(205, 27);
            tcpIpAddressTextBox.TabIndex = 3;
            tcpIpAddressTextBox.Text = "127.0.0.1";
            // 
            // tcpPortLabel
            // 
            tcpPortLabel.AutoSize = true;
            tcpPortLabel.Location = new Point(6, 53);
            tcpPortLabel.Name = "tcpPortLabel";
            tcpPortLabel.Size = new Size(38, 20);
            tcpPortLabel.TabIndex = 2;
            tcpPortLabel.Text = "Port:";
            // 
            // tcpPortTextBox
            // 
            tcpPortTextBox.Location = new Point(114, 49);
            tcpPortTextBox.Margin = new Padding(3, 4, 3, 4);
            tcpPortTextBox.Name = "tcpPortTextBox";
            tcpPortTextBox.Size = new Size(205, 27);
            tcpPortTextBox.TabIndex = 1;
            tcpPortTextBox.Text = "5760";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(808, 527);
            Controls.Add(statusStrip);
            Controls.Add(posBox);
            Controls.Add(gpsBox);
            Controls.Add(connectionBox);
            Controls.Add(attitudeBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "MainForm";
            Text = "DronePulse Telemetry";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            attitudeBox.ResumeLayout(false);
            gpsBox.ResumeLayout(false);
            posBox.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            connectionBox.ResumeLayout(false);
            serialPanel.ResumeLayout(false);
            serialPanel.PerformLayout();
            udpPanel.ResumeLayout(false);
            udpPanel.PerformLayout();
            tcpPanel.ResumeLayout(false);
            tcpPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

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
