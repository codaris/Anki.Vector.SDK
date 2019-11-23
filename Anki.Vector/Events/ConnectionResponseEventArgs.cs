// <copyright file="ConnectionResponseEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Connection response event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ConnectionResponseEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets a value indicating whether this instance is primary.
        /// </summary>
        public bool IsPrimary { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionResponseEventArgs" /> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal ConnectionResponseEventArgs(ExternalInterface.Event e) : base(e)
        {
            IsPrimary = e.ConnectionResponse.IsPrimary;
        }
    }
}
