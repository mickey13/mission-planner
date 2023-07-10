namespace MissionPlanner
{
    partial class AFTAir
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AFTAir));
            this.airToggleButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // airToggleButton
            // 
            this.airToggleButton.BackColor = System.Drawing.SystemColors.Control;
            this.airToggleButton.FlatAppearance.BorderSize = 0;
            this.airToggleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.airToggleButton.Image = global::MissionPlanner.Properties.Resources.tog_img_for_light_mode;
            this.airToggleButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.airToggleButton.Location = new System.Drawing.Point(1284, 63);
            this.airToggleButton.Margin = new System.Windows.Forms.Padding(2);
            this.airToggleButton.Name = "airToggleButton";
            this.airToggleButton.Size = new System.Drawing.Size(50, 50);
            this.airToggleButton.TabIndex = 16;
            this.airToggleButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.airToggleButton.UseVisualStyleBackColor = false;
            this.airToggleButton.Click += new System.EventHandler(this.airToggleButton_Click);
            // 
            // AFTAir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1424, 985);
            this.Controls.Add(this.airToggleButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AFTAir";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mission Planner - AFT Air Interface";
            this.Load += new System.EventHandler(this.AFTAir_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button airToggleButton;
    }
}