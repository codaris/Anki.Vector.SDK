// <copyright file="CustomWallArchetype.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Exceptions;
using Anki.Vector.Types;

namespace Anki.Vector.Objects
{
    /// <summary>
    /// Custom object wall definition
    /// </summary>
    /// <seealso cref="Anki.Vector.Objects.CustomObjectArchetype" />
    public class CustomWallArchetype : CustomObjectArchetype
    {
        /// <summary>
        /// Gets or sets the marker affixed to every side of the cube.
        /// </summary>
        public CustomObjectMarker Marker { get => _marker; set => SetProperty(ref _marker, value); }
        private CustomObjectMarker _marker = CustomObjectMarker.Undefined;

        /// <summary>
        /// Gets or sets the width of the object (in millimeters) (Y axis).
        /// </summary>
        public float WidthMm { get => _widthMm; set => SetProperty(ref _widthMm, value); }
        private float _widthMm = 0;

        /// <summary>
        /// Gets or sets the height of the object (in millimeters) (Z axis).
        /// </summary>
        public float HeightMm { get => _heightMm; set => SetProperty(ref _heightMm, value); }
        private float _heightMm = 0;

        /// <summary>
        /// Validates the custom object definition; raising an exception if invalid.
        /// </summary>
        internal override void Validate()
        {
            if (Marker == CustomObjectMarker.Undefined) throw new VectorInvalidValueException($"Custom wall marker cannot be undefined.");
            if (WidthMm <= 0) throw new VectorInvalidValueException($"Custom wall value '{nameof(WidthMm)}' must be greater than zero.");
            if (HeightMm <= 0) throw new VectorInvalidValueException($"Custom wall value '{nameof(HeightMm)}' must be greater than zero.");
        }

        /// <summary>
        /// Converts to robot CustomWallDefinition instance.
        /// </summary>
        /// <returns>Robot custom wall definition instance.</returns>
        internal ExternalInterface.CustomWallDefinition ToRobotCustomWallDefinition()
        {
            return new ExternalInterface.CustomWallDefinition()
            {
                Marker = Marker.ToRobotMarker(),
                WidthMm = WidthMm,
                HeightMm = HeightMm,
                MarkerWidthMm = MarkerWidthMm,
                MarkerHeightMm = MarkerHeightMm
            };
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"Custom Wall {CustomObjectType}";
        }
    }
}
