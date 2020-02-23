// <copyright file="ReserveBehaviorControl.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Anki.Vector.Exceptions;

namespace Anki.Vector
{
    /// <summary>
    /// A ReserveBehaviorControl object can be used to suppress the ordinary idle behaviors of the Robot and keep Vector still between SDK control instances.
    /// <para>Care must be taken when blocking background behaviors, as this may make Vector appear non-responsive.</para><para>As long as this object is not disposed, background behaviors will not activate, keeping Vector still while other SDK scripts may take control.  Highest-level behaviors like returning to
    /// the charger due to low battery will still activate.</para><para>If there is a need to keep background behaviors from activating in a single script, the class may be used to reserve behavior control while in scope.</para></summary>
    /// <seealso cref="System.IDisposable" />
    public class ReserveBehaviorControl : IDisposable
    {
        /// <summary>
        /// The robot
        /// </summary>
        private readonly Robot robot = new Robot();

        /// <summary>
        /// The robot configuration
        /// </summary>
        private readonly RobotConfiguration robotConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReserveBehaviorControl"/> class.
        /// </summary>
        /// <param name="reserveControl">if set to <c>true</c> to reserve control on construction.</param>
        /// <exception cref="VectorConfigurationException">No Robot Configuration found; please run the configuration tool to setup the robot connection.</exception>
        public ReserveBehaviorControl(bool reserveControl = true)
        {
            var configuration = RobotConfiguration.LoadDefault();
            this.robotConfiguration = configuration ?? throw new VectorConfigurationException("No Robot Configuration found; please run the configuration tool to setup the robot connection.");
            if (reserveControl) ReserveControl().Wait();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReserveBehaviorControl" /> class.
        /// </summary>
        /// <param name="robotConfiguration">The robot configuration.</param>
        /// <param name="reserveControl">if set to <c>true</c> to reserve control on construction.</param>
        /// <exception cref="VectorControlException">Unable to reserve behavior control</exception>
        public ReserveBehaviorControl(RobotConfiguration robotConfiguration, bool reserveControl = true)
        {
            this.robotConfiguration = robotConfiguration;
            if (reserveControl) ReserveControl().Wait();
        }

        /// <summary>
        /// Reserves control of Vector
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="VectorControlException">Unable to reserve behavior control</exception>
        public async Task ReserveControl()
        {
            await robot.Connect(robotConfiguration).ConfigureAwait(false);
            if (!await robot.Control.RequestControl(ControlPriority.ReserveControl).ConfigureAwait(false))
            {
                throw new VectorControlException("Unable to reserve behavior control");
            }
        }

        /// <summary>
        /// Releases control of Vector
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ReleaseControl()
        {
            await robot.Control.ReleaseControl().ConfigureAwait(false);
        }

        #region IDisposable Support

        /// <summary>
        /// To detect redundant calls
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    robot?.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
