// <copyright file="FaceScanCompleteEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Face scan complete
    /// </summary>
    /// <seealso cref="TimestampedStatusEventArgs" />
    public class FaceScanCompleteEventArgs : TimestampedStatusEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaceScanCompleteEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal FaceScanCompleteEventArgs(ExternalInterface.Event e) : base(e)
        {
        }
    }
}
