// <copyright file="VectorRequestException.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.Types;

namespace Anki.Vector.Exceptions
{
    /// <summary>
    /// Request to Vector has failed
    /// </summary>
    /// <seealso cref="Anki.Vector.Exceptions.VectorException" />
    public class VectorRequestException : VectorException
    {
        /// <summary>
        /// Gets the status code of the request.
        /// </summary>
        public StatusCode StatusCode { get; } = StatusCode.Unknown;

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorRequestException"/> class.
        /// </summary>
        internal VectorRequestException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorRequestException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal VectorRequestException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorRequestException"/> class.
        /// </summary>
        internal VectorRequestException(StatusCode statusCode) : base($"Request failed with status code '{statusCode}'.")
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorRequestException" /> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message that describes the error.</param>
        internal VectorRequestException(StatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorRequestException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        internal VectorRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
