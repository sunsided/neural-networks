using ZedGraph;

namespace Widemeadows.MachineLearning.Neural.Demonstration.Digit
{
    partial class TrainingProgressDialog
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
            this.components = new System.ComponentModel.Container();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonCancelTraining = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTrainingCost = new System.Windows.Forms.Label();
            this.costGraphControl = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 180);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(336, 25);
            this.progressBar1.TabIndex = 0;
            // 
            // buttonCancelTraining
            // 
            this.buttonCancelTraining.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelTraining.Location = new System.Drawing.Point(273, 211);
            this.buttonCancelTraining.Name = "buttonCancelTraining";
            this.buttonCancelTraining.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelTraining.TabIndex = 1;
            this.buttonCancelTraining.Text = "&Cancel";
            this.buttonCancelTraining.UseVisualStyleBackColor = true;
            this.buttonCancelTraining.Click += new System.EventHandler(this.buttonCancelTraining_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current training cost:";
            // 
            // labelTrainingCost
            // 
            this.labelTrainingCost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTrainingCost.AutoSize = true;
            this.labelTrainingCost.Location = new System.Drawing.Point(122, 211);
            this.labelTrainingCost.Name = "labelTrainingCost";
            this.labelTrainingCost.Size = new System.Drawing.Size(10, 13);
            this.labelTrainingCost.TabIndex = 3;
            this.labelTrainingCost.Text = "-";
            // 
            // costGraphControl
            // 
            this.costGraphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.costGraphControl.Location = new System.Drawing.Point(12, 12);
            this.costGraphControl.Name = "costGraphControl";
            this.costGraphControl.ScrollGrace = 0D;
            this.costGraphControl.ScrollMaxX = 0D;
            this.costGraphControl.ScrollMaxY = 0D;
            this.costGraphControl.ScrollMaxY2 = 0D;
            this.costGraphControl.ScrollMinX = 0D;
            this.costGraphControl.ScrollMinY = 0D;
            this.costGraphControl.ScrollMinY2 = 0D;
            this.costGraphControl.Size = new System.Drawing.Size(336, 162);
            this.costGraphControl.TabIndex = 4;
            // 
            // TrainingProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 243);
            this.ControlBox = false;
            this.Controls.Add(this.costGraphControl);
            this.Controls.Add(this.labelTrainingCost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancelTraining);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrainingProgressDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Progress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button buttonCancelTraining;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTrainingCost;
        private ZedGraphControl costGraphControl;
    }
}