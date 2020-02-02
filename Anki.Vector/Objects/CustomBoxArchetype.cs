// <copyright file="CustomBoxArchetype.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Anki.Vector.Exceptions;
using Anki.Vector.Types;

namespace Anki.Vector.Objects
{
    /// <summary>
    /// Custom object cuboid definition.
    /// </summary>
    /// <seealso cref="Anki.Vector.Objects.CustomObjectArchetype" />
    public class CustomBoxArchetype : CustomObjectArchetype
    {
        /// <summary>
        /// Gets or sets the marker affixed to the front of the object.
        /// </summary>
        public CustomObjectMarker MarkerFront { get => _markerFront; set => SetProperty(ref _markerFront, value); }
        private CustomObjectMarker _markerFront = CustomObjectMarker.Undefined;

        /// <summary>
        /// Gets or sets the marker affixed to the back of the object.
        /// </summary>
        public CustomObjectMarker MarkerBack { get => _markerBack; set => SetProperty(ref _markerBack, value); }
        private CustomObjectMarker _markerBack = CustomObjectMarker.Undefined;

        /// <summary>
        /// Gets or sets the marker affixed to the top of the object.
        /// </summary>
        public CustomObjectMarker MarkerTop { get => _markerTop; set => SetProperty(ref _markerTop, value); }
        private CustomObjectMarker _markerTop = CustomObjectMarker.Undefined;

        /// <summary>
        /// Gets or sets the marker affixed to the bottom of the object.
        /// </summary>
        public CustomObjectMarker MarkerBottom { get => _markerBottom; set => SetProperty(ref _markerBottom, value); }
        private CustomObjectMarker _markerBottom = CustomObjectMarker.Undefined;

        /// <summary>
        /// Gets or sets the marker affixed to the left of the object.
        /// </summary>
        public CustomObjectMarker MarkerLeft { get => _markerLeft; set => SetProperty(ref _markerLeft, value); }
        private CustomObjectMarker _markerLeft = CustomObjectMarker.Undefined;

        /// <summary>
        /// Gets or sets the marker affixed to the right of the object.
        /// </summary>
        public CustomObjectMarker MarkerRight { get => _markerRight; set => SetProperty(ref _markerRight, value); }
        private CustomObjectMarker _markerRight = CustomObjectMarker.Undefined;

        /// <summary>
        /// Gets or sets the depth of the object (in millimeters) (X axis).
        /// </summary>
        public float DepthMm { get => _depthMm; set => SetProperty(ref _depthMm, value); }
        private float _depthMm = 0;

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
            if (MarkerFront == CustomObjectMarker.Undefined) throw new VectorInvalidValueException($"Custom object marker '{nameof(MarkerFront)}' cannot be undefined.");
            if (MarkerBack == CustomObjectMarker.Undefined) throw new VectorInvalidValueException($"Custom object marker '{nameof(MarkerBack)}' cannot be undefined.");
            if (MarkerTop == CustomObjectMarker.Undefined) throw new VectorInvalidValueException($"Custom object marker '{nameof(MarkerTop)}' cannot be undefined.");
            if (MarkerBottom == CustomObjectMarker.Undefined) throw new VectorInvalidValueException($"Custom object marker '{nameof(MarkerBottom)}' cannot be undefined.");
            if (MarkerLeft == CustomObjectMarker.Undefined) throw new VectorInvalidValueException($"Custom object marker '{nameof(MarkerLeft)}' cannot be undefined.");
            if (MarkerRight == CustomObjectMarker.Undefined) throw new VectorInvalidValueException($"Custom object marker '{nameof(MarkerRight)}' cannot be undefined.");

            var markerSet = new HashSet<CustomObjectMarker>() { MarkerFront, MarkerBack, MarkerTop, MarkerBottom, MarkerLeft, MarkerRight };
            if (markerSet.Count() != 6) throw new VectorInvalidValueException("All custom object markers must be unique for a custom box");

            if (DepthMm <= 0) throw new VectorInvalidValueException($"Custom box value '{nameof(DepthMm)}' must be greater than zero.");
            if (WidthMm <= 0) throw new VectorInvalidValueException($"Custom box value '{nameof(WidthMm)}' must be greater than zero.");
            if (HeightMm <= 0) throw new VectorInvalidValueException($"Custom box value '{nameof(HeightMm)}' must be greater than zero.");
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"Custom Box {CustomObjectType}";
        }

        /// <summary>
        /// Converts to robot CustomBoxDefinition instance.
        /// </summary>
        /// <returns>Robot custom box definition.</returns>
        internal ExternalInterface.CustomBoxDefinition ToRobotCustomBoxDefinition()
        {
            return new ExternalInterface.CustomBoxDefinition()
            {
                MarkerFront = MarkerFront.ToRobotMarker(),
                MarkerBack = MarkerBack.ToRobotMarker(),
                MarkerTop = MarkerTop.ToRobotMarker(),
                MarkerBottom = MarkerBottom.ToRobotMarker(),
                MarkerLeft = MarkerLeft.ToRobotMarker(),
                MarkerRight = MarkerRight.ToRobotMarker(),
                XSizeMm = DepthMm,
                YSizeMm = WidthMm,
                ZSizeMm = HeightMm,
                MarkerWidthMm = MarkerWidthMm,
                MarkerHeightMm = MarkerHeightMm
            };
        }
    }
}
