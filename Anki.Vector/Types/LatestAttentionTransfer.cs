// <copyright file="LatestAttentionTransfer.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Vector attention transfer info event args
    /// </summary>
    public class LatestAttentionTransfer
    {
        /// <summary>
        /// Gets the reason that the attention was changed.
        /// </summary>
        public AttentionTransferReason Reason { get; }

        /// <summary>
        /// Gets how long ago the attention was changed.
        /// </summary>
        public float SecondsAgo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatestAttentionTransfer"/> class.
        /// </summary>
        /// <param name="attentionTransfer">The attention transfer information.</param>
        internal LatestAttentionTransfer(Anki.Vector.ExternalInterface.LatestAttentionTransfer attentionTransfer)
        {
            SecondsAgo = attentionTransfer.AttentionTransfer.SecondsAgo;
            Reason = attentionTransfer.AttentionTransfer.Reason.Convert();
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
