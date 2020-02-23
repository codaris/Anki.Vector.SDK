// <copyright file="ActionResult.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
    /// <summary>
    /// The result of the action method
    /// </summary>
    public class ActionResult
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        public StatusCode StatusCode { get; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        public ActionResultCode Result { get; }

        /// <summary>
        /// Gets a value indicating whether this instance indicates success.
        /// </summary>
        public bool IsSuccess => Result == ActionResultCode.ActionResultSuccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionResult"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="resultCode">The results.</param>
        internal ActionResult(ExternalInterface.ResponseStatus.Types.StatusCode statusCode, ExternalInterface.ActionResult.Types.ActionResultCode resultCode)
        {
            StatusCode = statusCode.Convert();
            Result = resultCode.Convert();
        }
    }
}
