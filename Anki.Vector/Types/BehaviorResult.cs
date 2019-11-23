// <copyright file="BehaviorResult.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
    /// <summary>
    /// Result from the behavior methods
    /// </summary>
    public class BehaviorResult
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        public StatusCode StatusCode { get; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        public BehaviorResultCode Result { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorResult"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="results">The results.</param>
        internal BehaviorResult(ExternalInterface.ResponseStatus.Types.StatusCode statusCode, ExternalInterface.BehaviorResults results)
        {
            StatusCode = (StatusCode)statusCode;
            Result = (BehaviorResultCode)results;
        }
    }
}
