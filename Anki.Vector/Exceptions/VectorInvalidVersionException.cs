// <copyright file="VectorInvalidVersionException.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Exceptions
{
    /// <summary>
    /// Your SDK version is not compatible with Vector’s version.
    /// </summary>
    /// <seealso cref="Anki.Vector.Exceptions.VectorException" />
    public class VectorInvalidVersionException : VectorException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorInvalidVersionException"/> class.
        /// </summary>
        internal VectorInvalidVersionException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorInvalidVersionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal VectorInvalidVersionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorInvalidVersionException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        internal VectorInvalidVersionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
