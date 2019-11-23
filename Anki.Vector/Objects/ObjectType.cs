// <copyright file="ObjectType.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Objects
{
    /// <summary>
    /// The type of the object
    /// </summary>
    public enum ObjectType
    {
        /// <summary>Invalid object</summary>
        InvalidObject = 0,
        /// <summary>Unknown object</summary>
        UnknownObject = 1,
        /// <summary>Light cube</summary>
        BlockLightcube1 = 2,
        /// <summary>Custom object</summary>
        CustomObject = 3,
        /// <summary>Charger</summary>
        ChargerBasic = 6
    }
}
