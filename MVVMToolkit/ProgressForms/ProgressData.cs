namespace PB.MVVMToolkit.ProgressForms
{
    /// <summary>
    /// Data for progress form window
    /// </summary>
    public class ProgressData
    {
        #region Properties

        /// <summary>
        /// Current item being processed
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Message on window
        /// </summary>
        public string Message { get; set; }

        #endregion


        #region Constructors

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ProgressData() { }

        /// <summary>
        /// Constructor with Count
        /// </summary>
        /// <param name="count">Item number as integer</param>
        public ProgressData(int count)
        {
            Count = count;
        }

        /// <summary>
        /// Constructor with message
        /// </summary>
        /// <param name="message">Message as string</param>
        public ProgressData(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Constructor with count and message
        /// </summary>
        /// <param name="count">Item number as integer</param>
        /// <param name="message">Message as string</param>
        public ProgressData(int count, string message)
        {
            Count = count;
            Message = message;
        }

        #endregion

    }
}
