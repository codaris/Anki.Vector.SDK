// <copyright file="VectorConfigurationException.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Exceptions
{
    /// <summary>
    /// The <see cref="IRobotConfiguration"/> instance is not valid.
    /// </summary>
    /// <seealso cref="Anki.Vector.Exceptions.VectorException" />
    public class VectorConfigurationException : VectorException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorConfigurationException"/> class.
        /// </summary>
        public VectorConfigurationException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public VectorConfigurationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public VectorConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
