// <copyright file="VectorControlException.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Exceptions
{
    /// <summary>
    /// Unable to run a function which requires behavior control.
    /// </summary>
    /// <seealso cref="Anki.Vector.Exceptions.VectorException" />
    public class VectorControlException : VectorException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorControlException"/> class.
        /// </summary>
        public VectorControlException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorControlException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public VectorControlException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorControlException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        internal VectorControlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
