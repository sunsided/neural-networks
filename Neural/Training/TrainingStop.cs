namespace Widemeadows.MachineLearning.Neural.Training
{
    /// <summary>
    /// Enum TrainingStop
    /// </summary>
    public enum TrainingStop
    {
        /// <summary>
        /// The maximum number of iterations has been reached
        /// </summary>
        MaximumIterationsReached,

        /// <summary>
        /// The predefined cost epsilon was undershot
        /// </summary>
        EpsilonReached
    }
}
