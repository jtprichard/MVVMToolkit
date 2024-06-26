﻿using System.Threading;

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
        /// Total items to process
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Message on window
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Group message on window
        /// </summary>
        public string GroupMessage { get; set; }

        /// <summary>
        /// The Cancellation Token Source
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; set; }

        /// <summary>
        /// The CancellationToken
        /// </summary>
        public CancellationToken CancellationToken => CancellationTokenSource.Token;

        #endregion

        #region Constructors

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ProgressData()
        {
            CancellationTokenSource = new CancellationTokenSource();
        }

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


        /// <summary>
        /// Constructor with cancellation token source (required for abort)
        /// </summary>
        public ProgressData(CancellationTokenSource cts)
        {
            CancellationTokenSource = cts;
        }


        #endregion

    }
}
