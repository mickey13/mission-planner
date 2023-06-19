namespace MissionPlanner
{
    partial class AFTVehiclePowerUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AFTVehiclePowerUp));
            this.label1 = new System.Windows.Forms.Label();
            this.btnSegment = new System.Windows.Forms.Button();
            this.gifPower = new System.Windows.Forms.PictureBox();
            this.gifDrone = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gifPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gifDrone)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(270, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Power up vehicle";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSegment
            // 
            this.btnSegment.FlatAppearance.BorderSize = 0;
            this.btnSegment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSegment.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSegment.Image = global::MissionPlanner.Properties.Resources.circle_hollow_small;
            this.btnSegment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSegment.Location = new System.Drawing.Point(340, 222);
            this.btnSegment.Name = "btnSegment";
            this.btnSegment.Size = new System.Drawing.Size(97, 29);
            this.btnSegment.TabIndex = 67;
            this.btnSegment.Text = "connect";
            this.btnSegment.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnSegment.UseVisualStyleBackColor = true;
            this.btnSegment.Click += new System.EventHandler(this.btnSegment_Click);
            // 
            // gifPower
            // 
            this.gifPower.Image = global::MissionPlanner.Properties.Resources.power_btn_88;
            this.gifPower.Location = new System.Drawing.Point(287, 109);
            this.gifPower.Name = "gifPower";
            this.gifPower.Size = new System.Drawing.Size(88, 88);
            this.gifPower.TabIndex = 68;
            this.gifPower.TabStop = false;
            // 
            // gifDrone
            // 
            this.gifDrone.Image = ((System.Drawing.Image)(resources.GetObject("gifDrone.Image")));
            this.gifDrone.Location = new System.Drawing.Point(324, 84);
            this.gifDrone.Name = "gifDrone";
            this.gifDrone.Size = new System.Drawing.Size(200, 154);
            this.gifDrone.TabIndex = 69;
            this.gifDrone.TabStop = false;
            // 
            // AFTVehiclePowerUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 350);
            this.Controls.Add(this.gifPower);
            this.Controls.Add(this.btnSegment);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gifDrone);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AFTVehiclePowerUp";
            this.Text = "AFTVehiclePowerUp";
            ((System.ComponentModel.ISupportInitialize)(this.gifPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gifDrone)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSegment;
        private System.Windows.Forms.PictureBox gifPower;
        private System.Windows.Forms.PictureBox gifDrone;
    }
}