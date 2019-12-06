// <copyright file="Charger.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Objects
{
    /// <summary>
    /// Vector’s charger object, which the robot can observe and drive toward. We get an <see cref="WorldComponent.ObjectObserved"/> message when 
    /// the robot sees the charger.
    /// </summary>
    /// <seealso cref="Anki.Vector.Objects.ObservableObject" />
    public class Charger : ObjectWithId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Charger" /> class.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        internal Charger(int objectId) : base(objectId)
        {
        }

        /// <summary>
        /// Gets the name of the object type.
        /// </summary>
        public override string ObjectTypeName => "Charger";
    }
}
