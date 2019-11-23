// <copyright file="VectorExternalAudioPlaybackException.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Exceptions
{
    /// <summary>
    /// Failed to play external audio on Vector.
    /// </summary>
    /// <seealso cref="Anki.Vector.Exceptions.VectorException" />
    public class VectorExternalAudioPlaybackException : VectorException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorExternalAudioPlaybackException"/> class.
        /// </summary>
        internal VectorExternalAudioPlaybackException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorExternalAudioPlaybackException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal VectorExternalAudioPlaybackException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorExternalAudioPlaybackException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        internal VectorExternalAudioPlaybackException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
