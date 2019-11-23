// <copyright file="IAnimation.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Types
{
    /// <summary>
    /// Interface for animation and animation triggers
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        /// Gets the name of the animation or trigger
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the friendly name of the animation or trigger
        /// </summary>
        string FriendlyName { get; }
    }
}
