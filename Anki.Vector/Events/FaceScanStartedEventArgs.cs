// <copyright file="FaceScanStartedEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Face scan started
    /// </summary>
    /// <seealso cref="TimestampedStatusEventArgs" />
    public class FaceScanStartedEventArgs : TimestampedStatusEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaceScanStartedEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal FaceScanStartedEventArgs(ExternalInterface.Event e) : base(e)
        {
        }
    }
}
