// <copyright file="ErrorEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Event args for background SDK errors
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the exception that caused the disconnection, if one exists.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventArgs" /> class.
        /// </summary>
        /// <param name="exception">The exception that caused the disconnection.</param>
        internal ErrorEventArgs(Exception exception)
        {
            var aggregateExcepton = (exception as AggregateException)?.Flatten();
            if (aggregateExcepton != null && aggregateExcepton.InnerExceptions.Count == 1)
            {
                Exception = aggregateExcepton.InnerException;
            }
            else
            {
                Exception = exception;
            }
        }
    }
}
