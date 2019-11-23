// <copyright file="VectorTimeoutException.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Exceptions
{
    /// <summary>
    /// Message took too long to complete.
    /// </summary>
    /// <seealso cref="Anki.Vector.Exceptions.VectorConnectionException" />
    public class VectorTimeoutException : VectorConnectionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorTimeoutException"/> class.
        /// </summary>
        internal VectorTimeoutException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorTimeoutException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal VectorTimeoutException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorTimeoutException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        internal VectorTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
