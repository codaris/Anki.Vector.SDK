// <copyright file="ObjectConnectedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object disappeared event args
    /// </summary>
    /// <seealso cref="ObjectEventArgs" />
    public class ObjectConnectedEventArgs : ObjectEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConnectedEventArgs" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        internal ObjectConnectedEventArgs(ObservableObject obj) : base(obj)
        {
        }
    }
}
