// <copyright file="CubeBatteryState.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Cube battery level
    /// </summary>
    public enum CubeBatteryLevel
    {
        /// <summary>1.1V or less.</summary>
        Low = 0,
        /// <summary>Normal operating levels.</summary>
        Normal = 1
    }

    /// <summary>
    ///  The state of the cube battery.
    /// </summary>
    public class CubeBatteryState
    {
        /// <summary>
        /// Gets the battery level.
        /// </summary>
        public CubeBatteryLevel BatteryLevel { get; }

        /// <summary>
        /// Gets the battery volts.
        /// </summary>
        public float BatteryVolts { get; }

        /// <summary>
        /// Gets the factory identifier of the cube.
        /// </summary>
        public string FactoryId { get; }

        /// <summary>
        /// Gets the time since last reading in seconds.
        /// </summary>
        public float TimeSinceLastReadingSec { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CubeBatteryState"/> class.
        /// </summary>
        /// <param name="cubeBattery">The cube battery.</param>
        internal CubeBatteryState(CubeBattery cubeBattery)
        {
            BatteryVolts = cubeBattery.BatteryVolts;
            FactoryId = cubeBattery.FactoryId;
            BatteryLevel = (CubeBatteryLevel)cubeBattery.Level;
            TimeSinceLastReadingSec = cubeBattery.TimeSinceLastReadingSec;
        }
    }
}
