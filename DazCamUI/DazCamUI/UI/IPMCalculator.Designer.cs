namespace DazCamUI.UI
{
    partial class IPMCalculator
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
            this.txtInchesX = new System.Windows.Forms.TextBox();
            this.txtResolution = new System.Windows.Forms.TextBox();
            this.txtStepDelay = new System.Windows.Forms.TextBox();
            this.txtInchesY = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtInchesZ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtInchesX
            // 
            this.txtInchesX.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInchesX.Location = new System.Drawing.Point(183, 88);
            this.txtInchesX.Name = "txtInchesX";
            this.txtInchesX.Size = new System.Drawing.Size(100, 26);
            this.txtInchesX.TabIndex = 0;
            this.txtInchesX.Enter += new System.EventHandler(this.txtInchesX_Enter);
            // 
            // txtResolution
            // 
            this.txtResolution.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResolution.Location = new System.Drawing.Point(183, 120);
            this.txtResolution.Name = "txtResolution";
            this.txtResolution.Size = new System.Drawing.Size(100, 26);
            this.txtResolution.TabIndex = 3;
            this.txtResolution.Enter += new System.EventHandler(this.txtResolution_Enter);
            // 
            // txtStepDelay
            // 
            this.txtStepDelay.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStepDelay.Location = new System.Drawing.Point(183, 152);
            this.txtStepDelay.Name = "txtStepDelay";
            this.txtStepDelay.Size = new System.Drawing.Size(100, 26);
            this.txtStepDelay.TabIndex = 4;
            this.txtStepDelay.Enter += new System.EventHandler(this.txtStepDelay_Enter);
            // 
            // txtInchesY
            // 
            this.txtInchesY.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInchesY.Location = new System.Drawing.Point(289, 88);
            this.txtInchesY.Name = "txtInchesY";
            this.txtInchesY.Size = new System.Drawing.Size(100, 26);
            this.txtInchesY.TabIndex = 1;
            this.txtInchesY.Enter += new System.EventHandler(this.txtInchesY_Enter);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(183, 184);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 5;
            this.btnGo.Text = "&Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtInchesZ
            // 
            this.txtInchesZ.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInchesZ.Location = new System.Drawing.Point(395, 88);
            this.txtInchesZ.Name = "txtInchesZ";
            this.txtInchesZ.Size = new System.Drawing.Size(100, 26);
            this.txtInchesZ.TabIndex = 2;
            this.txtInchesZ.Enter += new System.EventHandler(this.txtInchesZ_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Move Delta:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Resolution:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(120, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Step Delay:";
            // 
            // IPMCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 364);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInchesZ);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.txtInchesY);
            this.Controls.Add(this.txtStepDelay);
            this.Controls.Add(this.txtResolution);
            this.Controls.Add(this.txtInchesX);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IPMCalculator";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IPM - Calculator";
            this.Load += new System.EventHandler(this.IPMCalculator_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInchesX;
        private System.Windows.Forms.TextBox txtResolution;
        private System.Windows.Forms.TextBox txtStepDelay;
        private System.Windows.Forms.TextBox txtInchesY;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtInchesZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}