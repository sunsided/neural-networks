using System;
using System.Globalization;
using System.Windows.Forms;
using Widemeadows.MachineLearning.Neural.Training;

namespace Widemeadows.MachineLearning.Neural.Demonstration.Digit
{
    /// <summary>
    /// Class ProgressDialog.
    /// </summary>
    public partial class TrainingProgressDialog : Form
    {
        /// <summary>
        /// The maximum number of iterations
        /// </summary>
        private readonly int _maxIterations;

        /// <summary>
        /// Occurs when the training has been canceled.
        /// </summary>
        public event EventHandler TrainingCanceled;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class.
        /// </summary>
        public TrainingProgressDialog(int maxIterations)
            : this()
        {
            _maxIterations = maxIterations;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class.
        /// </summary>
        private TrainingProgressDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the progress.
        /// </summary>
        /// <param name="progress">The progress.</param>
        public void UpdateProgress(TrainingProgress progress)
        {
            var amount = progress.Iteration*100/_maxIterations;
            progressBar1.Value = amount;
            labelTrainingCost.Text = progress.Cost.ToString(CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Handles the Click event of the buttonCancelTraining control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonCancelTraining_Click(object sender, EventArgs e)
        {
            OnTrainingCanceled();
        }

        /// <summary>
        /// Called when the training has been canceled.
        /// </summary>
        protected virtual void OnTrainingCanceled()
        {
            var handler = TrainingCanceled;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
