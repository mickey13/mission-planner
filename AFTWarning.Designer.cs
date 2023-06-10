namespace MissionPlanner
{
    partial class AFTWarning
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
            this.cautionSign = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.confirmationButton = new ePOSOne.btnProduct.Button_WOC();
            ((System.ComponentModel.ISupportInitialize)(this.cautionSign)).BeginInit();
            this.SuspendLayout();
            // 
            // cautionSign
            // 
            this.cautionSign.Image = global::MissionPlanner.Properties.Resources.caution_sign_large;
            this.cautionSign.Location = new System.Drawing.Point(348, 24);
            this.cautionSign.Name = "cautionSign";
            this.cautionSign.Size = new System.Drawing.Size(81, 70);
            this.cautionSign.TabIndex = 0;
            this.cautionSign.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(294, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "WARNING";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(138, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(501, 50);
            this.label2.TabIndex = 2;
            this.label2.Text = "CUSTOMER ASSUMES ALL LIABILITY WHEN USING\r\nFULL PARAMETER AND SETTINGS MODE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // confirmationButton
            // 
            this.confirmationButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.confirmationButton.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.confirmationButton.FlatAppearance.BorderSize = 0;
            this.confirmationButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.confirmationButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.confirmationButton.Location = new System.Drawing.Point(252, 270);
            this.confirmationButton.Name = "confirmationButton";
            this.confirmationButton.OnHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.confirmationButton.OnHoverButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.confirmationButton.OnHoverTextColor = System.Drawing.SystemColors.ControlText;
            this.confirmationButton.Size = new System.Drawing.Size(274, 44);
            this.confirmationButton.TabIndex = 6;
            this.confirmationButton.Text = "Click to Confirm";
            this.confirmationButton.TextColor = System.Drawing.SystemColors.ControlText;
            this.confirmationButton.UseVisualStyleBackColor = true;
            this.confirmationButton.Click += new System.EventHandler(this.confirmationButton_Click);
            // 
            // AFTWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(777, 350);
            this.Controls.Add(this.confirmationButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cautionSign);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AFTWarning";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.cautionSign)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.PictureBox cautionSign;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label label2;
        internal ePOSOne.btnProduct.Button_WOC confirmationButton;
    }
}