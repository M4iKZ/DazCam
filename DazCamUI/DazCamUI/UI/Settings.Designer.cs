namespace DazCamUI.UI
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkIgnoreLimits = new System.Windows.Forms.CheckBox();
            this.btnExecFooterFile = new System.Windows.Forms.Button();
            this.btnExecHeaderFile = new System.Windows.Forms.Button();
            this.txtExecFooterFile = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtExecHeaderFile = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbOriginLR = new System.Windows.Forms.RadioButton();
            this.rbOriginUR = new System.Windows.Forms.RadioButton();
            this.rbOriginLL = new System.Windows.Forms.RadioButton();
            this.rbOriginUL = new System.Windows.Forms.RadioButton();
            this.txtExecSearchPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtArcLineSegmentLength = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDefaultCuttingFeedrate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMaximumFeedRate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabAxisSettings = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkInvertZ = new System.Windows.Forms.CheckBox();
            this.txtTurnsPerInchZ = new System.Windows.Forms.TextBox();
            this.txtStepsPerTurnZ = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkInvertY = new System.Windows.Forms.CheckBox();
            this.txtTurnsPerInchY = new System.Windows.Forms.TextBox();
            this.txtStepsPerTurnY = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkInvertX = new System.Windows.Forms.CheckBox();
            this.txtTurnsPerInchX = new System.Windows.Forms.TextBox();
            this.txtStepsPerTurnX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnScan = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabAxisSettings.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabAxisSettings);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(729, 409);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnScan);
            this.tabPage1.Controls.Add(this.chkIgnoreLimits);
            this.tabPage1.Controls.Add(this.btnExecFooterFile);
            this.tabPage1.Controls.Add(this.btnExecHeaderFile);
            this.tabPage1.Controls.Add(this.txtExecFooterFile);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.txtExecHeaderFile);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.txtExecSearchPath);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.txtArcLineSegmentLength);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.txtDefaultCuttingFeedrate);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.txtMaximumFeedRate);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtIPAddress);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(721, 383);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreLimits
            // 
            this.chkIgnoreLimits.AutoSize = true;
            this.chkIgnoreLimits.Location = new System.Drawing.Point(199, 187);
            this.chkIgnoreLimits.Name = "chkIgnoreLimits";
            this.chkIgnoreLimits.Size = new System.Drawing.Size(251, 17);
            this.chkIgnoreLimits.TabIndex = 23;
            this.chkIgnoreLimits.Text = "Ignore Limit Switches while G-Code is executing";
            this.chkIgnoreLimits.UseVisualStyleBackColor = true;
            // 
            // btnExecFooterFile
            // 
            this.btnExecFooterFile.Location = new System.Drawing.Point(576, 159);
            this.btnExecFooterFile.Name = "btnExecFooterFile";
            this.btnExecFooterFile.Size = new System.Drawing.Size(27, 23);
            this.btnExecFooterFile.TabIndex = 22;
            this.btnExecFooterFile.Text = "...";
            this.btnExecFooterFile.UseVisualStyleBackColor = true;
            this.btnExecFooterFile.Click += new System.EventHandler(this.btnExecFooterFile_Click);
            // 
            // btnExecHeaderFile
            // 
            this.btnExecHeaderFile.Location = new System.Drawing.Point(576, 133);
            this.btnExecHeaderFile.Name = "btnExecHeaderFile";
            this.btnExecHeaderFile.Size = new System.Drawing.Size(27, 23);
            this.btnExecHeaderFile.TabIndex = 21;
            this.btnExecHeaderFile.Text = "...";
            this.btnExecHeaderFile.UseVisualStyleBackColor = true;
            this.btnExecHeaderFile.Click += new System.EventHandler(this.btnExecHeaderFile_Click);
            // 
            // txtExecFooterFile
            // 
            this.txtExecFooterFile.Location = new System.Drawing.Point(199, 161);
            this.txtExecFooterFile.Name = "txtExecFooterFile";
            this.txtExecFooterFile.Size = new System.Drawing.Size(371, 20);
            this.txtExecFooterFile.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(71, 164);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(122, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Exec Footer GCode File:";
            // 
            // txtExecHeaderFile
            // 
            this.txtExecHeaderFile.Location = new System.Drawing.Point(199, 135);
            this.txtExecHeaderFile.Name = "txtExecHeaderFile";
            this.txtExecHeaderFile.Size = new System.Drawing.Size(371, 20);
            this.txtExecHeaderFile.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(66, 138);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Exec Header GCode File:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbOriginLR);
            this.groupBox4.Controls.Add(this.rbOriginUR);
            this.groupBox4.Controls.Add(this.rbOriginLL);
            this.groupBox4.Controls.Add(this.rbOriginUL);
            this.groupBox4.Location = new System.Drawing.Point(387, 20);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(106, 78);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Bed Origin";
            // 
            // rbOriginLR
            // 
            this.rbOriginLR.AutoSize = true;
            this.rbOriginLR.Location = new System.Drawing.Point(65, 50);
            this.rbOriginLR.Name = "rbOriginLR";
            this.rbOriginLR.Size = new System.Drawing.Size(14, 13);
            this.rbOriginLR.TabIndex = 3;
            this.rbOriginLR.TabStop = true;
            this.rbOriginLR.UseVisualStyleBackColor = true;
            // 
            // rbOriginUR
            // 
            this.rbOriginUR.AutoSize = true;
            this.rbOriginUR.Location = new System.Drawing.Point(65, 19);
            this.rbOriginUR.Name = "rbOriginUR";
            this.rbOriginUR.Size = new System.Drawing.Size(14, 13);
            this.rbOriginUR.TabIndex = 2;
            this.rbOriginUR.TabStop = true;
            this.rbOriginUR.UseVisualStyleBackColor = true;
            // 
            // rbOriginLL
            // 
            this.rbOriginLL.AutoSize = true;
            this.rbOriginLL.Location = new System.Drawing.Point(25, 50);
            this.rbOriginLL.Name = "rbOriginLL";
            this.rbOriginLL.Size = new System.Drawing.Size(14, 13);
            this.rbOriginLL.TabIndex = 1;
            this.rbOriginLL.TabStop = true;
            this.rbOriginLL.UseVisualStyleBackColor = true;
            // 
            // rbOriginUL
            // 
            this.rbOriginUL.AutoSize = true;
            this.rbOriginUL.Location = new System.Drawing.Point(25, 19);
            this.rbOriginUL.Name = "rbOriginUL";
            this.rbOriginUL.Size = new System.Drawing.Size(14, 13);
            this.rbOriginUL.TabIndex = 0;
            this.rbOriginUL.TabStop = true;
            this.rbOriginUL.UseVisualStyleBackColor = true;
            // 
            // txtExecSearchPath
            // 
            this.txtExecSearchPath.Location = new System.Drawing.Point(26, 236);
            this.txtExecSearchPath.Multiline = true;
            this.txtExecSearchPath.Name = "txtExecSearchPath";
            this.txtExecSearchPath.Size = new System.Drawing.Size(407, 124);
            this.txtExecSearchPath.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 220);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(223, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "!EXEC Search Path - Separate multiples with ;";
            // 
            // txtArcLineSegmentLength
            // 
            this.txtArcLineSegmentLength.Location = new System.Drawing.Point(199, 97);
            this.txtArcLineSegmentLength.Name = "txtArcLineSegmentLength";
            this.txtArcLineSegmentLength.Size = new System.Drawing.Size(50, 20);
            this.txtArcLineSegmentLength.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Arc Line Segment Length (inches):";
            // 
            // txtDefaultCuttingFeedrate
            // 
            this.txtDefaultCuttingFeedrate.Location = new System.Drawing.Point(199, 71);
            this.txtDefaultCuttingFeedrate.Name = "txtDefaultCuttingFeedrate";
            this.txtDefaultCuttingFeedrate.Size = new System.Drawing.Size(50, 20);
            this.txtDefaultCuttingFeedrate.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(60, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Default Cutting Feed Rate:";
            // 
            // txtMaximumFeedRate
            // 
            this.txtMaximumFeedRate.Location = new System.Drawing.Point(199, 46);
            this.txtMaximumFeedRate.Name = "txtMaximumFeedRate";
            this.txtMaximumFeedRate.Size = new System.Drawing.Size(50, 20);
            this.txtMaximumFeedRate.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Maximum Feed Rate (IPM):";
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(199, 20);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(119, 20);
            this.txtIPAddress.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(88, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Machine IP Address:";
            // 
            // tabAxisSettings
            // 
            this.tabAxisSettings.Controls.Add(this.groupBox3);
            this.tabAxisSettings.Controls.Add(this.groupBox2);
            this.tabAxisSettings.Controls.Add(this.groupBox1);
            this.tabAxisSettings.Controls.Add(this.label2);
            this.tabAxisSettings.Controls.Add(this.label1);
            this.tabAxisSettings.Location = new System.Drawing.Point(4, 22);
            this.tabAxisSettings.Name = "tabAxisSettings";
            this.tabAxisSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabAxisSettings.Size = new System.Drawing.Size(721, 383);
            this.tabAxisSettings.TabIndex = 1;
            this.tabAxisSettings.Text = "Axis Settings";
            this.tabAxisSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkInvertZ);
            this.groupBox3.Controls.Add(this.txtTurnsPerInchZ);
            this.groupBox3.Controls.Add(this.txtStepsPerTurnZ);
            this.groupBox3.Location = new System.Drawing.Point(316, 23);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(105, 109);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Z Axis";
            // 
            // chkInvertZ
            // 
            this.chkInvertZ.AutoSize = true;
            this.chkInvertZ.Location = new System.Drawing.Point(6, 80);
            this.chkInvertZ.Name = "chkInvertZ";
            this.chkInvertZ.Size = new System.Drawing.Size(98, 17);
            this.chkInvertZ.TabIndex = 7;
            this.chkInvertZ.Text = "Invert Direction";
            this.chkInvertZ.UseVisualStyleBackColor = true;
            // 
            // txtTurnsPerInchZ
            // 
            this.txtTurnsPerInchZ.Location = new System.Drawing.Point(6, 53);
            this.txtTurnsPerInchZ.Name = "txtTurnsPerInchZ";
            this.txtTurnsPerInchZ.Size = new System.Drawing.Size(93, 20);
            this.txtTurnsPerInchZ.TabIndex = 6;
            // 
            // txtStepsPerTurnZ
            // 
            this.txtStepsPerTurnZ.Location = new System.Drawing.Point(6, 27);
            this.txtStepsPerTurnZ.Name = "txtStepsPerTurnZ";
            this.txtStepsPerTurnZ.Size = new System.Drawing.Size(93, 20);
            this.txtStepsPerTurnZ.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkInvertY);
            this.groupBox2.Controls.Add(this.txtTurnsPerInchY);
            this.groupBox2.Controls.Add(this.txtStepsPerTurnY);
            this.groupBox2.Location = new System.Drawing.Point(205, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(105, 109);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Y Axis";
            // 
            // chkInvertY
            // 
            this.chkInvertY.AutoSize = true;
            this.chkInvertY.Location = new System.Drawing.Point(6, 80);
            this.chkInvertY.Name = "chkInvertY";
            this.chkInvertY.Size = new System.Drawing.Size(98, 17);
            this.chkInvertY.TabIndex = 7;
            this.chkInvertY.Text = "Invert Direction";
            this.chkInvertY.UseVisualStyleBackColor = true;
            // 
            // txtTurnsPerInchY
            // 
            this.txtTurnsPerInchY.Location = new System.Drawing.Point(6, 53);
            this.txtTurnsPerInchY.Name = "txtTurnsPerInchY";
            this.txtTurnsPerInchY.Size = new System.Drawing.Size(93, 20);
            this.txtTurnsPerInchY.TabIndex = 6;
            // 
            // txtStepsPerTurnY
            // 
            this.txtStepsPerTurnY.Location = new System.Drawing.Point(6, 27);
            this.txtStepsPerTurnY.Name = "txtStepsPerTurnY";
            this.txtStepsPerTurnY.Size = new System.Drawing.Size(93, 20);
            this.txtStepsPerTurnY.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkInvertX);
            this.groupBox1.Controls.Add(this.txtTurnsPerInchX);
            this.groupBox1.Controls.Add(this.txtStepsPerTurnX);
            this.groupBox1.Location = new System.Drawing.Point(94, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(105, 109);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "X Axis";
            // 
            // chkInvertX
            // 
            this.chkInvertX.AutoSize = true;
            this.chkInvertX.Location = new System.Drawing.Point(6, 80);
            this.chkInvertX.Name = "chkInvertX";
            this.chkInvertX.Size = new System.Drawing.Size(98, 17);
            this.chkInvertX.TabIndex = 7;
            this.chkInvertX.Text = "Invert Direction";
            this.chkInvertX.UseVisualStyleBackColor = true;
            // 
            // txtTurnsPerInchX
            // 
            this.txtTurnsPerInchX.Location = new System.Drawing.Point(6, 53);
            this.txtTurnsPerInchX.Name = "txtTurnsPerInchX";
            this.txtTurnsPerInchX.Size = new System.Drawing.Size(93, 20);
            this.txtTurnsPerInchX.TabIndex = 6;
            // 
            // txtStepsPerTurnX
            // 
            this.txtStepsPerTurnX.Location = new System.Drawing.Point(6, 27);
            this.txtStepsPerTurnX.Name = "txtStepsPerTurnX";
            this.txtStepsPerTurnX.Size = new System.Drawing.Size(93, 20);
            this.txtStepsPerTurnX.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Turns per Inch:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Steps per turn:";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(585, 427);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(666, 427);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.nc";
            this.openFileDialog.Filter = "NC Files|*.nc|All Files|*.*";
            this.openFileDialog.SupportMultiDottedExtensions = true;
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(324, 18);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(56, 23);
            this.btnScan.TabIndex = 24;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 462);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabAxisSettings.ResumeLayout(false);
            this.tabAxisSettings.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabPage tabAxisSettings;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkInvertZ;
        private System.Windows.Forms.TextBox txtTurnsPerInchZ;
        private System.Windows.Forms.TextBox txtStepsPerTurnZ;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkInvertY;
        private System.Windows.Forms.TextBox txtTurnsPerInchY;
        private System.Windows.Forms.TextBox txtStepsPerTurnY;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkInvertX;
        private System.Windows.Forms.TextBox txtTurnsPerInchX;
        private System.Windows.Forms.TextBox txtStepsPerTurnX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMaximumFeedRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDefaultCuttingFeedrate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtExecSearchPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtArcLineSegmentLength;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbOriginLR;
        private System.Windows.Forms.RadioButton rbOriginUR;
        private System.Windows.Forms.RadioButton rbOriginLL;
        private System.Windows.Forms.RadioButton rbOriginUL;
        private System.Windows.Forms.TextBox txtExecFooterFile;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtExecHeaderFile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnExecFooterFile;
        private System.Windows.Forms.Button btnExecHeaderFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.CheckBox chkIgnoreLimits;
        private System.Windows.Forms.Button btnScan;
    }
}