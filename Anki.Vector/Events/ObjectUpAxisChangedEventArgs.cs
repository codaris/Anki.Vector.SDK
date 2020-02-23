// <copyright file="ObjectUpAxisChangedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;

namespace Anki.Vector.Events
{
    /// <summary>
    ///  Object up axis changed event args
    /// </summary>
    /// <seealso cref="ObjectEventArgs" />
    public class ObjectUpAxisChangedEventArgs : ObjectEventArgs
    {
        /// <summary>
        /// Gets the robot timestamp of the event
        /// </summary>
        public uint RobotTimestamp { get; }

        /// <summary>
        /// Gets up axis.
        /// </summary>
        public UpAxis UpAxis { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectUpAxisChangedEventArgs" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="RobotObjectUpAxisChangedEventArgs" /> instance containing the event data.</param>
        internal ObjectUpAxisChangedEventArgs(ObservableObject obj, RobotObjectUpAxisChangedEventArgs e) : base(obj)
        {
            RobotTimestamp = e.RobotTimestamp;
            UpAxis = e.UpAxis;
        }
    }
}
