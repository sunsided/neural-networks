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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelClassLabel = new System.Windows.Forms.Label();
            this.labelClass = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigit)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxDigit
            // 
            this.pictureBoxDigit.Location = new System.Drawing.Point(6, 19);
            this.pictureBoxDigit.Name = "pictureBoxDigit";
            this.pictureBoxDigit.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxDigit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxDigit.TabIndex = 0;
            this.pictureBoxDigit.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelClass);
            this.groupBox1.Controls.Add(this.pictureBoxDigit);
            this.groupBox1.Controls.Add(this.labelClassLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(155, 94);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input";
            // 
            // labelClassLabel
            // 
            this.labelClassLabel.AutoSize = true;
            this.labelClassLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClassLabel.Location = new System.Drawing.Point(76, 19);
            this.labelClassLabel.Name = "labelClassLabel";
            this.labelClassLabel.Size = new System.Drawing.Size(41, 13);
            this.labelClassLabel.TabIndex = 2;
            this.labelClassLabel.Text = "Class:";
            // 
            // labelClass
            // 
            this.labelClass.AutoSize = true;
            this.labelClass.Location = new System.Drawing.Point(123, 19);
            this.labelClass.Name = "labelClass";
            this.labelClass.Size = new System.Drawing.Size(10, 13);
            this.labelClass.TabIndex = 3;
            this.labelClass.Text = "-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "add training / cross validation error graphs per epoch";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 240);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Handwritten Digit Classification";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigit)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxDigit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelClass;
        private System.Windows.Forms.Label labelClassLabel;
        private System.Windows.Forms.Label label1;
    }
}

