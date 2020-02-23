// <copyright file="ObjectTappedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object tapped event args
    /// </summary>
    /// <seealso cref="ObjectEventArgs" />
    public class ObjectTappedEventArgs : ObjectEventArgs
    {
        /// <summary>
        /// Gets the robot timestamp of the event
        /// </summary>
        public uint RobotTimestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectTappedEventArgs" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="RobotObjectTappedEventArgs" /> instance containing the event data.</param>
        internal ObjectTappedEventArgs(ObservableObject obj, RobotObjectTappedEventArgs e) : base(obj)
        {
            RobotTimestamp = e.RobotTimestamp;
        }
    }
}
