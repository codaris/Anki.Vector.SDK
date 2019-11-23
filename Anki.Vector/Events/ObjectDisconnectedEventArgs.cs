// <copyright file="ObjectDisconnectedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object disappeared event args
    /// </summary>
    /// <seealso cref="ObjectEventArgs" />
    public class ObjectDisconnectedEventArgs : ObjectEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDisconnectedEventArgs" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        internal ObjectDisconnectedEventArgs(ObservableObject obj) : base(obj)
        {
        }
    }
}
