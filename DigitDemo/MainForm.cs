using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace Widemeadows.MachineLearning.Neural.Demonstration.Digit
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Show();
            var dialog = new ProgressDialog();

            IReadOnlyCollection<TrainingExample> data = null;
            dialog.Shown += async (sender, args) =>
                            {
                                var progress = new Progress<float>();
                                progress.ProgressChanged += (task, value) => dialog.UpdateProgress(value);

                                data = await Program.LoadTrainingExamplesAsync(progress);
                                dialog.Close();
                            };
            dialog.ShowDialog(this);
            Debug.Assert(data != null, "data != null");

            PresentTrainingData(data);
        }

        /// <summary>
        /// Presents the training data.
        /// </summary>
        /// <param name="data">The data.</param>
        private void PresentTrainingData(IReadOnlyCollection<TrainingExample> data)
        {
            var example = data.First();
            var bitmap = CreateBitmap(example);

            pictureBoxDigit.Image = bitmap;
        }

        /// <summary>
        /// Creates the bitmap.
        /// </summary>
        /// <param name="example">The example.</param>
        /// <returns>Bitmap.</returns>
        [NotNull]
        private Bitmap CreateBitmap(TrainingExample example)
        {
            var bitmap = new Bitmap(20, 20, PixelFormat.Format32bppArgb);

            var min = example.Min;
            var max = example.Max;
            var pixels = example.Pixels.Map(v => (float) (255*(v - min)/(max - min)));

            foreach (var pixel in pixels.EnumerateIndexed())
            {
                var y = pixel.Item1;
                var x = pixel.Item2;
                var value = (int)Math.Floor(pixel.Item3);

                bitmap.SetPixel(x, y, Color.FromArgb(value, value, value));
            }

            return bitmap;
        }
    }
}
