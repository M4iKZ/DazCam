namespace DazCamUI.UI
{
    partial class Offsets
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Offsets));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbFoundPosition = new System.Windows.Forms.RadioButton();
            this.rbCurrentPosition = new System.Windows.Forms.RadioButton();
            this.btnJog = new System.Windows.Forms.Button();
            this.btnFindAll = new System.Windows.Forms.Button();
            this.btnFindZ = new System.Windows.Forms.Button();
            this.btnFindY = new System.Windows.Forms.Button();
            this.btnFindX = new System.Windows.Forms.Button();
            this.txtFoundX = new System.Windows.Forms.TextBox();
            this.txtFoundY = new System.Windows.Forms.TextBox();
            this.txtFoundZ = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCurrentX = new System.Windows.Forms.TextBox();
            this.txtCurrentY = new System.Windows.Forms.TextBox();
            this.txtCurrentZ = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxGCodeIdentifiers = new System.Windows.Forms.ListBox();
            this.lblOffsetIDUsage = new System.Windows.Forms.Label();
            this.txtOffsetName = new System.Windows.Forms.TextBox();
            this.chkOffsetInvertZ = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkOffsetInvertY = new System.Windows.Forms.CheckBox();
            this.chkOffsetInvertX = new System.Windows.Forms.CheckBox();
            this.txtOffsetZ = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOffsetY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOffsetX = new System.Windows.Forms.TextBox();
            this.btnOffsetDelete = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOffsetAdd = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddToZ = new System.Windows.Forms.Button();
            this.btnAddToY = new System.Windows.Forms.Button();
            this.btnAddToX = new System.Windows.Forms.Button();
            this.btnCopyAll = new System.Windows.Forms.Button();
            this.btnCopyZ = new System.Windows.Forms.Button();
            this.btnCopyY = new System.Windows.Forms.Button();
            this.btnCopyX = new System.Windows.Forms.Button();
            this.btnAddToAll = new System.Windows.Forms.Button();
            this.listBoxOffsets = new System.Windows.Forms.CheckedListBox();
            this.btnSubtractZFrom = new System.Windows.Forms.Button();
            this.btnSubtractYFrom = new System.Windows.Forms.Button();
            this.btnSubtractXFrom = new System.Windows.Forms.Button();
            this.btnSubtractFromAll = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbFoundPosition);
            this.groupBox1.Controls.Add(this.rbCurrentPosition);
            this.groupBox1.Controls.Add(this.btnJog);
            this.groupBox1.Controls.Add(this.btnFindAll);
            this.groupBox1.Controls.Add(this.btnFindZ);
            this.groupBox1.Controls.Add(this.btnFindY);
            this.groupBox1.Controls.Add(this.btnFindX);
            this.groupBox1.Controls.Add(this.txtFoundX);
            this.groupBox1.Controls.Add(this.txtFoundY);
            this.groupBox1.Controls.Add(this.txtFoundZ);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtCurrentX);
            this.groupBox1.Controls.Add(this.txtCurrentY);
            this.groupBox1.Controls.Add(this.txtCurrentZ);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(582, 175);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Coordinates";
            // 
            // rbFoundPosition
            // 
            this.rbFoundPosition.AutoSize = true;
            this.rbFoundPosition.Location = new System.Drawing.Point(41, 90);
            this.rbFoundPosition.Name = "rbFoundPosition";
            this.rbFoundPosition.Size = new System.Drawing.Size(98, 17);
            this.rbFoundPosition.TabIndex = 38;
            this.rbFoundPosition.Text = "Found Position:";
            this.rbFoundPosition.UseVisualStyleBackColor = true;
            // 
            // rbCurrentPosition
            // 
            this.rbCurrentPosition.AutoSize = true;
            this.rbCurrentPosition.Checked = true;
            this.rbCurrentPosition.Location = new System.Drawing.Point(41, 61);
            this.rbCurrentPosition.Name = "rbCurrentPosition";
            this.rbCurrentPosition.Size = new System.Drawing.Size(102, 17);
            this.rbCurrentPosition.TabIndex = 33;
            this.rbCurrentPosition.TabStop = true;
            this.rbCurrentPosition.Text = "&Current Position:";
            this.rbCurrentPosition.UseVisualStyleBackColor = true;
            // 
            // btnJog
            // 
            this.btnJog.Location = new System.Drawing.Point(392, 58);
            this.btnJog.Name = "btnJog";
            this.btnJog.Size = new System.Drawing.Size(75, 23);
            this.btnJog.TabIndex = 37;
            this.btnJog.Text = "&Jog";
            this.btnJog.UseVisualStyleBackColor = true;
            this.btnJog.Click += new System.EventHandler(this.btnJog_Click);
            // 
            // btnFindAll
            // 
            this.btnFindAll.Location = new System.Drawing.Point(149, 147);
            this.btnFindAll.Name = "btnFindAll";
            this.btnFindAll.Size = new System.Drawing.Size(237, 23);
            this.btnFindAll.TabIndex = 45;
            this.btnFindAll.Text = "&Find All";
            this.btnFindAll.UseVisualStyleBackColor = true;
            this.btnFindAll.Click += new System.EventHandler(this.btnFindAll_Click);
            // 
            // btnFindZ
            // 
            this.btnFindZ.Location = new System.Drawing.Point(311, 118);
            this.btnFindZ.Name = "btnFindZ";
            this.btnFindZ.Size = new System.Drawing.Size(75, 23);
            this.btnFindZ.TabIndex = 44;
            this.btnFindZ.Text = "Find";
            this.btnFindZ.UseVisualStyleBackColor = true;
            this.btnFindZ.Click += new System.EventHandler(this.btnFindZ_Click);
            // 
            // btnFindY
            // 
            this.btnFindY.Location = new System.Drawing.Point(230, 118);
            this.btnFindY.Name = "btnFindY";
            this.btnFindY.Size = new System.Drawing.Size(75, 23);
            this.btnFindY.TabIndex = 43;
            this.btnFindY.Text = "Find";
            this.btnFindY.UseVisualStyleBackColor = true;
            this.btnFindY.Click += new System.EventHandler(this.btnFindY_Click);
            // 
            // btnFindX
            // 
            this.btnFindX.Location = new System.Drawing.Point(149, 118);
            this.btnFindX.Name = "btnFindX";
            this.btnFindX.Size = new System.Drawing.Size(75, 23);
            this.btnFindX.TabIndex = 42;
            this.btnFindX.Text = "Find";
            this.btnFindX.UseVisualStyleBackColor = true;
            this.btnFindX.Click += new System.EventHandler(this.btnFindX_Click);
            // 
            // txtFoundX
            // 
            this.txtFoundX.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFoundX.Location = new System.Drawing.Point(149, 86);
            this.txtFoundX.Name = "txtFoundX";
            this.txtFoundX.Size = new System.Drawing.Size(75, 26);
            this.txtFoundX.TabIndex = 39;
            this.txtFoundX.Text = "0";
            // 
            // txtFoundY
            // 
            this.txtFoundY.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFoundY.Location = new System.Drawing.Point(230, 86);
            this.txtFoundY.Name = "txtFoundY";
            this.txtFoundY.Size = new System.Drawing.Size(75, 26);
            this.txtFoundY.TabIndex = 40;
            this.txtFoundY.Text = "0";
            // 
            // txtFoundZ
            // 
            this.txtFoundZ.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFoundZ.Location = new System.Drawing.Point(311, 86);
            this.txtFoundZ.Name = "txtFoundZ";
            this.txtFoundZ.Size = new System.Drawing.Size(75, 26);
            this.txtFoundZ.TabIndex = 41;
            this.txtFoundZ.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(331, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 36);
            this.label8.TabIndex = 32;
            this.label8.Text = "Z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(252, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 36);
            this.label2.TabIndex = 31;
            this.label2.Text = "Y";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(168, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 36);
            this.label9.TabIndex = 30;
            this.label9.Text = "X";
            // 
            // txtCurrentX
            // 
            this.txtCurrentX.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentX.Location = new System.Drawing.Point(149, 55);
            this.txtCurrentX.Name = "txtCurrentX";
            this.txtCurrentX.Size = new System.Drawing.Size(75, 26);
            this.txtCurrentX.TabIndex = 34;
            this.txtCurrentX.Text = "0";
            // 
            // txtCurrentY
            // 
            this.txtCurrentY.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentY.Location = new System.Drawing.Point(230, 55);
            this.txtCurrentY.Name = "txtCurrentY";
            this.txtCurrentY.Size = new System.Drawing.Size(75, 26);
            this.txtCurrentY.TabIndex = 35;
            this.txtCurrentY.Text = "0";
            // 
            // txtCurrentZ
            // 
            this.txtCurrentZ.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentZ.Location = new System.Drawing.Point(311, 55);
            this.txtCurrentZ.Name = "txtCurrentZ";
            this.txtCurrentZ.Size = new System.Drawing.Size(75, 26);
            this.txtCurrentZ.TabIndex = 36;
            this.txtCurrentZ.Text = "0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSubtractFromAll);
            this.groupBox2.Controls.Add(this.btnSubtractZFrom);
            this.groupBox2.Controls.Add(this.btnSubtractYFrom);
            this.groupBox2.Controls.Add(this.btnSubtractXFrom);
            this.groupBox2.Controls.Add(this.listBoxOffsets);
            this.groupBox2.Controls.Add(this.btnAddToAll);
            this.groupBox2.Controls.Add(this.btnAddToZ);
            this.groupBox2.Controls.Add(this.btnAddToY);
            this.groupBox2.Controls.Add(this.btnAddToX);
            this.groupBox2.Controls.Add(this.btnCopyAll);
            this.groupBox2.Controls.Add(this.btnCopyZ);
            this.groupBox2.Controls.Add(this.btnCopyY);
            this.groupBox2.Controls.Add(this.listBoxGCodeIdentifiers);
            this.groupBox2.Controls.Add(this.lblOffsetIDUsage);
            this.groupBox2.Controls.Add(this.txtOffsetName);
            this.groupBox2.Controls.Add(this.chkOffsetInvertZ);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.chkOffsetInvertY);
            this.groupBox2.Controls.Add(this.btnCopyX);
            this.groupBox2.Controls.Add(this.chkOffsetInvertX);
            this.groupBox2.Controls.Add(this.txtOffsetZ);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtOffsetY);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtOffsetX);
            this.groupBox2.Controls.Add(this.btnOffsetDelete);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnOffsetAdd);
            this.groupBox2.Location = new System.Drawing.Point(12, 193);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(582, 216);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Workspace &Offsets";
            // 
            // listBoxGCodeIdentifiers
            // 
            this.listBoxGCodeIdentifiers.FormattingEnabled = true;
            this.listBoxGCodeIdentifiers.Location = new System.Drawing.Point(16, 19);
            this.listBoxGCodeIdentifiers.Name = "listBoxGCodeIdentifiers";
            this.listBoxGCodeIdentifiers.Size = new System.Drawing.Size(142, 160);
            this.listBoxGCodeIdentifiers.TabIndex = 1;
            this.listBoxGCodeIdentifiers.SelectedIndexChanged += new System.EventHandler(this.listBoxGCodeIdentifiers_SelectedIndexChanged);
            // 
            // lblOffsetIDUsage
            // 
            this.lblOffsetIDUsage.AutoSize = true;
            this.lblOffsetIDUsage.ForeColor = System.Drawing.Color.Navy;
            this.lblOffsetIDUsage.Location = new System.Drawing.Point(477, 14);
            this.lblOffsetIDUsage.Name = "lblOffsetIDUsage";
            this.lblOffsetIDUsage.Size = new System.Drawing.Size(0, 13);
            this.lblOffsetIDUsage.TabIndex = 16;
            // 
            // txtOffsetName
            // 
            this.txtOffsetName.Location = new System.Drawing.Point(371, 19);
            this.txtOffsetName.Name = "txtOffsetName";
            this.txtOffsetName.Size = new System.Drawing.Size(109, 20);
            this.txtOffsetName.TabIndex = 6;
            this.txtOffsetName.Leave += new System.EventHandler(this.txtOffsetProperty_Leave);
            // 
            // chkOffsetInvertZ
            // 
            this.chkOffsetInvertZ.AutoSize = true;
            this.chkOffsetInvertZ.Location = new System.Drawing.Point(523, 99);
            this.chkOffsetInvertZ.Name = "chkOffsetInvertZ";
            this.chkOffsetInvertZ.Size = new System.Drawing.Size(53, 17);
            this.chkOffsetInvertZ.TabIndex = 24;
            this.chkOffsetInvertZ.Text = "Invert";
            this.chkOffsetInvertZ.UseVisualStyleBackColor = true;
            this.chkOffsetInvertZ.Click += new System.EventHandler(this.chkOffsetInvert_Clicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(322, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Name:";
            // 
            // chkOffsetInvertY
            // 
            this.chkOffsetInvertY.AutoSize = true;
            this.chkOffsetInvertY.Location = new System.Drawing.Point(523, 73);
            this.chkOffsetInvertY.Name = "chkOffsetInvertY";
            this.chkOffsetInvertY.Size = new System.Drawing.Size(53, 17);
            this.chkOffsetInvertY.TabIndex = 18;
            this.chkOffsetInvertY.Text = "Invert";
            this.chkOffsetInvertY.UseVisualStyleBackColor = true;
            this.chkOffsetInvertY.Click += new System.EventHandler(this.chkOffsetInvert_Clicked);
            // 
            // chkOffsetInvertX
            // 
            this.chkOffsetInvertX.AutoSize = true;
            this.chkOffsetInvertX.Location = new System.Drawing.Point(523, 48);
            this.chkOffsetInvertX.Name = "chkOffsetInvertX";
            this.chkOffsetInvertX.Size = new System.Drawing.Size(53, 17);
            this.chkOffsetInvertX.TabIndex = 12;
            this.chkOffsetInvertX.Text = "Invert";
            this.chkOffsetInvertX.UseVisualStyleBackColor = true;
            this.chkOffsetInvertX.Click += new System.EventHandler(this.chkOffsetInvert_Clicked);
            // 
            // txtOffsetZ
            // 
            this.txtOffsetZ.Location = new System.Drawing.Point(371, 97);
            this.txtOffsetZ.Name = "txtOffsetZ";
            this.txtOffsetZ.Size = new System.Drawing.Size(70, 20);
            this.txtOffsetZ.TabIndex = 20;
            this.txtOffsetZ.Leave += new System.EventHandler(this.txtOffsetProperty_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(339, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "&Z:";
            // 
            // txtOffsetY
            // 
            this.txtOffsetY.Location = new System.Drawing.Point(371, 71);
            this.txtOffsetY.Name = "txtOffsetY";
            this.txtOffsetY.Size = new System.Drawing.Size(70, 20);
            this.txtOffsetY.TabIndex = 14;
            this.txtOffsetY.Leave += new System.EventHandler(this.txtOffsetProperty_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(339, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "&Y:";
            // 
            // txtOffsetX
            // 
            this.txtOffsetX.Location = new System.Drawing.Point(371, 45);
            this.txtOffsetX.Name = "txtOffsetX";
            this.txtOffsetX.Size = new System.Drawing.Size(70, 20);
            this.txtOffsetX.TabIndex = 8;
            this.txtOffsetX.Leave += new System.EventHandler(this.txtOffsetProperty_Leave);
            // 
            // btnOffsetDelete
            // 
            this.btnOffsetDelete.Location = new System.Drawing.Point(250, 186);
            this.btnOffsetDelete.Name = "btnOffsetDelete";
            this.btnOffsetDelete.Size = new System.Drawing.Size(66, 23);
            this.btnOffsetDelete.TabIndex = 4;
            this.btnOffsetDelete.Text = "&Delete";
            this.btnOffsetDelete.UseVisualStyleBackColor = true;
            this.btnOffsetDelete.Click += new System.EventHandler(this.btnOffsetDelete_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(339, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "&X:";
            // 
            // btnOffsetAdd
            // 
            this.btnOffsetAdd.Location = new System.Drawing.Point(174, 186);
            this.btnOffsetAdd.Name = "btnOffsetAdd";
            this.btnOffsetAdd.Size = new System.Drawing.Size(66, 23);
            this.btnOffsetAdd.TabIndex = 3;
            this.btnOffsetAdd.Text = "&Add";
            this.btnOffsetAdd.UseVisualStyleBackColor = true;
            this.btnOffsetAdd.Click += new System.EventHandler(this.btnOffsetAdd_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(519, 415);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddToZ
            // 
            this.btnAddToZ.AutoSize = true;
            this.btnAddToZ.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddToZ.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddToZ.Image = ((System.Drawing.Image)(resources.GetObject("btnAddToZ.Image")));
            this.btnAddToZ.Location = new System.Drawing.Point(470, 95);
            this.btnAddToZ.Name = "btnAddToZ";
            this.btnAddToZ.Size = new System.Drawing.Size(22, 22);
            this.btnAddToZ.TabIndex = 22;
            this.btnAddToZ.UseVisualStyleBackColor = true;
            this.btnAddToZ.Click += new System.EventHandler(this.btnAddToZ_Click);
            // 
            // btnAddToY
            // 
            this.btnAddToY.AutoSize = true;
            this.btnAddToY.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddToY.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddToY.Image = ((System.Drawing.Image)(resources.GetObject("btnAddToY.Image")));
            this.btnAddToY.Location = new System.Drawing.Point(470, 69);
            this.btnAddToY.Name = "btnAddToY";
            this.btnAddToY.Size = new System.Drawing.Size(22, 22);
            this.btnAddToY.TabIndex = 16;
            this.btnAddToY.UseVisualStyleBackColor = true;
            this.btnAddToY.Click += new System.EventHandler(this.btnAddToY_Click);
            // 
            // btnAddToX
            // 
            this.btnAddToX.AutoSize = true;
            this.btnAddToX.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddToX.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddToX.Image = ((System.Drawing.Image)(resources.GetObject("btnAddToX.Image")));
            this.btnAddToX.Location = new System.Drawing.Point(470, 43);
            this.btnAddToX.Name = "btnAddToX";
            this.btnAddToX.Size = new System.Drawing.Size(22, 22);
            this.btnAddToX.TabIndex = 10;
            this.btnAddToX.UseVisualStyleBackColor = true;
            this.btnAddToX.Click += new System.EventHandler(this.btnAddToX_Click);
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCopyAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCopyAll.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyAll.Image")));
            this.btnCopyAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopyAll.Location = new System.Drawing.Point(394, 136);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new System.Drawing.Size(47, 22);
            this.btnCopyAll.TabIndex = 25;
            this.btnCopyAll.Text = "All";
            this.btnCopyAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopyAll.UseVisualStyleBackColor = true;
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // btnCopyZ
            // 
            this.btnCopyZ.AutoSize = true;
            this.btnCopyZ.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCopyZ.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCopyZ.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyZ.Image")));
            this.btnCopyZ.Location = new System.Drawing.Point(447, 95);
            this.btnCopyZ.Name = "btnCopyZ";
            this.btnCopyZ.Size = new System.Drawing.Size(22, 22);
            this.btnCopyZ.TabIndex = 21;
            this.btnCopyZ.UseVisualStyleBackColor = true;
            this.btnCopyZ.Click += new System.EventHandler(this.btnCopyZ_Click);
            // 
            // btnCopyY
            // 
            this.btnCopyY.AutoSize = true;
            this.btnCopyY.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCopyY.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCopyY.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyY.Image")));
            this.btnCopyY.Location = new System.Drawing.Point(447, 69);
            this.btnCopyY.Name = "btnCopyY";
            this.btnCopyY.Size = new System.Drawing.Size(22, 22);
            this.btnCopyY.TabIndex = 15;
            this.btnCopyY.UseVisualStyleBackColor = true;
            this.btnCopyY.Click += new System.EventHandler(this.btnCopyY_Click);
            // 
            // btnCopyX
            // 
            this.btnCopyX.AutoSize = true;
            this.btnCopyX.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCopyX.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCopyX.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyX.Image")));
            this.btnCopyX.Location = new System.Drawing.Point(447, 43);
            this.btnCopyX.Name = "btnCopyX";
            this.btnCopyX.Size = new System.Drawing.Size(22, 22);
            this.btnCopyX.TabIndex = 9;
            this.btnCopyX.UseVisualStyleBackColor = true;
            this.btnCopyX.Click += new System.EventHandler(this.btnCopyX_Click);
            // 
            // btnAddToAll
            // 
            this.btnAddToAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddToAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddToAll.Image = global::DazCamUI.Properties.Resources.plus_icon;
            this.btnAddToAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddToAll.Location = new System.Drawing.Point(443, 136);
            this.btnAddToAll.Name = "btnAddToAll";
            this.btnAddToAll.Size = new System.Drawing.Size(47, 22);
            this.btnAddToAll.TabIndex = 26;
            this.btnAddToAll.Text = "All";
            this.btnAddToAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddToAll.UseVisualStyleBackColor = true;
            this.btnAddToAll.Click += new System.EventHandler(this.btnAddToAll_Click);
            // 
            // listBoxOffsets
            // 
            this.listBoxOffsets.FormattingEnabled = true;
            this.listBoxOffsets.IntegralHeight = false;
            this.listBoxOffsets.Location = new System.Drawing.Point(167, 19);
            this.listBoxOffsets.Name = "listBoxOffsets";
            this.listBoxOffsets.Size = new System.Drawing.Size(149, 161);
            this.listBoxOffsets.TabIndex = 2;
            this.listBoxOffsets.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listBoxOffsets_ItemCheck);
            this.listBoxOffsets.SelectedIndexChanged += new System.EventHandler(this.listBoxOffsets_SelectedIndexChanged);
            // 
            // btnSubtractZFrom
            // 
            this.btnSubtractZFrom.AutoSize = true;
            this.btnSubtractZFrom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSubtractZFrom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubtractZFrom.Image = ((System.Drawing.Image)(resources.GetObject("btnSubtractZFrom.Image")));
            this.btnSubtractZFrom.Location = new System.Drawing.Point(495, 95);
            this.btnSubtractZFrom.Name = "btnSubtractZFrom";
            this.btnSubtractZFrom.Size = new System.Drawing.Size(22, 22);
            this.btnSubtractZFrom.TabIndex = 23;
            this.btnSubtractZFrom.UseVisualStyleBackColor = true;
            this.btnSubtractZFrom.Click += new System.EventHandler(this.btnSubtractZFrom_Click);
            // 
            // btnSubtractYFrom
            // 
            this.btnSubtractYFrom.AutoSize = true;
            this.btnSubtractYFrom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSubtractYFrom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubtractYFrom.Image = ((System.Drawing.Image)(resources.GetObject("btnSubtractYFrom.Image")));
            this.btnSubtractYFrom.Location = new System.Drawing.Point(495, 69);
            this.btnSubtractYFrom.Name = "btnSubtractYFrom";
            this.btnSubtractYFrom.Size = new System.Drawing.Size(22, 22);
            this.btnSubtractYFrom.TabIndex = 17;
            this.btnSubtractYFrom.UseVisualStyleBackColor = true;
            this.btnSubtractYFrom.Click += new System.EventHandler(this.btnSubtractYFrom_Click);
            // 
            // btnSubtractXFrom
            // 
            this.btnSubtractXFrom.AutoSize = true;
            this.btnSubtractXFrom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSubtractXFrom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubtractXFrom.Image = ((System.Drawing.Image)(resources.GetObject("btnSubtractXFrom.Image")));
            this.btnSubtractXFrom.Location = new System.Drawing.Point(495, 43);
            this.btnSubtractXFrom.Name = "btnSubtractXFrom";
            this.btnSubtractXFrom.Size = new System.Drawing.Size(22, 22);
            this.btnSubtractXFrom.TabIndex = 11;
            this.btnSubtractXFrom.UseVisualStyleBackColor = true;
            this.btnSubtractXFrom.Click += new System.EventHandler(this.btnSubtractXFrom_Click);
            // 
            // btnSubtractFromAll
            // 
            this.btnSubtractFromAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSubtractFromAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSubtractFromAll.Image = global::DazCamUI.Properties.Resources.plus_icon;
            this.btnSubtractFromAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSubtractFromAll.Location = new System.Drawing.Point(493, 136);
            this.btnSubtractFromAll.Name = "btnSubtractFromAll";
            this.btnSubtractFromAll.Size = new System.Drawing.Size(47, 22);
            this.btnSubtractFromAll.TabIndex = 27;
            this.btnSubtractFromAll.Text = "All";
            this.btnSubtractFromAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSubtractFromAll.UseVisualStyleBackColor = true;
            this.btnSubtractFromAll.Click += new System.EventHandler(this.btnSubtractFromAll_Click);
            // 
            // Offsets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 447);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Offsets";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Offsets";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Offsets_FormClosed);
            this.Load += new System.EventHandler(this.Offsets_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Offsets_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblOffsetIDUsage;
        private System.Windows.Forms.TextBox txtOffsetName;
        private System.Windows.Forms.CheckBox chkOffsetInvertZ;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkOffsetInvertY;
        private System.Windows.Forms.CheckBox chkOffsetInvertX;
        private System.Windows.Forms.TextBox txtOffsetZ;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOffsetY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOffsetX;
        private System.Windows.Forms.Button btnOffsetDelete;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOffsetAdd;
        private System.Windows.Forms.Button btnJog;
        private System.Windows.Forms.Button btnFindAll;
        private System.Windows.Forms.Button btnFindZ;
        private System.Windows.Forms.Button btnFindY;
        private System.Windows.Forms.Button btnFindX;
        private System.Windows.Forms.TextBox txtFoundX;
        private System.Windows.Forms.TextBox txtFoundY;
        private System.Windows.Forms.TextBox txtFoundZ;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCurrentX;
        private System.Windows.Forms.TextBox txtCurrentY;
        private System.Windows.Forms.TextBox txtCurrentZ;
        private System.Windows.Forms.RadioButton rbFoundPosition;
        private System.Windows.Forms.RadioButton rbCurrentPosition;
        private System.Windows.Forms.ListBox listBoxGCodeIdentifiers;
        private System.Windows.Forms.Button btnCopyX;
        private System.Windows.Forms.Button btnCopyZ;
        private System.Windows.Forms.Button btnCopyY;
        private System.Windows.Forms.Button btnCopyAll;
        private System.Windows.Forms.Button btnAddToZ;
        private System.Windows.Forms.Button btnAddToY;
        private System.Windows.Forms.Button btnAddToX;
        private System.Windows.Forms.Button btnAddToAll;
        private System.Windows.Forms.CheckedListBox listBoxOffsets;
        private System.Windows.Forms.Button btnSubtractFromAll;
        private System.Windows.Forms.Button btnSubtractZFrom;
        private System.Windows.Forms.Button btnSubtractYFrom;
        private System.Windows.Forms.Button btnSubtractXFrom;
    }
}