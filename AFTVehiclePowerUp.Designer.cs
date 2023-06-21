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
            this.label1 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.gifPower = new System.Windows.Forms.PictureBox();
            this.gifDrone = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gifPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gifDrone)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(270, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Power up vehicle";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnConnect
            // 
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnConnect.Image = global::MissionPlanner.Properties.Resources.circle_hollow_small;
            this.btnConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConnect.Location = new System.Drawing.Point(340, 222);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(97, 29);
            this.btnConnect.TabIndex = 67;
            this.btnConnect.Text = "connect";
            this.btnConnect.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // gifPower
            // 
            this.gifPower.Image = global::MissionPlanner.Properties.Resources.power_btn_88;
            this.gifPower.Location = new System.Drawing.Point(280, 109);
            this.gifPower.Name = "gifPower";
            this.gifPower.Size = new System.Drawing.Size(88, 88);
            this.gifPower.TabIndex = 68;
            this.gifPower.TabStop = false;
            // 
            // gifDrone
            // 
            this.gifDrone.BackColor = System.Drawing.Color.Transparent;
            this.gifDrone.Image = global::MissionPlanner.Properties.Resources.drone_loading_200;
            this.gifDrone.Location = new System.Drawing.Point(330, 84);
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
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.gifPower);
            this.Controls.Add(this.gifDrone);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AFTVehiclePowerUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AFTVehiclePowerUp";
            ((System.ComponentModel.ISupportInitialize)(this.gifPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gifDrone)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.PictureBox gifPower;
        private System.Windows.Forms.PictureBox gifDrone;
    }
}