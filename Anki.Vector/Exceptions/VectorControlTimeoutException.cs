// <copyright file="VectorControlTimeoutException.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Exceptions
{
    /// <summary>
    /// Failed to get control of Vector.
    /// <para>Please verify that Vector is connected to the Internet, is on a flat surface, and is fully charged.</para>
    /// </summary>
    /// <seealso cref="Anki.Vector.Exceptions.VectorException" />
    public class VectorControlTimeoutException : VectorException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorControlTimeoutException"/> class.
        /// </summary>
        public VectorControlTimeoutException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorControlTimeoutException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public VectorControlTimeoutException(string message) : base(message)
        {
        }
    }
}
