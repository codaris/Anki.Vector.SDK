// <copyright file="RobotObjectAvailableEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Cube availability event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RobotObjectAvailableEventArgs : RobotObjectEventArgs
    {
        /// <summary>
        /// Gets the factory identifier of the available cube
        /// </summary>
        public string FactoryId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotObjectAvailableEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal RobotObjectAvailableEventArgs(ExternalInterface.Event e) : base(e)
        {
            FactoryId = e.ObjectEvent.ObjectAvailable.FactoryId;
        }
    }
}
