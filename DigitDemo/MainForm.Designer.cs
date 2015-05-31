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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pictureBoxDigit = new System.Windows.Forms.PictureBox();
            this.labelClassLabel = new System.Windows.Forms.Label();
            this.labelClass = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripContainerMain = new System.Windows.Forms.ToolStripContainer();
            this.toolStripNetworkTraining = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButtonNetwork = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItemResetNetwork = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBoxHidden = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelNetworkOutput = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelLikelihood = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRandomExample = new System.Windows.Forms.ToolStripButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelCrossValidationPerformance = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelTrainingDataPerformance = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigit)).BeginInit();
            this.toolStripContainerMain.ContentPanel.SuspendLayout();
            this.toolStripContainerMain.TopToolStripPanel.SuspendLayout();
            this.toolStripContainerMain.SuspendLayout();
            this.toolStripNetworkTraining.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHidden)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxDigit
            // 
            this.pictureBoxDigit.Location = new System.Drawing.Point(9, 32);
            this.pictureBoxDigit.Name = "pictureBoxDigit";
            this.pictureBoxDigit.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxDigit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDigit.TabIndex = 0;
            this.pictureBoxDigit.TabStop = false;
            // 
            // labelClassLabel
            // 
            this.labelClassLabel.AutoSize = true;
            this.labelClassLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClassLabel.Location = new System.Drawing.Point(6, 99);
            this.labelClassLabel.Name = "labelClassLabel";
            this.labelClassLabel.Size = new System.Drawing.Size(42, 13);
            this.labelClassLabel.TabIndex = 2;
            this.labelClassLabel.Text = "Label:";
            // 
            // labelClass
            // 
            this.labelClass.AutoSize = true;
            this.labelClass.Location = new System.Drawing.Point(6, 112);
            this.labelClass.Name = "labelClass";
            this.labelClass.Size = new System.Drawing.Size(10, 13);
            this.labelClass.TabIndex = 3;
            this.labelClass.Text = "-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 227);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "add training / cross validation error graphs per epoch";
            // 
            // toolStripContainerMain
            // 
            // 
            // toolStripContainerMain.ContentPanel
            // 
            this.toolStripContainerMain.ContentPanel.Controls.Add(this.groupBox4);
            this.toolStripContainerMain.ContentPanel.Controls.Add(this.groupBox3);
            this.toolStripContainerMain.ContentPanel.Controls.Add(this.groupBox2);
            this.toolStripContainerMain.ContentPanel.Controls.Add(this.groupBox1);
            this.toolStripContainerMain.ContentPanel.Controls.Add(this.label1);
            this.toolStripContainerMain.ContentPanel.Size = new System.Drawing.Size(449, 253);
            this.toolStripContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainerMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainerMain.Name = "toolStripContainerMain";
            this.toolStripContainerMain.Size = new System.Drawing.Size(449, 278);
            this.toolStripContainerMain.TabIndex = 3;
            // 
            // toolStripContainerMain.TopToolStripPanel
            // 
            this.toolStripContainerMain.TopToolStripPanel.Controls.Add(this.toolStripNetworkTraining);
            this.toolStripContainerMain.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStripNetworkTraining
            // 
            this.toolStripNetworkTraining.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripNetworkTraining.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButtonNetwork});
            this.toolStripNetworkTraining.Location = new System.Drawing.Point(3, 0);
            this.toolStripNetworkTraining.Name = "toolStripNetworkTraining";
            this.toolStripNetworkTraining.Size = new System.Drawing.Size(110, 25);
            this.toolStripNetworkTraining.TabIndex = 0;
            // 
            // toolStripSplitButtonNetwork
            // 
            this.toolStripSplitButtonNetwork.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButtonNetwork.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemResetNetwork});
            this.toolStripSplitButtonNetwork.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonNetwork.Image")));
            this.toolStripSplitButtonNetwork.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonNetwork.Name = "toolStripSplitButtonNetwork";
            this.toolStripSplitButtonNetwork.Size = new System.Drawing.Size(98, 22);
            this.toolStripSplitButtonNetwork.Text = "&Train Network";
            this.toolStripSplitButtonNetwork.ToolTipText = "&Network";
            this.toolStripSplitButtonNetwork.ButtonClick += new System.EventHandler(this.toolStripSplitButtonNetwork_ButtonClick);
            // 
            // toolStripMenuItemResetNetwork
            // 
            this.toolStripMenuItemResetNetwork.Name = "toolStripMenuItemResetNetwork";
            this.toolStripMenuItemResetNetwork.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemResetNetwork.Text = "&Reset network";
            this.toolStripMenuItemResetNetwork.Click += new System.EventHandler(this.toolStripMenuItemResetNetwork_Click);
            // 
            // pictureBoxHidden
            // 
            this.pictureBoxHidden.Location = new System.Drawing.Point(9, 32);
            this.pictureBoxHidden.Name = "pictureBoxHidden";
            this.pictureBoxHidden.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxHidden.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxHidden.TabIndex = 4;
            this.pictureBoxHidden.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "20 x 20 px";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "5 x 5 px";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "1 neuron";
            // 
            // labelNetworkOutput
            // 
            this.labelNetworkOutput.AutoSize = true;
            this.labelNetworkOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNetworkOutput.Location = new System.Drawing.Point(6, 32);
            this.labelNetworkOutput.Name = "labelNetworkOutput";
            this.labelNetworkOutput.Size = new System.Drawing.Size(24, 25);
            this.labelNetworkOutput.TabIndex = 8;
            this.labelNetworkOutput.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxDigit);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.labelClassLabel);
            this.groupBox1.Controls.Add(this.labelClass);
            this.groupBox1.Location = new System.Drawing.Point(12, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(110, 132);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input activation";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBoxHidden);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(128, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(110, 132);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hidden activation";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.labelLikelihood);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.labelNetworkOutput);
            this.groupBox3.Location = new System.Drawing.Point(244, 66);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(110, 132);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output activation";
            // 
            // labelLikelihood
            // 
            this.labelLikelihood.AutoSize = true;
            this.labelLikelihood.Location = new System.Drawing.Point(7, 112);
            this.labelLikelihood.Name = "labelLikelihood";
            this.labelLikelihood.Size = new System.Drawing.Size(10, 13);
            this.labelLikelihood.TabIndex = 9;
            this.labelLikelihood.Text = "-";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Likelihood:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRandomExample});
            this.toolStrip1.Location = new System.Drawing.Point(113, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(115, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // toolStripButtonRandomExample
            // 
            this.toolStripButtonRandomExample.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRandomExample.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRandomExample.Image")));
            this.toolStripButtonRandomExample.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRandomExample.Name = "toolStripButtonRandomExample";
            this.toolStripButtonRandomExample.Size = new System.Drawing.Size(103, 22);
            this.toolStripButtonRandomExample.Text = "&Random example";
            this.toolStripButtonRandomExample.Click += new System.EventHandler(this.toolStripButtonRandomExample_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.labelTrainingDataPerformance);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.labelCrossValidationPerformance);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(12, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(226, 52);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Network performance";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Cross-validation:";
            // 
            // labelCrossValidationPerformance
            // 
            this.labelCrossValidationPerformance.AutoSize = true;
            this.labelCrossValidationPerformance.Location = new System.Drawing.Point(111, 16);
            this.labelCrossValidationPerformance.Name = "labelCrossValidationPerformance";
            this.labelCrossValidationPerformance.Size = new System.Drawing.Size(10, 13);
            this.labelCrossValidationPerformance.TabIndex = 12;
            this.labelCrossValidationPerformance.Text = "-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Training data:";
            // 
            // labelTrainingDataPerformance
            // 
            this.labelTrainingDataPerformance.AutoSize = true;
            this.labelTrainingDataPerformance.Location = new System.Drawing.Point(111, 29);
            this.labelTrainingDataPerformance.Name = "labelTrainingDataPerformance";
            this.labelTrainingDataPerformance.Size = new System.Drawing.Size(10, 13);
            this.labelTrainingDataPerformance.TabIndex = 14;
            this.labelTrainingDataPerformance.Text = "-";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 278);
            this.Controls.Add(this.toolStripContainerMain);
            this.Name = "MainForm";
            this.Text = "Handwritten Digit Classification";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigit)).EndInit();
            this.toolStripContainerMain.ContentPanel.ResumeLayout(false);
            this.toolStripContainerMain.ContentPanel.PerformLayout();
            this.toolStripContainerMain.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainerMain.TopToolStripPanel.PerformLayout();
            this.toolStripContainerMain.ResumeLayout(false);
            this.toolStripContainerMain.PerformLayout();
            this.toolStripNetworkTraining.ResumeLayout(false);
            this.toolStripNetworkTraining.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHidden)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxDigit;
        private System.Windows.Forms.Label labelClass;
        private System.Windows.Forms.Label labelClassLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripContainer toolStripContainerMain;
        private System.Windows.Forms.ToolStrip toolStripNetworkTraining;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonNetwork;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemResetNetwork;
        private System.Windows.Forms.PictureBox pictureBoxHidden;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelNetworkOutput;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelLikelihood;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRandomExample;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label labelCrossValidationPerformance;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelTrainingDataPerformance;
        private System.Windows.Forms.Label label7;
    }
}

