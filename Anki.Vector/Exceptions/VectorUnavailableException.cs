// <copyright file="VectorUnavailableException.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Exceptions
{
    /// <summary>
    /// Unable to reach Vector.
    /// </summary>
    /// <seealso cref="Anki.Vector.Exceptions.VectorConnectionException" />
    public class VectorUnavailableException : VectorConnectionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorUnavailableException"/> class.
        /// </summary>
        internal VectorUnavailableException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorUnavailableException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal VectorUnavailableException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorUnavailableException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        internal VectorUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
