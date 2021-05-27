namespace AgileHDWPF.AgileSng
{
    partial class dg2
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
            this.label1 = new System.Windows.Forms.Label();
            this.nudD = new System.Windows.Forms.NumericUpDown();
            this.nudH = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnS = new System.Windows.Forms.Button();
            this.btnR = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudH)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Duration";
            // 
            // nudD
            // 
            this.nudD.DecimalPlaces = 1;
            this.nudD.Location = new System.Drawing.Point(18, 36);
            this.nudD.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nudD.Name = "nudD";
            this.nudD.Size = new System.Drawing.Size(60, 23);
            this.nudD.TabIndex = 1;
            // 
            // nudH
            // 
            this.nudH.DecimalPlaces = 1;
            this.nudH.Location = new System.Drawing.Point(101, 36);
            this.nudH.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nudH.Name = "nudH";
            this.nudH.Size = new System.Drawing.Size(60, 23);
            this.nudH.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Hold";
            // 
            // btnS
            // 
            this.btnS.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnS.Location = new System.Drawing.Point(172, 30);
            this.btnS.Name = "btnS";
            this.btnS.Size = new System.Drawing.Size(75, 30);
            this.btnS.TabIndex = 4;
            this.btnS.Text = "Set";
            this.btnS.UseVisualStyleBackColor = true;
            // 
            // btnR
            // 
            this.btnR.Location = new System.Drawing.Point(18, 69);
            this.btnR.Name = "btnR";
            this.btnR.Size = new System.Drawing.Size(143, 30);
            this.btnR.TabIndex = 5;
            this.btnR.Text = "Remove from list";
            this.btnR.UseVisualStyleBackColor = true;
            // 
            // dg2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(259, 111);
            this.Controls.Add(this.btnR);
            this.Controls.Add(this.btnS);
            this.Controls.Add(this.nudH);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudD);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "dg2";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detail";
            ((System.ComponentModel.ISupportInitialize)(this.nudD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudH)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown nudD;
        public System.Windows.Forms.NumericUpDown nudH;
        public System.Windows.Forms.Button btnS;
        public System.Windows.Forms.Button btnR;


    }
}