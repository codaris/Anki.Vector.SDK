// <copyright file="VectorNotFoundException.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Exceptions
{
    /// <summary>
    /// Unable to establish a connection to Vector.
    /// <para>Make sure you’re on the same network, and Vector is connected to the Internet.</para>
    /// </summary>
    /// <seealso cref="Anki.Vector.Exceptions.VectorException" />
    public class VectorNotFoundException : VectorConnectionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorNotFoundException"/> class.
        /// </summary>
        internal VectorNotFoundException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal VectorNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        internal VectorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
