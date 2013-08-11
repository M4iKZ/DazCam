namespace DazCamUI.UI
{
    partial class Jog
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
            this.btnExit = new System.Windows.Forms.Button();
            this.btnXY = new System.Windows.Forms.Button();
            this.btnXZ = new System.Windows.Forms.Button();
            this.btnYZ = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(25, 176);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(199, 59);
            this.btnExit.TabIndex = 1;
            this.btnExit.TabStop = false;
            this.btnExit.Text = "Exit &Jog Mode";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnXY
            // 
            this.btnXY.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnXY.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXY.Location = new System.Drawing.Point(25, 17);
            this.btnXY.Name = "btnXY";
            this.btnXY.Size = new System.Drawing.Size(199, 42);
            this.btnXY.TabIndex = 2;
            this.btnXY.Text = "Jog &X / Y";
            this.btnXY.UseVisualStyleBackColor = false;
            this.btnXY.Click += new System.EventHandler(this.btnXY_Click);
            // 
            // btnXZ
            // 
            this.btnXZ.BackColor = System.Drawing.SystemColors.Control;
            this.btnXZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXZ.Location = new System.Drawing.Point(25, 65);
            this.btnXZ.Name = "btnXZ";
            this.btnXZ.Size = new System.Drawing.Size(199, 42);
            this.btnXZ.TabIndex = 3;
            this.btnXZ.Text = "Jog X / &Z";
            this.btnXZ.UseVisualStyleBackColor = false;
            this.btnXZ.Click += new System.EventHandler(this.btnXZ_Click);
            // 
            // btnYZ
            // 
            this.btnYZ.BackColor = System.Drawing.SystemColors.Control;
            this.btnYZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYZ.Location = new System.Drawing.Point(25, 113);
            this.btnYZ.Name = "btnYZ";
            this.btnYZ.Size = new System.Drawing.Size(199, 42);
            this.btnYZ.TabIndex = 0;
            this.btnYZ.Text = "Jog &Y / Z";
            this.btnYZ.UseVisualStyleBackColor = false;
            this.btnYZ.Click += new System.EventHandler(this.btnYZ_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 255);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 26);
            this.label1.TabIndex = 4;
            this.label1.Text = "[ESC] - Exits Jog Mode\r\n[SPACE BAR] - Toggles X/Y and Y/Z modes";
            // 
            // Jog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 290);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnYZ);
            this.Controls.Add(this.btnXZ);
            this.Controls.Add(this.btnXY);
            this.Controls.Add(this.btnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Jog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Axis Jog Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Jog_FormClosing);
            this.Load += new System.EventHandler(this.Jog_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Jog_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnXY;
        private System.Windows.Forms.Button btnXZ;
        private System.Windows.Forms.Button btnYZ;
        private System.Windows.Forms.Label label1;
    }
}