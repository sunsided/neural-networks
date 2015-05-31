using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Widemeadows.MachineLearning.Neural.Training;
using ZedGraph;

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
        /// The graph's point pair list
        /// </summary>
        private readonly PointPairList _list;

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

            var graph = costGraphControl;
            graph.BorderStyle = BorderStyle.None;
            graph.BackColor = Color.Transparent;

            var pane = graph.GraphPane;

            pane.Title.IsVisible = false;
            pane.XAxis.Title.IsVisible = false;
            pane.YAxis.Title.IsVisible = false;
            pane.TitleGap = 0;
            pane.Legend.IsVisible = false;
            pane.Border.IsVisible = false;
            pane.Fill.Color = BackColor;
            pane.Chart.Fill.Color = Color.White;

            pane.YAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MinorGrid.IsVisible = true;

            pane.XAxis.Scale.Min = 0;
            pane.XAxis.Scale.MinorStep = 1;
            pane.XAxis.Scale.MajorStepAuto = true;
            pane.XAxis.Scale.Mag = 0;
            pane.YAxis.Type = AxisType.Log;

            var list = new PointPairList();
            _list = list;

            var pointsCurve = pane.AddCurve("Cost", list, Color.DarkRed);
            pointsCurve.Line.IsVisible = true;
            pointsCurve.Line.Width = 2;

            pane.AxisChange();
            graph.Refresh();
        }

        /// <summary>
        /// Adds the point to the plot.
        /// </summary>
        /// <param name="iteration">The iteration.</param>
        /// <param name="cost">The cost.</param>
        private void AddPoint(int iteration, float cost)
        {
            _list.Add(new PointPair(iteration, cost));

            var graph = costGraphControl;
            graph.GraphPane.AxisChange();
            graph.Refresh();
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

            AddPoint(progress.Iteration, progress.Cost);
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
