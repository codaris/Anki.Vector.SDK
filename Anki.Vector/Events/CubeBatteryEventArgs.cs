// <copyright file="CubeBatteryEventArgs.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

namespace Anki.Vector.Events
{
    /// <summary>
    /// Cube battery level 
    /// </summary>
    public enum CubeBatteryLevel
    {
        /// <summary>Cube battery is low</summary>
        Low = 0,
        /// <summary>Cube battery is normal</summary>
        Normal = 1
    }

    /// <summary>
    /// Cube battery level event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class CubeBatteryEventArgs : RobotEventArgs
    {
        /// <summary>
        /// Gets the battery level in volts.
        /// </summary>
        public float BatteryVolts { get; }

        /// <summary>
        /// Gets the battery level.
        /// </summary>
        public CubeBatteryLevel Level { get; }

        /// <summary>
        /// Gets the factory identifier of the cube.
        /// </summary>
        public string FactoryId { get; }

        /// <summary>
        /// Gets the time since last reading in seconds.
        /// </summary>
        public float TimeSinceLastReadingSec { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CubeBatteryEventArgs" /> class.
        /// </summary>
        /// <param name="e">The event information.</param>
        internal CubeBatteryEventArgs(ExternalInterface.Event e) : base(e)
        {
            BatteryVolts = e.CubeBattery.BatteryVolts;
            Level = (CubeBatteryLevel)e.CubeBattery.Level;
            FactoryId = e.CubeBattery.FactoryId;
            TimeSinceLastReadingSec = e.CubeBattery.TimeSinceLastReadingSec;
        }
    }
}
