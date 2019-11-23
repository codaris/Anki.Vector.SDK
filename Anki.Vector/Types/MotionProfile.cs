// <copyright file="MotionProfile.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using Anki.Vector.ExternalInterface;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Tells Vector how to drive when receiving navigation and movement actions such as GoToPose and DockWithCube.
    /// </summary>
    public class MotionProfile
    {
#pragma warning disable SA1600
        private const float DefaultAccelMmps2 = 200f;
        private const float DefaultDecelMmps2 = 500f;
        private const float DefaultDockAccelMmps2 = 200f;
        private const float DefaultDockDecelMmps2 = 500f;
        private const float DefaultDockSpeedMmps = 60f;
        private const float DefaultPointTurnAccelRadPerSec2 = 10f;
        private const float DefaultPointTurnDecelRadPerSec2 = 10f;
        private const float DefaultPointTurnSpeedRadPerSec = 2f;
        private const float DefaultReverseSpeedMmps = 80f;
        private const float DefaultSpeedMmps = 100f;
#pragma warning restore SA1600

        /// <summary>
        /// Gets or sets the acceleration in mm/s2
        /// </summary>
        public float AccelMmps2 { get; set; } = DefaultAccelMmps2;

        /// <summary>
        /// Gets or sets the deceleration in mm/s2
        /// </summary>
        public float DecelMmps2 { get; set; } = DefaultDecelMmps2;

        /// <summary>
        /// Gets or sets the docking acceleration mm/s2.
        /// </summary>
        public float DockAccelMmps2 { get; set; } = DefaultDockAccelMmps2;

        /// <summary>
        /// Gets or sets the docking deceleration in mm/s2
        /// </summary>
        public float DockDecelMmps2 { get; set; } = DefaultDockDecelMmps2;

        /// <summary>
        /// Gets or sets the docking speed in mm/s2
        /// </summary>
        public float DockSpeedMmps { get; set; } = DefaultDockSpeedMmps;

        /// <summary>
        /// Gets or sets the point turn acceleration in radians/s2
        /// </summary>
        public float PointTurnAccelRadPerSec2 { get; set; } = DefaultPointTurnAccelRadPerSec2;

        /// <summary>
        /// Gets or sets the point turn deceleration in radians/s2
        /// </summary>
        public float PointTurnDecelRadPerSec2 { get; set; } = DefaultPointTurnDecelRadPerSec2;

        /// <summary>
        /// Gets or sets the point turn speed in radians/s
        /// </summary>
        public float PointTurnSpeedRadPerSec { get; set; } = DefaultPointTurnSpeedRadPerSec;

        /// <summary>
        /// Gets or sets the reverse speed mm/s
        /// </summary>
        public float ReverseSpeedMmps { get; set; } = DefaultReverseSpeedMmps;

        /// <summary>
        /// Gets or sets the forward speed in mm/s
        /// </summary>
        public float SpeedMmps { get; set; } = DefaultSpeedMmps;

        /// <summary>
        /// Gets a value indicating whether this motion profile has been customized
        /// </summary>
        public bool IsCustom
        {
            get
            {
                if (AccelMmps2 != DefaultAccelMmps2) return true;
                if (DecelMmps2 != DefaultDecelMmps2) return true;
                if (DockAccelMmps2 != DefaultDockAccelMmps2) return true;
                if (DockDecelMmps2 != DefaultDockDecelMmps2) return true;
                if (DockSpeedMmps != DefaultDockSpeedMmps) return true;
                if (PointTurnAccelRadPerSec2 != DefaultPointTurnAccelRadPerSec2) return true;
                if (PointTurnDecelRadPerSec2 != DefaultPointTurnDecelRadPerSec2) return true;
                if (PointTurnSpeedRadPerSec != DefaultPointTurnSpeedRadPerSec) return true;
                if (ReverseSpeedMmps != DefaultReverseSpeedMmps) return true;
                if (SpeedMmps != DefaultSpeedMmps) return true;
                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MotionProfile"/> class.
        /// </summary>
        internal MotionProfile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MotionProfile"/> class.
        /// </summary>
        /// <param name="motionProfile">The motion profile.</param>
        internal MotionProfile(PathMotionProfile motionProfile)
        {
            AccelMmps2 = motionProfile.AccelMmps2;
            DecelMmps2 = motionProfile.DecelMmps2;
            DockAccelMmps2 = motionProfile.DockAccelMmps2;
            DockDecelMmps2 = motionProfile.DockDecelMmps2;
            DockSpeedMmps = motionProfile.DockSpeedMmps;
            PointTurnAccelRadPerSec2 = motionProfile.PointTurnAccelRadPerSec2;
            PointTurnDecelRadPerSec2 = motionProfile.PointTurnDecelRadPerSec2;
            PointTurnSpeedRadPerSec = motionProfile.PointTurnSpeedRadPerSec;
            ReverseSpeedMmps = motionProfile.ReverseSpeedMmps;
            SpeedMmps = motionProfile.SpeedMmps;
        }

        /// <summary>
        /// Sets motion profile back to the default values
        /// </summary>
        public void SetDefault()
        {
            AccelMmps2 = DefaultAccelMmps2;
            DecelMmps2 = DefaultDecelMmps2;
            DockAccelMmps2 = DefaultDockAccelMmps2;
            DockDecelMmps2 = DefaultDockDecelMmps2;
            DockSpeedMmps = DefaultDockSpeedMmps;
            PointTurnAccelRadPerSec2 = DefaultPointTurnAccelRadPerSec2;
            PointTurnDecelRadPerSec2 = DefaultPointTurnDecelRadPerSec2;
            PointTurnSpeedRadPerSec = DefaultPointTurnSpeedRadPerSec;
            ReverseSpeedMmps = DefaultReverseSpeedMmps;
            SpeedMmps = DefaultSpeedMmps;
        }

        /// <summary>
        /// To the robot path motion profile type.
        /// </summary>
        /// <returns>Robot PathMotionProfile</returns>
        internal PathMotionProfile ToPathMotionProfile()
        {
            return new PathMotionProfile()
            {
                AccelMmps2 = this.AccelMmps2,
                DecelMmps2 = this.DecelMmps2,
                DockAccelMmps2 = this.DockAccelMmps2,
                DockDecelMmps2 = this.DockDecelMmps2,
                DockSpeedMmps = this.DockSpeedMmps,
                PointTurnAccelRadPerSec2 = this.PointTurnAccelRadPerSec2,
                PointTurnDecelRadPerSec2 = this.PointTurnDecelRadPerSec2,
                PointTurnSpeedRadPerSec = this.PointTurnSpeedRadPerSec,
                ReverseSpeedMmps = this.ReverseSpeedMmps,
                SpeedMmps = this.SpeedMmps,
                IsCustom = this.IsCustom
            };
        }
    }
}
