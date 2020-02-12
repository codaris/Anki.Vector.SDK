using System.Collections.Generic;
using System.Linq;

namespace Anki.Vector
{
    /// <summary>
    /// Vector atttention transfer info event args
    /// </summary>
    public class LatestAttentionTransfer
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
        /// Initializes a new instance of the <see cref="LatestAttentionTransfer"/> class.
        /// </summary>
        /// <param name="attentionTransfer">The attention transfer information.</param>
        internal LatestAttentionTransfer(Anki.Vector.ExternalInterface.LatestAttentionTransfer attentionTransfer)
        {
            SecondsAgo = attentionTransfer.AttentionTransfer.SecondsAgo;
            Reason = attentionTransfer.AttentionTransfer.Reason;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Reason}";
        }
    }
}
