using System;
using System.Windows.Forms;

namespace Widemeadows.MachineLearning.Neural.Demonstration.Digit
{
    /// <summary>
    /// Class ProgressDialog.
    /// </summary>
    public partial class ProgressDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class.
        /// </summary>
        public ProgressDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the progress.
        /// </summary>
        /// <param name="progress">The progress.</param>
        public void UpdateProgress(float progress)
        {
            progressBar1.Value = (int)Math.Floor(progress*100F);
        }
    }
}
