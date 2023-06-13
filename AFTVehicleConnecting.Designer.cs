namespace MissionPlanner
{
    partial class AFTVehicleConnecting
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
            this.btnSegment = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gifDrone = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gifDrone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSegment
            // 
            this.btnSegment.FlatAppearance.BorderSize = 0;
            this.btnSegment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSegment.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSegment.Image = global::MissionPlanner.Properties.Resources.circle_selected_small;
            this.btnSegment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSegment.Location = new System.Drawing.Point(331, 240);
            this.btnSegment.Name = "btnSegment";
            this.btnSegment.Size = new System.Drawing.Size(97, 29);
            this.btnSegment.TabIndex = 71;
            this.btnSegment.Text = "connect";
            this.btnSegment.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnSegment.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(261, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 46);
            this.label1.TabIndex = 70;
            this.label1.Text = "Power up vehicle";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gifDrone
            // 
            this.gifDrone.Location = new System.Drawing.Point(280, 75);
            this.gifDrone.Name = "gifDrone";
            this.gifDrone.Size = new System.Drawing.Size(200, 154);
            this.gifDrone.TabIndex = 73;
            this.gifDrone.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(230, 137);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 300);
            this.pictureBox1.TabIndex = 74;
            this.pictureBox1.TabStop = false;
            // 
            // AFTVehicleConnecting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 350);
            this.Controls.Add(this.btnSegment);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gifDrone);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AFTVehicleConnecting";
            this.Text = "AFTVehicleConnecting";
            ((System.ComponentModel.ISupportInitialize)(this.gifDrone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSegment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox gifDrone;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}