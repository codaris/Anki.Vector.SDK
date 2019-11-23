// <copyright file="VectorException.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Exceptions
{
    /// <summary>
    /// Base class of all Vector SDK exceptions.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public abstract class VectorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorException"/> class.
        /// </summary>
        internal VectorException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal VectorException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        internal VectorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
