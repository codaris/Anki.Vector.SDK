// <copyright file="NavMapGridNode.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Anki.Vector.ExternalInterface;
using Anki.Vector.Objects;

namespace Anki.Vector.Types
{
    /// <summary>
    /// The content types for a <see cref="NavMapGridNode" />.
    /// </summary>
    public enum NavNodeContentType
    {
        /// <summary>The contents of the node is unknown.</summary>
        NavNodeUnknown = 0,
        /// <summary>The node is clear of obstacles, because Vector has seen objects on the other side, but it might contain a cliff. The node will be marked as either <see cref="NavNodeCliff"/> or <see cref="NavNodeClearOfCliff"/> once Vector has driven there.</summary>
        NavNodeClearOfObstacle = 1,
        /// <summary>The node is clear of any cliffs (a sharp drop) or obstacles.</summary>
        NavNodeClearOfCliff = 2,
        /// <summary>The node contains a <see cref="LightCube"/>.</summary>
        NavNodeObstacleCube = 3,
        /// <summary>The node contains a proximity detected obstacle which has not been explored.</summary>
        NavNodeObstacleProximity = 4,
        /// <summary>The node contains a proximity detected obstacle which has been explored.</summary>
        NavNodeObstacleProximityExplored = 5,
        /// <summary>The node contains an unrecognized obstacle.</summary>
        NavNodeObstacleUnrecognized = 6,
        /// <summary>The node contains a cliff (a sharp drop).</summary>
        NavNodeCliff = 7,
        /// <summary>The node contains a visible edge (based on the camera feed).</summary>
        NavNodeInterestingEdge = 8,
        /// <summary>This entry is undocumented and not currently used</summary>
        NavNodeNonInterestingEdge = 9
    }

    /// <summary>
    /// A node in the NavMap
    /// <para>Leaf nodes contain content, all other nodes are split into 4 equally sized children.</para>
    /// <para>Child node indices are stored in the following X,Y orientation:
    ///
    ///    +---+----+---+
    ///    | ^ | 2  | 0 |
    ///    +---+----+---+
    ///    | Y | 3  | 1 |
    ///    +---+----+---+
    ///    |   | X->|   |
    ///    +---+----+---+
    ///    
    /// </para></summary>
    public class NavMapGridNode
    {
        /// <summary>
        /// Gets the depth of this node (i.e. how far down the quad-tree it is).
        /// </summary>
        public int Depth { get; }

        /// <summary>
        /// Gets the size (width or length) of this square node in mm.
        /// </summary>
        public float Size { get; }

        /// <summary>
        /// Gets the center of this node.
        /// </summary>
        public Vector3 Center { get; }

        /// <summary>
        /// Gets the parent of this node. Is null for the root node.
        /// </summary>
        public NavMapGridNode Parent { get; } = null;

        /// <summary>
        /// Gets the content type in this node.  Only leaf nodes have content, this is <c>null</c> for all other nodes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "Method used for point access")]
        public NavNodeContentType? Content { get; private set; } = null;

        /// <summary>
        /// Gets the color of the node in RGBA.
        /// </summary>
        public uint ColorRgba { get; private set; }

        /// <summary>
        /// The child nodes.  Null for leaf nodes and an array of 4 child nodes otherwise.
        /// </summary>
        private NavMapGridNode[] children = null;

