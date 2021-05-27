namespace AgileHDWPF.AgileSng
{
    partial class GItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn1 = new System.Windows.Forms.Button();
            this.pic1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn1
            // 
            this.btn1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.btn1.FlatAppearance.BorderSize = 0;
            this.btn1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.btn1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.btn1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn1.Location = new System.Drawing.Point(0, 75);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(110, 25);
            this.btn1.TabIndex = 0;
            this.btn1.Text = "TITLE";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.GItem_Click);
            this.btn1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic1_MouseDown);
            // 
            // pic1
            // 
            this.pic1.BackColor = System.Drawing.Color.Black;
            this.pic1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pic1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.pic1.ErrorImage = global::mep_agh.Properties.Resources.logo_y;
            //this.pic1.InitialImage = global::mep_agh.Properties.Resources.logo_y;
            this.pic1.Location = new System.Drawing.Point(0, 0);
            this.pic1.Margin = new System.Windows.Forms.Padding(0);
            this.pic1.Name = "pic1";
            this.pic1.Size = new System.Drawing.Size(110, 75);
            this.pic1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic1.TabIndex = 2;
            this.pic1.TabStop = false;
            this.pic1.Click += new System.EventHandler(this.GItem_Click);
            this.pic1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic1_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 15);
            this.label1.TabIndex = 3;
            this.label1.Tag = "0";
            this.label1.Text = "0";
            this.label1.Visible = false;
            // 
            // GItem
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(52)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.pic1);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.LightGray;
            this.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.Name = "GItem";
            this.Size = new System.Drawing.Size(110, 100);
            this.Click += new System.EventHandler(this.GItem_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btn1;
        public System.Windows.Forms.PictureBox pic1;
        private System.Windows.Forms.Label label1;
    }
}
