using System;
using System.Collections.Generic;
using System.Linq;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Vector atttention transfer info event args
    /// </summary>
    [Serializable]
    public class AttentionTransferEventArgs : RobotEventArgs
    {
        /// <summary>
        /// The reason that the attention was changed.
        /// </summary>
        public global::Anki.Vector.ExternalInterface.AttentionTransferReason Reason { get; }

        /// <summary>
        /// How long ago the attention was changed.
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
