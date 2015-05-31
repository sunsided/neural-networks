namespace Widemeadows.MachineLearning.Neural.Training
{
    /// <summary>
    /// Enum TrainingProgress
    /// </summary>
    public struct TrainingProgress
    {
        /// <summary>
        /// The iteration
        /// </summary>
        public readonly int Iteration;

        /// <summary>
        /// The network cost
        /// </summary>
        public readonly float Cost;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainingProgress"/> struct.
        /// </summary>
        /// <param name="iteration">The iteration.</param>
        /// <param name="cost">The cost.</param>
        public TrainingProgress(int iteration, float cost)
        {
            Iteration = iteration;
            Cost = cost;
        }
    }
}
