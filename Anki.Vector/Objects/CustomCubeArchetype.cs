// <copyright file="CustomCubeArchetype.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Exceptions;
using Anki.Vector.Types;

namespace Anki.Vector.Objects
{
    /// <summary>
    /// Custom object cube definition
    /// </summary>
    /// <seealso cref="Anki.Vector.Objects.CustomObjectArchetype" />
    public class CustomCubeArchetype : CustomObjectArchetype
    {
        /// <summary>
        /// Gets or sets the marker affixed to every side of the cube.
        /// </summary>
        public CustomObjectMarker Marker { get => _marker; set => SetProperty(ref _marker, value); }
        private CustomObjectMarker _marker = CustomObjectMarker.Undefined;

        /// <summary>
        /// Gets or sets the size of each side of the cube (in millimeters).
        /// </summary>
        public float SizeMm { get => _sizeMm; set => SetProperty(ref _sizeMm, value); }
        private float _sizeMm = 0;

        /// <summary>
        /// Validates the custom object definition; raising an exception if invalid.
        /// </summary>
        internal override void Validate()
        {
            if (Marker == CustomObjectMarker.Undefined) throw new VectorInvalidValueException($"Custom cube marker cannot be undefined.");
            if (SizeMm <= 0) throw new VectorInvalidValueException($"Custom box size must be greater than zero.");
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"Custom Cube {CustomObjectType}";
        }

        /// <summary>
        /// Converts to robot CustomCubeDefinition instance.
        /// </summary>
        /// <returns>Robot custom cube definition.</returns>
        internal ExternalInterface.CustomCubeDefinition ToRobotCustomCubeDefinition()
        {
            return new ExternalInterface.CustomCubeDefinition()
            {
                Marker = Marker.ToRobotMarker(),
                SizeMm = SizeMm,
                MarkerWidthMm = MarkerWidthMm,
                MarkerHeightMm = MarkerHeightMm
            };
        }
    }
}
