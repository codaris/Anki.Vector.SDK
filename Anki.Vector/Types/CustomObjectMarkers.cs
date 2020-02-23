// <copyright file="CustomObjectMarkers.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Exceptions;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Defines all available custom object markers.
    /// </summary>
    public enum CustomObjectMarker
    {
        /// <summary>
        /// The marker is not defined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The custom marker circles2.  Images/SDK_2Circles.png
        /// </summary>
        CustomMarkerCircles2 = 1,

        /// <summary>
        /// The custom marker circles3.  Images/SDK_3Circles.png
        /// </summary>
        CustomMarkerCircles3 = 2,

        /// <summary>
        /// The custom marker circles4.  Images/SDK_4Circles.png
        /// </summary>
        CustomMarkerCircles4 = 3,

        /// <summary>
        /// The custom marker circles5.  Images/SDK_5Circles.png
        /// </summary>
        CustomMarkerCircles5 = 4,

        /// <summary>
        /// The custom marker diamonds2.  Images/SDK_2Diamonds.png
        /// </summary>
        CustomMarkerDiamonds2 = 5,

        /// <summary>
        /// The custom marker diamonds3.  Images/SDK_3Diamonds.png
        /// </summary>
        CustomMarkerDiamonds3 = 6,

        /// <summary>
        /// The custom marker diamonds4.  Images/SDK_4Diamonds.png
        /// </summary>
        CustomMarkerDiamonds4 = 7,

        /// <summary>
        /// The custom marker diamonds5.  Images/SDK_5Diamonds.png
        /// </summary>
        CustomMarkerDiamonds5 = 8,

        /// <summary>
        /// The custom marker hexagons2.  Images/SDK_2Hexagons.png
        /// </summary>
        CustomMarkerHexagons2 = 9,

        /// <summary>
        /// The custom marker hexagons3.  Images/SDK_3Hexagons.png
        /// </summary>
        CustomMarkerHexagons3 = 10,

        /// <summary>
        /// The custom marker hexagons4.  Images/SDK_4Hexagons.png
        /// </summary>
        CustomMarkerHexagons4 = 11,

        /// <summary>
        /// The custom marker hexagons5.  Images/SDK_5Hexagons.png
        /// </summary>
        CustomMarkerHexagons5 = 12,

        /// <summary>
        /// The custom marker triangles2.  Images/SDK_2Triangles.png
        /// </summary>
        CustomMarkerTriangles2 = 13,

        /// <summary>
        /// The custom marker triangles3.  Images/SDK_3Triangles.png
        /// </summary>
        CustomMarkerTriangles3 = 14,

        /// <summary>
        /// The custom marker triangles4.  Images/SDK_4Triangles.png
        /// </summary>
        CustomMarkerTriangles4 = 15,

        /// <summary>
        /// The custom marker triangles5.  Images/SDK_5Triangles.png
        /// </summary>
        CustomMarkerTriangles5 = 16,
    }

    /// <summary>
    /// Convert custom object marker to robot marker
    /// </summary>
    internal static class CustomObjectMarkerExtensions
    {
        /// <summary>
        /// Converts to robot marker.
        /// </summary>
        /// <param name="customObjectMarker">The custom object marker.</param>
        /// <returns>Robot custom object marker</returns>
        internal static ExternalInterface.CustomObjectMarker ToRobotMarker(this CustomObjectMarker customObjectMarker)
        {
            if (customObjectMarker == CustomObjectMarker.Undefined) throw new VectorInvalidValueException("Custom object marker cannot be undefined.");
            return (ExternalInterface.CustomObjectMarker)customObjectMarker;
        }
    }
}
