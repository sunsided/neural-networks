namespace Widemeadows.MachineLearning.Neural.Demonstration.Digit
{
    partial class MainForm
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
            this.pictureBoxDigit = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigit)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxDigit
            // 
            this.pictureBoxDigit.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxDigit.Name = "pictureBoxDigit";
            this.pictureBoxDigit.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxDigit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxDigit.TabIndex = 0;
            this.pictureBoxDigit.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 240);
            this.Controls.Add(this.pictureBoxDigit);
            this.Name = "MainForm";
            this.Text = "Handwritten Digital Classification";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxDigit;
    }
}

