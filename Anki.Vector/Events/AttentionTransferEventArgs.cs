// <copyright file="AttentionTransferEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Vector attention transfer info event args
    /// </summary>
    [Serializable]
    public class AttentionTransferEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the reason that the attention was changed.
        /// </summary>
        public global::Anki.Vector.ExternalInterface.AttentionTransferReason Reason { get; }

        /// <summary>
        /// Gets how long ago the attention was changed.
        /// </summary>
        public float SecondsAgo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttentionTransferEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal AttentionTransferEventArgs(ExternalInterface.Event e) : base(e)
        {
            var attentionTransfer = e.AttentionTransfer;
            SecondsAgo = attentionTransfer.SecondsAgo;
            Reason = attentionTransfer.Reason;
        }
    }
}
