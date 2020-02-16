// <copyright file="BehaviorResultCode.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// The result of a behavior
    /// </summary>
    public enum BehaviorResultCode
    {
        Invalid = 0,
        Complete = 1,
        WontActivate = 2
    }

    internal static class BehaviorResultCodeExtensions
    {
        /// <summary>
        /// Converts from Robot result code to SDK result code
        /// </summary>
        /// <param name="results">The behavior results.</param>
        /// <returns>Converted result</returns>
        internal static BehaviorResultCode Convert(this ExternalInterface.BehaviorResults results)
        {
            return (BehaviorResultCode)results;
        }
    }
}
