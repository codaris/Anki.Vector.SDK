// <copyright file="NavMapUpdateEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.Types;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Nav map update event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class NavMapUpdateEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the nav map grid.
        /// </summary>
        public NavMapGrid NavMap { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavMapUpdateEventArgs" /> class.
        /// </summary>
        /// <param name="response">The response.</param>
        internal NavMapUpdateEventArgs(ExternalInterface.NavMapFeedResponse response)
        {
            NavMap = new NavMapGrid(response);
        }
    }
}
