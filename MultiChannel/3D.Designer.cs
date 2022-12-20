namespace MultiChannel
{
    partial class Form3D
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
            this.FT_rb = new System.Windows.Forms.RadioButton();
            this.AM_rb = new System.Windows.Forms.RadioButton();
            this.PM2_rb = new System.Windows.Forms.RadioButton();
            this.HorizontalTR_action = new System.Windows.Forms.TrackBar();
            this.VerticalTR_action = new System.Windows.Forms.TrackBar();
            this.MainPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalTR_action)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VerticalTR_action)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // FT_rb
            // 
            this.FT_rb.AutoSize = true;
            this.FT_rb.Location = new System.Drawing.Point(271, 596);
            this.FT_rb.Name = "FT_rb";
            this.FT_rb.Size = new System.Drawing.Size(38, 17);
            this.FT_rb.TabIndex = 12;
            this.FT_rb.TabStop = true;
            this.FT_rb.Text = "FT";
            this.FT_rb.UseVisualStyleBackColor = true;
            this.FT_rb.CheckedChanged += new System.EventHandler(this.RB_changed);
            // 
            // AM_rb
            // 
            this.AM_rb.AutoSize = true;
            this.AM_rb.Location = new System.Drawing.Point(315, 596);
            this.AM_rb.Name = "AM_rb";
            this.AM_rb.Size = new System.Drawing.Size(41, 17);
            this.AM_rb.TabIndex = 11;
            this.AM_rb.TabStop = true;
            this.AM_rb.Text = "AM";
            this.AM_rb.UseVisualStyleBackColor = true;
            this.AM_rb.CheckedChanged += new System.EventHandler(this.RB_changed);
            // 
            // PM2_rb
            // 
            this.PM2_rb.AutoSize = true;
            this.PM2_rb.Location = new System.Drawing.Point(215, 596);
            this.PM2_rb.Name = "PM2_rb";
            this.PM2_rb.Size = new System.Drawing.Size(50, 17);
            this.PM2_rb.TabIndex = 10;
            this.PM2_rb.TabStop = true;
            this.PM2_rb.Text = "PM-2";
            this.PM2_rb.UseVisualStyleBackColor = true;
            this.PM2_rb.CheckedChanged += new System.EventHandler(this.RB_changed);
            // 
            // HorizontalTR_action
            // 
            this.HorizontalTR_action.Location = new System.Drawing.Point(12, 558);
            this.HorizontalTR_action.Maximum = 180;
            this.HorizontalTR_action.Minimum = -180;
            this.HorizontalTR_action.Name = "HorizontalTR_action";
            this.HorizontalTR_action.Size = new System.Drawing.Size(540, 45);
            this.HorizontalTR_action.SmallChange = 5;
            this.HorizontalTR_action.TabIndex = 9;
            this.HorizontalTR_action.Scroll += new System.EventHandler(this.TrackChange);
            // 
            // VerticalTR_action
            // 
            this.VerticalTR_action.Location = new System.Drawing.Point(571, 12);
            this.VerticalTR_action.Maximum = 90;
            this.VerticalTR_action.Minimum = -90;
            this.VerticalTR_action.Name = "VerticalTR_action";
            this.VerticalTR_action.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.VerticalTR_action.Size = new System.Drawing.Size(45, 540);
            this.VerticalTR_action.TabIndex = 8;
            this.VerticalTR_action.Scroll += new System.EventHandler(this.TrackChange);
            // 
            // MainPicture
            // 
            this.MainPicture.Location = new System.Drawing.Point(12, 12);
            this.MainPicture.Name = "MainPicture";
            this.MainPicture.Size = new System.Drawing.Size(540, 540);
            this.MainPicture.TabIndex = 7;
            this.MainPicture.TabStop = false;
            // 
            // _3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 620);
            this.Controls.Add(this.FT_rb);
            this.Controls.Add(this.AM_rb);
            this.Controls.Add(this.PM2_rb);
            this.Controls.Add(this.HorizontalTR_action);
            this.Controls.Add(this.VerticalTR_action);
            this.Controls.Add(this.MainPicture);
            this.Name = "_3D";
            this.Text = "_3D";
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalTR_action)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VerticalTR_action)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton FT_rb;
        private System.Windows.Forms.RadioButton AM_rb;
        private System.Windows.Forms.RadioButton PM2_rb;
        private System.Windows.Forms.TrackBar HorizontalTR_action;
        private System.Windows.Forms.TrackBar VerticalTR_action;
        private System.Windows.Forms.PictureBox MainPicture;
    }
}