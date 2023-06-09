namespace MissionPlanner
{
    partial class AFTNewMission
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
            this.btnNewMission = new System.Windows.Forms.Button();
            this.btnLoadMission = new System.Windows.Forms.Button();
            this.vertLine = new System.Windows.Forms.PictureBox();
            this.btnContinue = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.vertLine)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNewMission
            // 
            this.btnNewMission.FlatAppearance.BorderSize = 0;
            this.btnNewMission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewMission.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnNewMission.Image = global::MissionPlanner.Properties.Resources.circle_hollow;
            this.btnNewMission.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewMission.Location = new System.Drawing.Point(241, 392);
            this.btnNewMission.Name = "btnNewMission";
            this.btnNewMission.Size = new System.Drawing.Size(190, 34);
            this.btnNewMission.TabIndex = 0;
            this.btnNewMission.Text = "NEW MISSION";
            this.btnNewMission.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNewMission.UseVisualStyleBackColor = true;
            this.btnNewMission.Click += new System.EventHandler(this.btnNewMission_Click);
            // 
            // btnLoadMission
            // 
            this.btnLoadMission.FlatAppearance.BorderSize = 0;
            this.btnLoadMission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadMission.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnLoadMission.Image = global::MissionPlanner.Properties.Resources.circle_hollow;
            this.btnLoadMission.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadMission.Location = new System.Drawing.Point(636, 392);
            this.btnLoadMission.Name = "btnLoadMission";
            this.btnLoadMission.Size = new System.Drawing.Size(305, 34);
            this.btnLoadMission.TabIndex = 1;
            this.btnLoadMission.Text = "LOAD MISSION FROM FILE";
            this.btnLoadMission.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLoadMission.UseVisualStyleBackColor = true;
            this.btnLoadMission.Click += new System.EventHandler(this.btnLoadMission_Click);
            // 
            // vertLine
            // 
            this.vertLine.Image = global::MissionPlanner.Properties.Resources.vert_line_long;
            this.vertLine.Location = new System.Drawing.Point(544, 172);
            this.vertLine.Name = "vertLine";
            this.vertLine.Size = new System.Drawing.Size(1, 462);
            this.vertLine.TabIndex = 2;
            this.vertLine.TabStop = false;
            // 
            // btnContinue
            // 
            this.btnContinue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(199)))), ((int)(((byte)(231)))));
            this.btnContinue.FlatAppearance.BorderSize = 0;
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnContinue.Location = new System.Drawing.Point(915, 717);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(116, 20);
            this.btnContinue.TabIndex = 3;
            this.btnContinue.Text = "CONTINUE";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // AFTNewMission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 772);
            this.ControlBox = false;
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.vertLine);
            this.Controls.Add(this.btnLoadMission);
            this.Controls.Add(this.btnNewMission);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AFTNewMission";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AFTNewMission";
            ((System.ComponentModel.ISupportInitialize)(this.vertLine)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNewMission;
        private System.Windows.Forms.Button btnLoadMission;
        private System.Windows.Forms.PictureBox vertLine;
        private System.Windows.Forms.Button btnContinue;
    }
}