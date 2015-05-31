using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;

namespace Widemeadows.MachineLearning.Neural.Demonstration.Digit
{
    static class Program
    {
        /// <summary>
        /// Asynchronously reads the given number of bytes from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="count">The count.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        private static async Task<byte[]> ReadBytesAsync(Stream stream, int count)
        {
            var buffer = new byte[count];
            var offset = 0;
            var remaining = count;
            do
            {
                var read = await stream.ReadAsync(buffer, offset, remaining);
                if (read == 0) return null;

                remaining -= read;
                offset += read;

            } while (remaining > 0);

            return buffer;
        }

        /// <summary>
        /// Asynchronously reads an integer from the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        private static async Task<int> ReadInt32Async(Stream stream)
        {
            var buffer = await ReadBytesAsync(stream, 4);
            if (buffer == null) return -1;

            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Asynchronously reads an integer from the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        private static async Task<float> ReadFloatAsync(Stream stream)
        {
            var buffer = await ReadBytesAsync(stream, 4);
            if (buffer == null) return float.NaN;

            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Loads the training examples.
        /// </summary>
        /// <returns>Task&lt;IReadOnlyCollection&lt;TrainingExample&gt;&gt;.</returns>
        public static async Task<IReadOnlyCollection<TrainingExample>> LoadTrainingExamplesAsync(IProgress<float> progress)
        {
            var collection = new Collection<TrainingExample>();
            var fi = new FileInfo("images.dat.gz");

            using (var fileStream = File.Open(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var stream = new GZipStream(fileStream, CompressionMode.Decompress))
            {
                var invLength = 1.0F/fileStream.Length;
                progress.Report(0.0F);

                do
                {
                    // read the label
                    var label = await ReadInt32Async(stream); // TODO: little endian / big endian?
                    if (label < 0) break;

                    // read the minimum value
                    var min = await ReadFloatAsync(stream);
                    if (float.IsNaN(min)) break;

                    // read the maximum value
                    var max = await ReadFloatAsync(stream);
                    if (float.IsNaN(max)) break;

                    // read the 400 pixel values
                    var pixels = new float[400];
                    for (int i = 0; i < pixels.Length; ++i)
                    {
                        pixels[i] = await ReadFloatAsync(stream);
                        Debug.Assert(!float.IsNaN(pixels[i]), "!float.IsNaN(pixels[i])");
                    }

                    // store the training example
                    var matrix = Matrix<float>.Build.Dense(20, 20, pixels);
                    collection.Add(new TrainingExample(label, matrix, min, max));

                    // report progress
                    progress.Report(fileStream.Position * invLength);

                } while (true);
            }

            return collection;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if false
            var input = new FileInfo("data.csv");
            var output = new FileInfo("images.dat.gz");

            using (var stream = File.Open(input.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var outstream = File.Open(output.FullName, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var compressedStream = new GZipStream(outstream, CompressionLevel.Optimal))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            using (var writer = new BinaryWriter(compressedStream, Encoding.UTF8))
            {
                string line;
                while (null != (line = reader.ReadLine()))
                {
                    var elements = line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                    Debug.Assert(elements.Length == 401, "elements.Length == 401");

                    // read in the data
                    var label = (int) Double.Parse(elements.First(), CultureInfo.InvariantCulture);
                    var values = elements.Skip(1).Select(value => Double.Parse(value, CultureInfo.InvariantCulture)).ToList();
                    var minMax = values.Aggregate(new Tuple<double, double>(float.PositiveInfinity, float.NegativeInfinity),
                        (minmax, value) => new Tuple<double, double>(minmax.Item1 > value ? value : minmax.Item1, minmax.Item2 < value ? value : minmax.Item2));
                    // double mean = values.Average();
                    // double std = values.WholePopulationStdDev(mean);

                    // write out the data
                    writer.Write(label); // label
                    writer.Write((float)minMax.Item1); // min
                    writer.Write((float)minMax.Item2); // max
                    // writer.Write((float)mean); // mean
                    // writer.Write((float)std); // standard deviation
                    foreach (var value in values)
                    {
                        writer.Write((float)value);
                    }
                    writer.Flush();
                }
            }
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
