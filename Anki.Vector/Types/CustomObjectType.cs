// <copyright file="CustomObjectType.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Custom object type.
    /// <para>Vector has 20 slots for custom object types.  You can define a <see cref="CustomBoxDefinition"/>, <see cref="CustomCubeDefinition"/>, or <see cref="CustomWallDefinition"/> 
    /// in these slots and Vector will recognize objects of those types.</para>
    /// </summary>
    public enum CustomObjectType
    {
        /// <summary>Not a custom object type.</summary>
        None = 0,
        /// <summary>Custom object type #1.</summary>
        Type1 = 1,
        /// <summary>Custom object type #2.</summary>
        Type2 = 2,
        /// <summary>Custom object type #3.</summary>
        Type3 = 3,
        /// <summary>Custom object type #4.</summary>
        Type4 = 4,
        /// <summary>Custom object type #5.</summary>
        Type5 = 5,
        /// <summary>Custom object type #6.</summary>
        Type6 = 6,
        /// <summary>Custom object type #7.</summary>
        Type7 = 7,
        /// <summary>Custom object type #8.</summary>
        Type8 = 8,
        /// <summary>Custom object type #9.</summary>
        Type9 = 9,
        /// <summary>Custom object type #10.</summary>
        Type10 = 10,
        /// <summary>Custom object type #11.</summary>
        Type11 = 11,
        /// <summary>Custom object type #12.</summary>
        Type12 = 12,
        /// <summary>Custom object type #13.</summary>
        Type13 = 13,
        /// <summary>Custom object type #14.</summary>
        Type14 = 14,
        /// <summary>Custom object type #15.</summary>
        Type15 = 15,
        /// <summary>Custom object type #16.</summary>
        Type16 = 16,
        /// <summary>Custom object type #17.</summary>
        Type17 = 17,
        /// <summary>Custom object type #18.</summary>
        Type18 = 18,
        /// <summary>Custom object type #19.</summary>
        Type19 = 19,
        /// <summary>Custom object type #20.</summary>
        Type20 = 20
    }

    /// <summary>
    /// Convert custom object type to robot type
    /// </summary>
    internal static class CustomObjectTypeExtensions
    {
        /// <summary>
        /// Converts to robot type.
        /// </summary>
        /// <param name="customObjectType">Type of the custom object.</param>
        /// <returns>Robot custom object type</returns>
        internal static CustomType ToRobotType(this CustomObjectType customObjectType)
        {
            return (CustomType)customObjectType;
        }
    }
}