        /// <summary>
        /// The next child; used when building to track which branch to follow
        /// </summary>
        private int nextChild = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavMapGridNode"/> class.  This is for the root node.
        /// </summary>
        /// <param name="navMapInfo">The nav map information.</param>
        internal NavMapGridNode(NavMapInfo navMapInfo)
        {
            Depth = navMapInfo.RootDepth;
            Size = navMapInfo.RootSizeMm;
            Center = new Vector3(navMapInfo.RootCenterX, navMapInfo.RootCenterY, navMapInfo.RootCenterZ);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavMapGridNode"/> class.
        /// </summary>
        /// <param name="depth">The depth.</param>
        /// <param name="size">The size.</param>
        /// <param name="center">The center.</param>
        /// <param name="parent">The parent.</param>
        private NavMapGridNode(int depth, float size, Vector3 center, NavMapGridNode parent)
        {
            Depth = depth;
            Size = size;
            Center = center;
            Parent = parent;
        }

        /// <summary>
        /// Test if the node contains the given x,y coordinates.
        /// </summary>
        /// <param name="x">The x coordinate for the point.</param>
        /// <param name="y">The y coordinate for the point.</param>
        /// <returns>
        ///   <c>true</c> if the node contains point; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsPoint(float x, float y)
        {
            var halfSize = Size * 0.5f;
            var distX = Math.Abs(Center.X - x);
            var distY = Math.Abs(Center.Y - y);
            return (distX <= halfSize) && (distY <= halfSize);
        }

        /// <summary>
        /// Get the node at the given x, y coordinates.
        /// </summary>
        /// <param name="x">The x coordinate for the point.</param>
        /// <param name="y">The y coordinate for the point.</param>
        /// <returns>The smallest node that includes the point.  Will return <c>null</c> if point is outside the map.</returns>
        public NavMapGridNode GetNode(float x, float y)
        {
            if (!this.ContainsPoint(x, y)) return null;
            return GetNodeRecursive(x, y);
        }

        /// <summary>
        /// Recursively searches for the node at the given x, y coordinates.
        /// </summary>
        /// <param name="x">The x coordinate for the point.</param>
        /// <param name="y">The y coordinate for the point.</param>
        /// <returns>The smallest node that includes the point.</returns>
        private NavMapGridNode GetNodeRecursive(float x, float y)
        {
            if (children == null) return this;

            var xOffset = (x < Center.X) ? 2 : 0;
            var yOffset = (y < Center.Y) ? 1 : 0;
            // Search child node
            return children[xOffset + yOffset].GetNodeRecursive(x, y);
        }

        /// <summary>
        /// Get the node's content at the given x,y coordinates.
        /// </summary>
        /// <param name="x">The x coordinate for the point.</param>
        /// <param name="y">The y coordinate for the point.</param>
        /// <returns>The content included at that point.  Will be <see cref="NavNodeContentType.NavNodeUnknown"/> if point is outside the map.</returns>
        public NavNodeContentType GetContent(float x, float y)
        {
            return GetNode(x, y)?.Content ?? NavNodeContentType.NavNodeUnknown;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has child nodes.
        /// </summary>
        public bool HasChildren => children != null;

        /// <summary>
        /// Gets the child nodes of this node
        /// </summary>
        public IEnumerable<NavMapGridNode> Children
        {
            get
            {
                if (children == null) yield break;
                foreach (var child in children) yield return child;
            }
        }

        /// <summary>
        /// Add a child node to the quad tree.
        /// <para>The quad-tree is serialized to a flat list of nodes, we deserialize back to a quad-tree structure here, with the depth of each node indicating where it is placed.</para>
        /// </summary>
        /// <param name="quadInfo">The quad information.</param>
        /// <returns><c>true</c> if parent should use the next child for future AddChild calls.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// quadInfo - NavMapGridNode depth '{quadInfo.Depth}' greater than '{Depth}'.
        /// or
        /// quadInfo - NavMapGridNode nextChild '{nextChild}' greater than 3 at depth '{Depth}'.
        /// </exception>
        /// <exception cref="InvalidOperationException">NavMapGridNode clobbering {Content} at depth {Depth} with {quadInfo.Content}</exception>
        internal bool AddChild(NavMapQuadInfo quadInfo)
        {
            if (quadInfo.Depth > Depth) throw new ArgumentOutOfRangeException(nameof(quadInfo), $"NavMapGridNode depth '{quadInfo.Depth}' greater than '{Depth}'.");
            if (nextChild > 3) throw new ArgumentOutOfRangeException(nameof(quadInfo), $"NavMapGridNode nextChild '{nextChild}' greater than 3 at depth '{Depth}'.");

            if (Depth == quadInfo.Depth)
            {
                if (Content != null) throw new InvalidOperationException($"NavMapGridNode clobbering {Content} at depth {Depth} with {quadInfo.Content}");
                ColorRgba = quadInfo.ColorRgba;
                Content = (NavNodeContentType)quadInfo.Content;
                // This node won't be further subdivided and is now full
                return true;
            }

            if (children == null)
            {
                var nextDepth = Depth - 1;
                var nextSize = Size * 0.5f;
                var offset = nextSize * 0.5f;
                var center1 = new Vector3(Center.X + offset, Center.Y + offset, Center.Z);
                var center2 = new Vector3(Center.X + offset, Center.Y - offset, Center.Z);
                var center3 = new Vector3(Center.X - offset, Center.Y + offset, Center.Z);
                var center4 = new Vector3(Center.X - offset, Center.Y - offset, Center.Z);
                children = new NavMapGridNode[4]
                {
                    new NavMapGridNode(nextDepth, nextSize, center1, this),
                    new NavMapGridNode(nextDepth, nextSize, center2, this),
                    new NavMapGridNode(nextDepth, nextSize, center3, this),
                    new NavMapGridNode(nextDepth, nextSize, center4, this)
                };
            }

            if (children[nextChild].AddChild(quadInfo))
            {
                // The child node is now full, start using next child
                nextChild++;
            }

            // If all children are now full, parent should start using next child.  Otherwise empty children remain and parent can keep using child.
            return nextChild > 3;
        }
    }
}
