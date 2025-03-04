using System;

namespace ADSB_Server
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.lblBasestationStatus = new System.Windows.Forms.Label();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.txtBasestationIP = new System.Windows.Forms.TextBox();
            this.txtBasestationPort = new System.Windows.Forms.TextBox();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnViewJson = new System.Windows.Forms.Button();
            this.btnClearConsole = new System.Windows.Forms.Button();
            this.btnStats = new System.Windows.Forms.Button();
            this.lblBasestationIP = new System.Windows.Forms.Label();
            this.lblBasestationPort = new System.Windows.Forms.Label();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnExampleJson = new System.Windows.Forms.Button();
            this.btnCopyConsole = new System.Windows.Forms.Button();
            this.btnToggleAutoScroll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtConsole
            // 
            this.txtConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConsole.ForeColor = System.Drawing.Color.Lime;
            this.txtConsole.Location = new System.Drawing.Point(12, 12);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConsole.Size = new System.Drawing.Size(867, 397);
            this.txtConsole.TabIndex = 0;
            // 
            // lblBasestationStatus
            // 
            this.lblBasestationStatus.AutoSize = true;
            this.lblBasestationStatus.BackColor = System.Drawing.Color.Red;
            this.lblBasestationStatus.Location = new System.Drawing.Point(12, 435);
            this.lblBasestationStatus.Name = "lblBasestationStatus";
            this.lblBasestationStatus.Padding = new System.Windows.Forms.Padding(5);
            this.lblBasestationStatus.Size = new System.Drawing.Size(23, 23);
            this.lblBasestationStatus.TabIndex = 1;
            this.lblBasestationStatus.Text = "  ";
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.BackColor = System.Drawing.Color.Red;
            this.lblServerStatus.Location = new System.Drawing.Point(12, 460);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Padding = new System.Windows.Forms.Padding(5);
            this.lblServerStatus.Size = new System.Drawing.Size(23, 23);
            this.lblServerStatus.TabIndex = 2;
            this.lblServerStatus.Text = "  ";
            // 
            // txtBasestationIP
            // 
            this.txtBasestationIP.Location = new System.Drawing.Point(121, 438);
            this.txtBasestationIP.Name = "txtBasestationIP";
            this.txtBasestationIP.Size = new System.Drawing.Size(80, 20);
            this.txtBasestationIP.TabIndex = 3;
            this.txtBasestationIP.Text = "127.0.0.1";
            // 
            // txtBasestationPort
            // 
            this.txtBasestationPort.Location = new System.Drawing.Point(251, 438);
            this.txtBasestationPort.Name = "txtBasestationPort";
            this.txtBasestationPort.Size = new System.Drawing.Size(50, 20);
            this.txtBasestationPort.TabIndex = 4;
            this.txtBasestationPort.Text = "30003";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(121, 463);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(80, 20);
            this.txtServerIP.TabIndex = 5;
            this.txtServerIP.Text = "192.168.2.16";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(251, 463);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(50, 20);
            this.txtServerPort.TabIndex = 6;
            this.txtServerPort.Text = "30154";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(321, 436);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(84, 23);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "▶ Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(321, 461);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(84, 23);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "■ Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnViewJson
            // 
            this.btnViewJson.Location = new System.Drawing.Point(411, 436);
            this.btnViewJson.Name = "btnViewJson";
            this.btnViewJson.Size = new System.Drawing.Size(91, 23);
            this.btnViewJson.TabIndex = 9;
            this.btnViewJson.Text = "{ } View JSON";
            this.btnViewJson.UseVisualStyleBackColor = true;
            this.btnViewJson.Click += new System.EventHandler(this.btnViewJson_Click);
            // 
            // btnClearConsole
            // 
            this.btnClearConsole.Location = new System.Drawing.Point(648, 461);
            this.btnClearConsole.Name = "btnClearConsole";
            this.btnClearConsole.Size = new System.Drawing.Size(75, 22);
            this.btnClearConsole.TabIndex = 10;
            this.btnClearConsole.Text = "× Clear";
            this.btnClearConsole.UseVisualStyleBackColor = true;
            this.btnClearConsole.Click += new System.EventHandler(this.btnClearConsole_Click);
            // 
            // btnStats
            // 
            this.btnStats.Location = new System.Drawing.Point(411, 461);
            this.btnStats.Name = "btnStats";
            this.btnStats.Size = new System.Drawing.Size(91, 23);
            this.btnStats.TabIndex = 11;
            this.btnStats.Text = "≡ Statistics";
            this.btnStats.UseVisualStyleBackColor = true;
            this.btnStats.Click += new System.EventHandler(this.btnStats_Click);
            // 
            // lblBasestationIP
            // 
            this.lblBasestationIP.AutoSize = true;
            this.lblBasestationIP.Location = new System.Drawing.Point(41, 441);
            this.lblBasestationIP.Name = "lblBasestationIP";
            this.lblBasestationIP.Size = new System.Drawing.Size(78, 13);
            this.lblBasestationIP.TabIndex = 3;
            this.lblBasestationIP.Text = "Basestation IP:";
            // 
            // lblBasestationPort
            // 
            this.lblBasestationPort.AutoSize = true;
            this.lblBasestationPort.Location = new System.Drawing.Point(211, 441);
            this.lblBasestationPort.Name = "lblBasestationPort";
            this.lblBasestationPort.Size = new System.Drawing.Size(29, 13);
            this.lblBasestationPort.TabIndex = 4;
            this.lblBasestationPort.Text = "Port:";
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Location = new System.Drawing.Point(41, 466);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(54, 13);
            this.lblServerIP.TabIndex = 5;
            this.lblServerIP.Text = "Server IP:";
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(211, 466);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(29, 13);
            this.lblServerPort.TabIndex = 6;
            this.lblServerPort.Text = "Port:";
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Location = new System.Drawing.Point(12, 415);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(134, 13);
            this.lblCurrentTime.TabIndex = 0;
            this.lblCurrentTime.Text = "UTC: 2025-03-03 02:39:26";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnExampleJson
            // 
            this.btnExampleJson.Location = new System.Drawing.Point(508, 436);
            this.btnExampleJson.Name = "btnExampleJson";
            this.btnExampleJson.Size = new System.Drawing.Size(134, 22);
            this.btnExampleJson.TabIndex = 12;
            this.btnExampleJson.Text = "Send Test JSON";
            this.btnExampleJson.UseVisualStyleBackColor = true;
            this.btnExampleJson.Click += new System.EventHandler(this.btnExampleJson_Click);
            // 
            // btnCopyConsole
            // 
            this.btnCopyConsole.Location = new System.Drawing.Point(648, 436);
            this.btnCopyConsole.Name = "btnCopyConsole";
            this.btnCopyConsole.Size = new System.Drawing.Size(75, 23);
            this.btnCopyConsole.TabIndex = 13;
            this.btnCopyConsole.Text = "⎘ Copy";
            this.btnCopyConsole.UseVisualStyleBackColor = true;
            this.btnCopyConsole.Click += new System.EventHandler(this.btnCopyConsole_Click);
            // 
            // btnToggleAutoScroll
            // 
            this.btnToggleAutoScroll.Location = new System.Drawing.Point(508, 461);
            this.btnToggleAutoScroll.Name = "btnToggleAutoScroll";
            this.btnToggleAutoScroll.Size = new System.Drawing.Size(134, 22);
            this.btnToggleAutoScroll.TabIndex = 14;
            this.btnToggleAutoScroll.Text = "▼ Auto-scroll is ON";
            this.btnToggleAutoScroll.UseVisualStyleBackColor = true;
            this.btnToggleAutoScroll.Click += new System.EventHandler(this.btnToggleAutoScroll_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 490);
            this.Controls.Add(this.btnToggleAutoScroll);
            this.Controls.Add(this.btnCopyConsole);
            this.Controls.Add(this.btnExampleJson);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.lblCurrentTime);
            this.Controls.Add(this.lblBasestationStatus);
            this.Controls.Add(this.lblServerStatus);
            this.Controls.Add(this.lblBasestationIP);
            this.Controls.Add(this.lblBasestationPort);
            this.Controls.Add(this.lblServerIP);
            this.Controls.Add(this.lblServerPort);
            this.Controls.Add(this.txtBasestationIP);
            this.Controls.Add(this.txtBasestationPort);
            this.Controls.Add(this.txtServerIP);
            this.Controls.Add(this.txtServerPort);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnViewJson);
            this.Controls.Add(this.btnClearConsole);
            this.Controls.Add(this.btnStats);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(816, 529);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ADSB Bridge";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.Label lblBasestationStatus;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.TextBox txtBasestationIP;
        private System.Windows.Forms.TextBox txtBasestationPort;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnViewJson;
        private System.Windows.Forms.Button btnClearConsole;
        private System.Windows.Forms.Button btnStats;
        private System.Windows.Forms.Label lblBasestationIP;
        private System.Windows.Forms.Label lblBasestationPort;
        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnExampleJson;
        private System.Windows.Forms.Button btnCopyConsole;
        private System.Windows.Forms.Button btnToggleAutoScroll;
    }
}