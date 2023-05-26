namespace MissionPlanner
{
    partial class AFTGround
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AFTGround));
            this.groundToggleButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // groundToggleButton
            // 
            this.groundToggleButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groundToggleButton.BackColor = System.Drawing.SystemColors.Control;
            this.groundToggleButton.FlatAppearance.BorderSize = 0;
            this.groundToggleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groundToggleButton.Image = global::MissionPlanner.Properties.Resources.tog_img_for_light_mode;
            this.groundToggleButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.groundToggleButton.Location = new System.Drawing.Point(1284, 63);
            this.groundToggleButton.Margin = new System.Windows.Forms.Padding(2);
            this.groundToggleButton.Name = "groundToggleButton";
            this.groundToggleButton.Size = new System.Drawing.Size(50, 50);
            this.groundToggleButton.TabIndex = 15;
            this.groundToggleButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.groundToggleButton.UseVisualStyleBackColor = false;
            this.groundToggleButton.Click += new System.EventHandler(this.groundToggleButton_Click);
            // 
            // AFTGround
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1424, 985);
            this.Controls.Add(this.groundToggleButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AFTGround";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ground";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.groundForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button groundToggleButton;
    }
}