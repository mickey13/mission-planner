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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.line2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.line1)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // airButton
            // 
            resources.ApplyResources(this.airButton, "airButton");
            this.airButton.BackColor = System.Drawing.SystemColors.Control;
            this.airButton.FlatAppearance.BorderSize = 0;
            this.airButton.Image = global::MissionPlanner.Properties.Resources.Quad_lt_blue__2_;
            this.airButton.Name = "airButton";
            this.airButton.UseVisualStyleBackColor = false;
            this.airButton.Click += new System.EventHandler(this.airButton_Click);
            // 
            // customButton
            // 
            resources.ApplyResources(this.customButton, "customButton");
            this.customButton.BackColor = System.Drawing.SystemColors.Control;
            this.customButton.FlatAppearance.BorderSize = 0;
            this.customButton.Image = global::MissionPlanner.Properties.Resources.caution_sign;
            this.customButton.Name = "customButton";
            this.customButton.UseVisualStyleBackColor = false;
            this.customButton.Click += new System.EventHandler(this.customButton_Click);
            // 
            // groundButton
            // 
            resources.ApplyResources(this.groundButton, "groundButton");
            this.groundButton.BackColor = System.Drawing.SystemColors.Control;
            this.groundButton.FlatAppearance.BorderSize = 0;
            this.groundButton.Image = global::MissionPlanner.Properties.Resources.moon_rover1;
            this.groundButton.Name = "groundButton";
            this.groundButton.UseVisualStyleBackColor = false;
            this.groundButton.Click += new System.EventHandler(this.groundButton_Click);
            // 
            // line2
            // 
            resources.ApplyResources(this.line2, "line2");
            this.line2.Image = global::MissionPlanner.Properties.Resources.line_black;
            this.line2.Name = "line2";
            this.line2.TabStop = false;
            // 
            // line1
            // 
            resources.ApplyResources(this.line1, "line1");
            this.line1.Image = global::MissionPlanner.Properties.Resources.line_black;
            this.line1.Name = "line1";
            this.line1.TabStop = false;
            // 
            // toggleButton
            // 
            resources.ApplyResources(this.toggleButton, "toggleButton");
            this.toggleButton.BackColor = System.Drawing.SystemColors.Control;
            this.toggleButton.FlatAppearance.BorderSize = 0;
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
            // panel1
            // 
            this.panel1.Controls.Add(this.groundButton);
            this.panel1.Controls.Add(this.groundLabel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel3, 7, 8);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 4, 8);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel6, 9, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 2, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.customLabel);
            this.panel3.Controls.Add(this.customButton);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            this.tableLayoutPanel1.SetRowSpan(this.panel3, 2);
            // 
            // panel2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel2, 2);
            this.panel2.Controls.Add(this.airButton);
            this.panel2.Controls.Add(this.airLabel);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            this.tableLayoutPanel1.SetRowSpan(this.panel2, 2);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 6);
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.pictureBox1.Image = global::MissionPlanner.Properties.Resources.AFT_logo_black;
            this.pictureBox1.Name = "pictureBox1";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBox1, 3);
            this.pictureBox1.TabStop = false;
            // 
            // panel5
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel5, 8);
            this.panel5.Controls.Add(this.label1);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            this.tableLayoutPanel1.SetRowSpan(this.panel5, 2);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.toggleButton);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            this.tableLayoutPanel1.SetRowSpan(this.panel6, 2);
            // 
            // panel7
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel7, 6);
            this.panel7.Controls.Add(this.line2);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Controls.Add(this.line1);
            resources.ApplyResources(this.panel7, "panel7");
            this.panel7.Name = "panel7";
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // MainAFT
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "MainAFT";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.line2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.line1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
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
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        internal Label label1;
        private Panel panel6;
        internal PictureBox pictureBox1;
        private Panel panel7;
    }
}
