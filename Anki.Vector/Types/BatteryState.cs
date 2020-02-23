// <copyright file="BatteryState.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Battery level
    /// </summary>
    public enum BatteryLevel
    {
        /// <summary>The level is unknown</summary>
        Unknown = 0,
        /// <summary>3.6V or less. If on charger, 4V or less.</summary>
        Low = 1,
        /// <summary>Normal operating levels.</summary>
        Nominal = 2,
        /// <summary>This state can only be achieved when Vector is on the charger</summary>
        Full = 3
    }

    /// <summary>
    /// The state of the robot battery.
    /// </summary>
    public class BatteryState
    {
        /// <summary>
        /// Gets the battery level.
        /// </summary>
        public BatteryLevel BatteryLevel { get; }

        /// <summary>
        /// Gets the battery volts.
        /// </summary>
        public float BatteryVolts { get; }

        /// <summary>
        /// Gets a value indicating whether the robot is charging.
        /// </summary>
        public bool IsCharging { get; }

        /// <summary>
        /// Gets a value indicating whether the robot is on charger platform.
        /// </summary>
        public bool IsOnChargerPlatform { get; }

        /// <summary>
        /// Gets the suggested charger time in seconds.
        /// </summary>
        public float SuggestedChargerSec { get; }

        /// <summary>
        /// Gets the cube battery state.
        /// </summary>
        public CubeBatteryState CubeBattery { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatteryState"/> class.
        /// </summary>
        /// <param name="batteryStateResponse">The battery state response.</param>
        internal BatteryState(BatteryStateResponse batteryStateResponse)
        {
            BatteryLevel = (BatteryLevel)batteryStateResponse.BatteryLevel;
            BatteryVolts = batteryStateResponse.BatteryVolts;
            IsCharging = batteryStateResponse.IsCharging;
            IsOnChargerPlatform = batteryStateResponse.IsOnChargerPlatform;
            SuggestedChargerSec = batteryStateResponse.SuggestedChargerSec;

            if (batteryStateResponse.CubeBattery != null)
            {
                CubeBattery = new CubeBatteryState(batteryStateResponse.CubeBattery);
            }
        }
    }
}
