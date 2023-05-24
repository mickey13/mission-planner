using System.Windows.Forms;

namespace MissionPlanner
{
    partial class MainAFT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainAFT));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.airButton = new System.Windows.Forms.Button();
            this.customButton = new System.Windows.Forms.Button();
            this.groundButton = new System.Windows.Forms.Button();
            this.line2 = new System.Windows.Forms.PictureBox();
            this.line1 = new System.Windows.Forms.PictureBox();
            this.toggleButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groundLabel = new System.Windows.Forms.Label();
            this.customLabel = new System.Windows.Forms.Label();
            this.airLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.line2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.line1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.pictureBox1.Image = global::MissionPlanner.Properties.Resources.AFT_logo_black;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // airButton
            // 
            this.airButton.BackColor = System.Drawing.SystemColors.Control;
            this.airButton.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.airButton, "airButton");
            this.airButton.Image = global::MissionPlanner.Properties.Resources.Quad_lt_blue__2_;
            this.airButton.Name = "airButton";
            this.airButton.UseVisualStyleBackColor = false;
            this.airButton.Click += new System.EventHandler(this.airButton_Click);
            // 
            // customButton
            // 
            this.customButton.BackColor = System.Drawing.SystemColors.Control;
            this.customButton.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.customButton, "customButton");
            this.customButton.Image = global::MissionPlanner.Properties.Resources.caution_sign;
            this.customButton.Name = "customButton";
            this.customButton.UseVisualStyleBackColor = false;
            this.customButton.Click += new System.EventHandler(this.customButton_Click);
            // 
            // groundButton
            // 
            this.groundButton.BackColor = System.Drawing.SystemColors.Control;
            this.groundButton.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.groundButton, "groundButton");
            this.groundButton.Image = global::MissionPlanner.Properties.Resources.moon_rover1;
            this.groundButton.Name = "groundButton";
            this.groundButton.UseVisualStyleBackColor = false;
            this.groundButton.Click += new System.EventHandler(this.groundButton_Click);
            // 
            // line2
            // 
            this.line2.Image = global::MissionPlanner.Properties.Resources.line_black;
            resources.ApplyResources(this.line2, "line2");
            this.line2.Name = "line2";
            this.line2.TabStop = false;
            // 
            // line1
            // 
            this.line1.Image = global::MissionPlanner.Properties.Resources.line_black;
            resources.ApplyResources(this.line1, "line1");
            this.line1.Name = "line1";
            this.line1.TabStop = false;
            // 
            // toggleButton
            // 
            this.toggleButton.BackColor = System.Drawing.SystemColors.Control;
            this.toggleButton.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.toggleButton, "toggleButton");
            this.toggleButton.Image = global::MissionPlanner.Properties.Resources.tog_img_for_light_mode;
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.UseVisualStyleBackColor = false;
            this.toggleButton.Click += new System.EventHandler(this.toggleButton_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groundLabel
            // 
            resources.ApplyResources(this.groundLabel, "groundLabel");
            this.groundLabel.Name = "groundLabel";
            // 
            // customLabel
            // 
            resources.ApplyResources(this.customLabel, "customLabel");
            this.customLabel.Name = "customLabel";
            // 
            // airLabel
            // 
            resources.ApplyResources(this.airLabel, "airLabel");
            this.airLabel.Name = "airLabel";
            // 
            // MainAFT
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.airLabel);
            this.Controls.Add(this.customLabel);
            this.Controls.Add(this.groundLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.toggleButton);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.line2);
            this.Controls.Add(this.customButton);
            this.Controls.Add(this.airButton);
            this.Controls.Add(this.groundButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Name = "MainAFT";
            this.Load += new System.EventHandler(this.MainAFT_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.line2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.line1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.PictureBox pictureBox1;
        internal System.Windows.Forms.Button airButton;
        internal System.Windows.Forms.Button customButton;
        internal System.Windows.Forms.Button groundButton;
        internal PictureBox line2;
        internal PictureBox line1;
        internal Button toggleButton;
        internal Label label2;
        internal Label groundLabel;
        internal Label customLabel;
        internal Label airLabel;
    }
}
