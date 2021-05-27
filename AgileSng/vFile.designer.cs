namespace AgileHDWPF.AgileSng
{
    partial class vFile
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
            this.txtVF = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.chkVL = new System.Windows.Forms.CheckBox();
            this.chkVM = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnVb = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtVF
            // 
            this.txtVF.Location = new System.Drawing.Point(20, 40);
            this.txtVF.Name = "txtVF";
            this.txtVF.ReadOnly = true;
            this.txtVF.Size = new System.Drawing.Size(200, 23);
            this.txtVF.TabIndex = 49;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.ForeColor = System.Drawing.Color.Silver;
            this.lblTitle.Location = new System.Drawing.Point(17, 17);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(61, 15);
            this.lblTitle.TabIndex = 50;
            this.lblTitle.Text = "Video File";
            // 
            // chkVL
            // 
            this.chkVL.AutoSize = true;
            this.chkVL.ForeColor = System.Drawing.Color.Silver;
            this.chkVL.Location = new System.Drawing.Point(20, 69);
            this.chkVL.Name = "chkVL";
            this.chkVL.Size = new System.Drawing.Size(52, 19);
            this.chkVL.TabIndex = 47;
            this.chkVL.Text = "Loop";
            this.chkVL.UseVisualStyleBackColor = true;
            // 
            // chkVM
            // 
            this.chkVM.AutoSize = true;
            this.chkVM.ForeColor = System.Drawing.Color.Silver;
            this.chkVM.Location = new System.Drawing.Point(166, 69);
            this.chkVM.Name = "chkVM";
            this.chkVM.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkVM.Size = new System.Drawing.Size(54, 19);
            this.chkVM.TabIndex = 48;
            this.chkVM.Text = "Mute";
            this.chkVM.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.DimGray;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.LightGray;
            this.btnOk.Location = new System.Drawing.Point(202, 104);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 25);
            this.btnOk.TabIndex = 51;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // btnVb
            // 
           // this.btnVb.BackgroundImage = global::AgileHDWPF.Properties.Resources.floder_grey;
            this.btnVb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnVb.FlatAppearance.BorderSize = 0;
            this.btnVb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVb.Location = new System.Drawing.Point(226, 35);
            this.btnVb.Name = "btnVb";
            this.btnVb.Size = new System.Drawing.Size(37, 32);
            this.btnVb.TabIndex = 46;
            this.btnVb.UseVisualStyleBackColor = true;
            // 
            // vFile
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 141);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtVF);
            this.Controls.Add(this.btnVb);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.chkVL);
            this.Controls.Add(this.chkVM);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "vFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "vFile";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Button btnVb;
        public System.Windows.Forms.TextBox txtVF;
        public System.Windows.Forms.CheckBox chkVL;
        public System.Windows.Forms.CheckBox chkVM;
        public System.Windows.Forms.Button btnOk;
    }
}