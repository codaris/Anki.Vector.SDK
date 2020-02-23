// <copyright file="RobotObjectConnectionStateEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.Objects;
using Anki.Vector.Types;

namespace Anki.Vector.Events
{
    /// <summary>
    /// Object connection state event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RobotObjectConnectionStateEventArgs : RobotObjectEventArgs
    {
        /// <summary>
        /// Gets a value indicating whether the object is connected.
        /// </summary>
        public bool Connected { get; }

        /// <summary>
        /// Gets the factory identifier of the object.
        /// </summary>
        public string FactoryId { get; }

        /// <summary>
        /// Gets the object identifier.
        /// </summary>
        public int ObjectId { get; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        public ObjectType ObjectType { get; }

        /// <summary>
        /// Gets the index of the custom object.
        /// </summary>
        public CustomObjectType CustomObjectType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotObjectConnectionStateEventArgs"/> class.
        /// </summary>
        /// <param name="e">The event.</param>
        internal RobotObjectConnectionStateEventArgs(ExternalInterface.Event e) : base(e)
        {
            var objectConnectionState = e.ObjectEvent.ObjectConnectionState;
            Connected = objectConnectionState.Connected;
            FactoryId = objectConnectionState.FactoryId;
            ObjectId = (int)objectConnectionState.ObjectId;
            ObjectType = objectConnectionState.ObjectType.ToSdkObjectType();
            CustomObjectType = objectConnectionState.ObjectType.ToSdkCustomObjectType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotObjectConnectionStateEventArgs"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        internal RobotObjectConnectionStateEventArgs(ExternalInterface.ConnectCubeResponse response) : base(ObjectEventType.ObjectConnectionState)
        {
            Connected = response.Success;
            FactoryId = response.FactoryId;
            ObjectId = (int)response.ObjectId;
            ObjectType = ObjectType.BlockLightcube1;
        }
    }
}
