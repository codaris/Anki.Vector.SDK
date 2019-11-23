// <copyright file="ObjectDisappearedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object disappeared event args
    /// </summary>
    /// <seealso cref="ObjectEventArgs" />
    public class ObjectDisappearedEventArgs : ObjectEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDisappearedEventArgs" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        internal ObjectDisappearedEventArgs(ObservableObject obj) : base(obj)
        {
        }
    }
}
