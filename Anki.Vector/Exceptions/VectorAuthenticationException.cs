// <copyright file="VectorAuthenticationException.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Exceptions
{
    /// <summary>
    /// Type of authentication failure.  
    /// </summary>
    public enum VectorAuthenticationFailureType
    {
        /// <summary>
        /// The authentication failure type is not known.
        /// </summary>
        Unknown,

        /// <summary>
        /// Unable to connect to robot for authentication.
        /// </summary>
        Connection,

        /// <summary>
        /// The serial number is not valid.
        /// </summary>
        SerialNumber,

        /// <summary>
        /// The IP address is missing and could not be determined automatically.
        /// </summary>
        IPAddress,

        /// <summary>
        /// The email address or password is not correct.
        /// </summary>
        Login
    }

    /// <summary>
    /// Failure during authentication
    /// </summary>
    /// <seealso cref="Anki.Vector.Exceptions.VectorException" />
    public class VectorAuthenticationException : VectorException
    {
        /// <summary>
        /// Gets the type of the failure.
        /// </summary>
        public VectorAuthenticationFailureType FailureType { get; } = VectorAuthenticationFailureType.Unknown;

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorAuthenticationException"/> class.
        /// </summary>
        internal VectorAuthenticationException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorAuthenticationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal VectorAuthenticationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorAuthenticationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        internal VectorAuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorAuthenticationException" /> class.
        /// </summary>
        /// <param name="failureType">Type of the failure.</param>
        /// <param name="message">The message that describes the error.</param>
        internal VectorAuthenticationException(VectorAuthenticationFailureType failureType, string message) : base(message)
        {
            FailureType = failureType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorAuthenticationException" /> class.
        /// </summary>
        /// <param name="failureType">Type of the failure.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        internal VectorAuthenticationException(VectorAuthenticationFailureType failureType, string message, Exception innerException) : base(message, innerException)
        {
            FailureType = failureType;
        }
    }
}
