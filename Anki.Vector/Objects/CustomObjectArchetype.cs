// <copyright file="CustomObjectArchetype.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.Exceptions;
using Anki.Vector.Types;

namespace Anki.Vector.Objects
{
    /// <summary>
    /// Abstract base class for all custom object definition types
    /// </summary>
    public abstract class CustomObjectArchetype
    {
        /// <summary>
        /// Gets the type of the custom object.
        /// </summary>
        public CustomObjectType CustomObjectType { get; private set; } = CustomObjectType.None;

        /// <summary>
        /// Gets or sets the width of the printed marker (in millimeters).
        /// </summary>
        public float MarkerWidthMm { get => _markerWidthMm; set => SetProperty(ref _markerWidthMm, value); }
        private float _markerWidthMm = 0;

        /// <summary>
        /// Gets or sets the height of the printed marker (in millimeters).
        /// </summary>
        public float MarkerHeightMm { get => _markerHeightMm; set => SetProperty(ref _markerHeightMm, value); }
        private float _markerHeightMm = 0;

        /// <summary>
        /// Binds this definition to the specified custom object type.
        /// </summary>
        /// <param name="customObjectType">Type of the custom object.</param>
        internal void Bind(CustomObjectType customObjectType)
        {
            Validate();
            if (MarkerWidthMm <= 0) throw new VectorInvalidValueException($"'{nameof(MarkerWidthMm)}' must be greater than zero.");
            if (MarkerHeightMm <= 0) throw new VectorInvalidValueException($"'{nameof(MarkerHeightMm)}' must be greater than zero.");
            CustomObjectType = customObjectType;
        }

        /// <summary>
        /// Detaches this instance.
        /// </summary>
        internal void Unbind()
        {
            CustomObjectType = CustomObjectType.None;
        }

        /// <summary>
        /// Validates the custom object definition; raising an exception if invalid.
        /// </summary>
        internal abstract void Validate();

        /// <summary>
        /// Sets the custom object property; preventing changes to attached object definitions.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="InvalidOperationException">Cannot change the definition of defined custom objects.</exception>
        internal void SetProperty<T>(ref T field, T value)
        {
            if (CustomObjectType != CustomObjectType.None)
            {
                throw new InvalidOperationException("Bound custom object definitions cannot be changed.");
            }
            field = value;
        }
    }
}
