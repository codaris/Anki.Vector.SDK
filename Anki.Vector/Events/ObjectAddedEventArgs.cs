// <copyright file="ObjectAddedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object added event args
    /// </summary>
    /// <seealso cref="ObjectEventArgs" />
    public class ObjectAddedEventArgs : ObjectEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectAddedEventArgs" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        internal ObjectAddedEventArgs(ObservableObject obj) : base(obj)
        {
        }
    }
}
