// <copyright file="ObjectEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    using System;
    using Anki.Vector.Objects;

    /// <summary>
    /// Abstract base class event args for all object events
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public abstract class ObjectEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the object that triggered the event
        /// </summary>
        public ObservableObject Object { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectEventArgs" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        internal ObjectEventArgs(ObservableObject obj)
        {
            Object = obj;
        }
    }
}
