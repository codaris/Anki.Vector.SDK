// <copyright file="Extensions.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using Anki.Vector.Types;

namespace Anki.Vector.Objects
{
    /// <summary>
    /// Object extension methods
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Converts the enumeration.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>The library type</returns>
        internal static ObjectType ToSdkObjectType(this ExternalInterface.ObjectType objectType)
        {
            switch (objectType)
            {
                case ExternalInterface.ObjectType.InvalidObject:
                    return ObjectType.InvalidObject;
                case ExternalInterface.ObjectType.UnknownObject:
                    return ObjectType.UnknownObject;
                case ExternalInterface.ObjectType.BlockLightcube1:
                    return ObjectType.BlockLightcube1;
                case ExternalInterface.ObjectType.ChargerBasic:
                    return ObjectType.ChargerBasic;
                default:
                    if ((int)objectType >= (int)ExternalInterface.ObjectType.FirstCustomObjectType) return ObjectType.CustomObject;
                    throw new InvalidOperationException($"Unknown object type {objectType}");
            }
        }

        /// <summary>
        /// Gets the type of the custom object from the objectType parameter
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>SDK Custom object type value</returns>
        internal static CustomObjectType ToSdkCustomObjectType(this ExternalInterface.ObjectType objectType)
        {
            if ((int)objectType < (int)ExternalInterface.ObjectType.FirstCustomObjectType) return CustomObjectType.None;
            return (CustomObjectType)((int)objectType - (int)ExternalInterface.ObjectType.FirstCustomObjectType + 1);
        }
    }
}
