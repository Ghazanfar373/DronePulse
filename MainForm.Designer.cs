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
            this.attitudeBox.SuspendLayout();
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.posBox);
            this.Controls.Add(this.gpsBox);
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
    }
}
