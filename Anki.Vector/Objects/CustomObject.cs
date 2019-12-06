// <copyright file="CustomObject.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Objects
{
    /// <summary>
    /// An object defined by the SDK observed by the robot.  The object will reference a <see cref="CustomObjectArchetype"/>, with additional instance data.
    /// </summary>
    /// <seealso cref="Anki.Vector.Objects.ObservableObject" />
    public class CustomObject : ObjectWithId
    {
        /// <summary>
        /// Gets the archetype defining this custom object's properties.
        /// </summary>
        public CustomObjectArchetype Archetype { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomObject" /> class.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="archetype">The archetype defining this custom object's properties.</param>
        internal CustomObject(int objectId, CustomObjectArchetype archetype) : base(objectId)
        {
            Archetype = archetype;
        }

        /// <summary>
        /// Gets the name of the object type.
        /// </summary>
        public override string ObjectTypeName => Archetype.ToString();
    }
}
