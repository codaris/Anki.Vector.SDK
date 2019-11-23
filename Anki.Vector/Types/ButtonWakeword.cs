// <copyright file="ButtonWakeword.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
    /// <summary>
    /// The service that responds when clicking Vector's back button
    /// </summary>
    public enum ButtonWakeWord
    {
        /// <summary>The button triggers Hey Vector.</summary>
        HeyVector = 0,
        /// <summary>The button triggers Alexa.</summary>
        Alexa = 1
    }
}
