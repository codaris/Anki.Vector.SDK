// <copyright file="NavMapGrid.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// A navigation memory map, stored as a quad-tree.
    /// </summary>
    /// <seealso cref="Anki.Vector.Types.NavMapGridNode" />
    public class NavMapGrid : NavMapGridNode
    {
        /// <summary>
        /// Gets the origin identifier
        /// </summary>
        public uint OriginId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavMapGrid"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        internal NavMapGrid(NavMapFeedResponse response) : base(response.MapInfo)
        {
            OriginId = response.OriginId;
            // Add all child nodes
            foreach (var quadInfo in response.QuadInfos) this.AddChild(quadInfo);
        }
    }
}
