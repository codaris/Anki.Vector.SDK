// <copyright file="Robot.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Anki.Vector.Events;
using Anki.Vector.Exceptions;
using Anki.Vector.ExternalInterface;
using Anki.Vector.Types;
using Grpc.Core;
using Newtonsoft.Json;
using static Anki.Vector.ExternalInterface.ExternalInterface;

namespace Anki.Vector
{
    /// <summary>
    /// Vector control priority level
    /// </summary>
    public enum ControlPriority
    {
        /// <summary>
        /// Highest priority level. Suppresses most automatic physical reactions, use with caution. 
        /// </summary>
        OverrideBehaviors = 10,

        /// <summary>
        /// Normal priority level. Directly under mandatory physical reactions.
        /// </summary>
        Default = 20,

        /// <summary>
        /// Enable long-running SDK control between script execution.  Not to be used for regular behavior control.
        /// </summary>
        ReserveControl = 30
    }

    /// <summary>
    /// The main robot class for managing Vector.
    /// <para>The Robot object is responsible for managing the state and connections to a Vector, and is typically the entry-point to running the SDK.</para>
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class Robot : RobotObject, IDisposable
    {
        /// <summary>
        /// Occurs when there is a background exception.
        /// </summary>
        public event EventHandler<ErrorEventArgs> Error;

        /// <summary>
        /// Occurs when the robot is fully connected.
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;

        /// <summary>
        /// Occurs when robot is disconnected.
        /// </summary>
        public event EventHandler<DisconnectedEventArgs> Disconnected;

        /// <summary>
        /// Occurs when robot is delocalized  (i.e. whenever Vector no longer knows where he is - e.g.when he's picked up).
        /// </summary>
        public event EventHandler<DelocalizedEventArgs> Delocalized;

        /// <summary>
        /// The GRPC channel
        /// </summary>
        private Channel channel = null;

        /// <summary>
        /// Gets the control component
        /// </summary>
        public ControlComponent Control { get; }

        /// <summary>
        /// Gets the camera component
        /// </summary>
        public CameraComponent Camera { get; }

        /// <summary>
        /// Gets the behavior component
        /// </summary>
        public BehaviorComponent Behavior { get; }

        /// <summary>
        /// Gets the motors component
        /// </summary>
        public MotorComponent Motors { get; }

        /// <summary>
        /// Gets the photo component.
        /// </summary>
        public PhotoComponent Photos { get; }

        /// <summary>
        /// Gets the events component 
        /// </summary>
        public EventComponent Events { get; }

        /// <summary>
        /// Gets the animation component.
        /// </summary>
        public AnimationComponent Animation { get; }

        /// <summary>
        /// Gets the face component
        /// </summary>
        public FaceComponent Faces { get; }

        /// <summary>
        /// Gets the audio component.
        /// </summary>
        public AudioComponent Audio { get; }

        /// <summary>
        /// Gets the screen component.
        /// </summary>
        public ScreenComponent Screen { get; }

        /// <summary>
        /// Gets the vision component.
        /// </summary>
        public VisionComponent Vision { get; }

        /// <summary>
        /// Gets the world component.
        /// </summary>
        public WorldComponent World { get; }

        /// <summary>
        /// Gets the navmap component.
        /// </summary>
        public NavMapComponent NavMap { get; }

        /// <summary>
        /// The HTTP client for REST calls to Vector
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA2213", Justification = "HTTP Client is disposed in Disconnect method.")]
        private HttpClient httpClient = null;

        /// <summary>
        /// The GRPC client
        /// </summary>
        private ExternalInterfaceClient grpcClient = null;

        /// <summary>
        /// Gets a value indicating whether the robot is connected.
        /// </summary>
        public bool IsConnected => Events.IsProccessingEvents;

        /// <summary>
        /// Gets the current accelerometer reading
        /// </summary>
        public Acceleration Acceleration { get => _acceleration; private set => SetProperty(ref _acceleration, value); }
        private Acceleration _acceleration;

        /// <summary>
        /// Gets the ID of the object currently being carried (-1 if none)
        /// </summary>
        public int CarryingObjectId { get => _carryingObjectId; private set => SetProperty(ref _carryingObjectId, value); }
        private int _carryingObjectId = -1;

        /// <summary>
        /// Gets the current gyroscope reading (x, y, z) 
        /// </summary>
        public AngularVelocity Gyro { get => _gyro; private set => SetProperty(ref _gyro, value); }
        private AngularVelocity _gyro;

        /// <summary>
        /// Gets Vector's head angle (up/down) in radians
        /// </summary>
        public float HeadAngleRad { get => _headAngleRad; private set => SetProperty(ref _headAngleRad, value); }
        private float _headAngleRad;

        /// <summary>
        /// Gets the ID of the object the head is tracking to (-1 if none)
        /// </summary>
        public int HeadTrackingObjectId { get => _headTrackingObjectId; private set => SetProperty(ref _headTrackingObjectId, value); }
        private int _headTrackingObjectId = -1;

        /// <summary>
        /// Gets the robot's timestamp for the last image seen.
        /// </summary>
        public uint LastImageTimestamp { get => _lastImageTimestamp; private set => SetProperty(ref _lastImageTimestamp, value); }
        private uint _lastImageTimestamp;

        /// <summary>
        /// Gets Vector’s left wheel speed in mm/sec
        /// </summary>
        public float LeftWheelSpeedMmps { get => _leftWheelSpeedMmps; private set => SetProperty(ref _leftWheelSpeedMmps, value); }
        private float _leftWheelSpeedMmps;

        /// <summary>
        /// Gets the height of Vector’s lift from the ground.
        /// </summary>
        public float LiftHeightMm { get => _liftHeightMm; private set => SetProperty(ref _liftHeightMm, value); }
        private float _liftHeightMm;

        /// <summary>
        /// Gets the ID of the object that the robot is localized to (-1 if none)
        /// </summary>
        public int LocalizedToObjectId { get => _localizedToObjectId; private set => SetProperty(ref _localizedToObjectId, value); }
        private int _localizedToObjectId = -1;

        /// <summary>
        /// Gets the current pose (position and orientation) of Vector.
        /// </summary>
        public Pose Pose { get => _pose; private set => SetProperty(ref _pose, value); }
        private Pose _pose = new Pose();

        /// <summary>
        /// Gets Vector's pose angle (heading in X-Y plane) in radians
        /// </summary>
        public float PoseAngleRad { get => _poseAngleRad; private set => SetProperty(ref _poseAngleRad, value); }
        private float _poseAngleRad;

        /// <summary>
        /// Gets Vector’s pose pitch (angle up/down) in radians
        /// </summary>
        public float PosePitchRad { get => _posePitchRad; private set => SetProperty(ref _posePitchRad, value); }
        private float _posePitchRad;

        /// <summary>
        /// Gets the proximity sensor data.
        /// </summary>
        public ProximitySensorData Proximity { get => _proximity; private set => SetProperty(ref _proximity, value); }
        private ProximitySensorData _proximity;

        /// <summary>
        /// Gets Vector's right wheel speed in mm/sec
        /// </summary>
        public float RightWheelSpeedMmps { get => _rightWheelSpeedMmps; private set => SetProperty(ref _rightWheelSpeedMmps, value); }
        private float _rightWheelSpeedMmps;

        /// <summary>
        /// Gets the various status properties of the robot.
        /// </summary>
        public RobotStatus Status { get => _status; private set => SetProperty(ref _status, value); }
        private RobotStatus _status;

        /// <summary>
        /// Gets state related to touch detection.
        /// </summary>
        public TouchSensorData Touch { get => _touch; private set => SetProperty(ref _touch, value); }
        private TouchSensorData _touch;

        /// <summary>
        /// Gets the current IP address of the robot.  Can be null if remote Vector connection is used.
        /// </summary>
        public IPAddress IPAddress { get => _ipAddress; private set => SetProperty(ref _ipAddress, value); }
        private IPAddress _ipAddress;
        
        /// <summary>
        /// The timeout for calls and connections in milliseconds
        /// </summary>
        internal const int DefaultConnectionTimeout = 5_000;

        /// <summary>
        /// Has the disconnect method been called
        /// </summary>
        private volatile bool disconnecting = false;

        /// <summary>
        /// Propagates the exception by raising the error event.
        /// </summary>
        /// <param name="ex">The ex.</param>
        internal void PropagateException(Exception ex)
        {
            Error?.Invoke(this, new ErrorEventArgs(ex));
        }

        /// <summary>
        /// Connects to Vector and returns a robot instance.
        /// </summary>
        /// <param name="robotConfiguration">The robot configuration.</param>
        /// <param name="timeout">The connection timeout in milliseconds.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the connected robot instance</returns>
        public static async Task<Robot> NewConnection(IRobotConfiguration robotConfiguration, int timeout = DefaultConnectionTimeout)
        {
            // Create a robot proxy object used to communicate with Vector
            var robot = new Robot();
            try
            {
                // Attempt to connect the robot
                await robot.Connect(robotConfiguration, timeout).ConfigureAwait(false);
            }
            catch (Exception)
            {
                // There was a problem connecting with the robot, clean up the connection
                robot.Dispose();
                throw;
            }
            return robot;
        }


        /// <summary>
        /// Connects to Vector using the first robot found in the configuration file and returns a robot instance.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the connected robot instance.</returns>
        /// <exception cref="VectorConfigurationException">No Robot Configuration found; please run the configuration tool to setup the robot connection.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Ignore VectorNotFoundException until all Vectors have been tested.")]
        public static async Task<Robot> NewConnection(int timeout = DefaultConnectionTimeout)
        {
            // Load the configuration file
            var configurations = RobotConfiguration.Load().ToArray();
            if (configurations.Length == 0)
            {
                throw new VectorConfigurationException("No Robot Configuration found; please run the configuration tool to setup the robot connection.");
            }
            
            // try each robot in order until the one is able to connect
            foreach (var configuration in configurations)
            {
                try
                {
                    // Try connecting to the robot.  Wait for it connect so that we can capture any
                    // failed connection exceptions.  If we succeed, we'll return the connection;
                    // if get an exception, we'll try the next configuration
                    return await NewConnection(configuration, timeout).ConfigureAwait(false);
                }
                catch (VectorNotFoundException)
                {
                    // If only one robot, rethrow the original exception
                    if (configurations.Length == 1) throw;
                }
            }

            // More than one robot, throw exception if none found
            throw new VectorNotFoundException("Could not connect to any of the configured Vectors.");
        }

        /// <summary>
        /// Connects to Vector using the robot configuration that matches the name or serial number provided and returns a robot instance.
        /// </summary>
        /// <param name="nameOrSerialNumber">The name or serial number.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the connected robot instance.</returns>
        /// <exception cref="VectorConfigurationException">No Robot Configuration with the name or serial number matching '{nameOrSerialNumber}' found.</exception>
        public static Task<Robot> NewConnection(string nameOrSerialNumber, int timeout = DefaultConnectionTimeout)
        {
            var configuration = RobotConfiguration.Load().FirstOrDefault(c => c.RobotName == nameOrSerialNumber || c.SerialNumber == nameOrSerialNumber);
            if (configuration == null)
            {
                throw new VectorConfigurationException($"No Robot Configuration with the name or serial number matching '{nameOrSerialNumber}' found.");
            }
            return NewConnection(configuration, timeout);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Robot"/> class.
        /// </summary>
        public Robot() 
        {
            // Components
            this.Control = new ControlComponent(this);
            this.Camera = new CameraComponent(this);
            this.Behavior = new BehaviorComponent(this);
            this.Motors = new MotorComponent(this);
            this.Events = new EventComponent(this);
            this.Animation = new AnimationComponent(this);
            this.Audio = new AudioComponent(this);
            this.Screen = new ScreenComponent(this);
            this.Vision = new VisionComponent(this);
            this.World = new WorldComponent(this);
            this.Faces = new FaceComponent(this);
            this.Photos = new PhotoComponent(this);
            this.NavMap = new NavMapComponent(this);

            // Handle the robot state event
            this.Events.RobotState += Events_RobotState;

            // Update the connected state if processing event changes
            this.Events.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Events.IsProccessingEvents))
                {
                    OnPropertyChanged(nameof(IsConnected));
                    if (IsConnected) Connected?.Invoke(this, new ConnectedEventArgs());
                }
            };
        }

        /// <summary>
        /// Handles the RobotState event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RobotStateEventArgs"/> instance containing the event data.</param>
        private void Events_RobotState(object sender, RobotStateEventArgs e)
        {
            // Vector is delocalized if his pose is no longer comparable.
            bool delocalized = !Pose.IsComparable(e.Pose);

            Acceleration = e.Acceleration;
            CarryingObjectId = e.CarryingObjectId;
            Gyro = e.Gyro;
            HeadAngleRad = e.HeadAngleRad;
            HeadTrackingObjectId = e.HeadTrackingObjectId;
            LastImageTimestamp = e.LastImageTimestamp;
            LeftWheelSpeedMmps = e.LeftWheelSpeedMmps;
            LiftHeightMm = e.LiftHeightMm;
            LocalizedToObjectId = e.LocalizedToObjectId;
            Pose = e.Pose;
            PoseAngleRad = e.PoseAngleRad;
            PosePitchRad = e.PosePitchRad;
            Proximity = e.Proximity;
            RightWheelSpeedMmps = e.RightWheelSpeedMmps;
            Status = e.Status;
            Touch = e.Touch;

            // Trigger the delocalized event
            if (delocalized) Delocalized?.Invoke(this, new DelocalizedEventArgs());
        }


        /// <summary>
        /// Connect to Vector on the local network.  This will attempt to connect using the configured IP address (if provided) otherwise it will
        /// trigger an mDNS query to find Vector's IP address.  For connections to Vector over the Internet, use the <see cref="RemoteConnect(IRemoteRobotConfiguration, int)"/> method instead.
        /// </summary>
        /// <param name="robotConfiguration">The robot configuration.</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <param name="useZeroConfig">if set to <c>true</c> use zero configuration (mDNS).</param>
        /// <exception cref="ArgumentNullException">robotConfiguration</exception>
        /// <exception cref="VectorNotFoundException">Unable to establish a connection to Vector.</exception>
        /// <exception cref="VectorInvalidVersionException">Your SDK version is not compatible with Vector’s version.</exception>
        public async Task Connect(IRobotConfiguration robotConfiguration, int timeout = DefaultConnectionTimeout, bool useZeroConfig = true)
        {
            if (robotConfiguration == null) throw new ArgumentNullException(nameof(robotConfiguration));

            // If already connected, do nothing.
            if (IsConnected) return;

            // Reset disconnecting flag
            disconnecting = false;

            // Create the channel credentials
            var channelCredentials = CreateChannelCredentials(robotConfiguration.Certificate, robotConfiguration.Guid);
            // The channel to connect with
            Channel channel = null;
            // The connection task running channel.ConnectAsync()
            Task connectionTask = null;
            // The robot IP address            
            var ipAddress = robotConfiguration.IPAddress;

            // If using mDNS then lookup IP address 
            if (useZeroConfig)
            {
                // If an IP address is specified, try and connect to that IP while looking up additional IP
                if (ipAddress != null)
                {
                    // Create connection to default IP address
                    channel = CreateChannel(channelCredentials, ipAddress.ToString(), robotConfiguration.RobotName);
                    connectionTask = channel.ConnectAsync(GrpcDeadline(timeout));

                    // Search for updated IP address
                    var ipAddressTask = robotConfiguration.FindRobotAddress();

                    // Run both connection and IP address lookup at the same time
                    var completedTask = await Task.WhenAny(connectionTask, ipAddressTask).ConfigureAwait(false);

                    if (completedTask == ipAddressTask && !ipAddressTask.IsFaulted && ipAddressTask.Result != ipAddress)
                    {
                        // If IP address lookup completed first and IP doesn't match then try again below with new IP
                        ipAddress = ipAddressTask.Result;
                        connectionTask = null;
                    }
                    else if (completedTask == connectionTask && connectionTask.IsFaulted)
                    {
                        // If connection is faulted then wait for IP address and try again below
                        ipAddress = await ipAddressTask.ConfigureAwait(false);
                        connectionTask = null;
                    }
                }
                else
                {
                    // If no IP address specified look it up
                    ipAddress = await robotConfiguration.FindRobotAddress().ConfigureAwait(false);
                    connectionTask = null;
                }
            }

            // If cannot find IP address then error out
            if (ipAddress == null)
            {
                throw new VectorNotFoundException("Could not find Vector and no IP address specified.");
            }

            // If connection task not created, create one here.
            if (connectionTask == null)
            {
                channel = CreateChannel(channelCredentials, ipAddress.ToString(), robotConfiguration.RobotName);
                connectionTask = channel.ConnectAsync(GrpcDeadline(timeout));
            }
            // Connects to the channel
            await Connect(channel, connectionTask, robotConfiguration.Certificate, robotConfiguration.Guid, ipAddress).ConfigureAwait(false);
        }

        /// <summary>
        /// Connects to the robot using the remote connection information.  This is used for connecting to Vector when he's not on the LAN.  This requires port forwarding on your 
        /// router to setup an external connection to Vector.  For connecting to Vector on the same network as this application, use the <see cref="Connect(IRobotConfiguration, int, bool)"/> method instead.
        /// </summary>
        /// <param name="robotConfiguration">The remote robot configuration.</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="ArgumentNullException">robotConfiguration</exception>
        /// <exception cref="VectorConfigurationException">Remote host not specified for remote connection.</exception>
        public async Task RemoteConnect(IRemoteRobotConfiguration robotConfiguration, int timeout = DefaultConnectionTimeout)
        {
            if (robotConfiguration == null) throw new ArgumentNullException(nameof(robotConfiguration));
            if (string.IsNullOrWhiteSpace(robotConfiguration.RemoteHost)) throw new VectorConfigurationException("Remote host not specified for remote connection.");

            // If already connected, do nothing.
            if (IsConnected) return;

            // Reset disconnecting flag
            disconnecting = false;

            // Create the channel credentials
            var channelCredentials = CreateChannelCredentials(robotConfiguration.Certificate, robotConfiguration.Guid);
                       
            // Creates the channel
            var channel = CreateChannel(channelCredentials, robotConfiguration.RemoteHost, robotConfiguration.RobotName);
            // Connects to the channel
            await Connect(channel, channel.ConnectAsync(GrpcDeadline(timeout)), robotConfiguration.Certificate, robotConfiguration.Guid).ConfigureAwait(false);
        }

        /// <summary>
        /// Connects to the robot given the channel, connection task, certificate, guid, and IP addess.  This sets up all the instance variables
        /// for Vector's connection and will clean them up on failure.
        /// </summary>
        /// <param name="channel">The gRPC channel.</param>
        /// <param name="connectionTask">The connection task running ConnectAsync() on the channel.</param>
        /// <param name="certificate">The robot certificate (for HTTP).</param>
        /// <param name="guid">The unique identifier (for HTTP).</param>
        /// <param name="ipAddress">The optional IP address of the connection.</param>
        private async Task Connect(Channel channel, Task connectionTask, string certificate, string guid, IPAddress ipAddress = null)
        {
            try
            {
                // Connect and store channel, client, and http Client info
                this.grpcClient = await ConnectToGrpc(channel, connectionTask).ConfigureAwait(false);
                this.channel = channel;
                this.IPAddress = ipAddress;
                this.httpClient = CreateHttpClient(certificate, guid);
                // Start the event loop
                await this.Events.Start().ConfigureAwait(false);
            }
            catch (Exception)
            {
                this.grpcClient = null;
                await channel.ShutdownAsync().ConfigureAwait(false);
                this.channel = null;
                this.IPAddress = null;
                this.httpClient?.Dispose();
                this.httpClient = null;
                throw;
            }
        }

        /// <summary>
        /// Reads the state of the battery from the robot.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the battery state.</returns>
        public async Task<BatteryState> ReadBatteryState()
        {
            var response = await RunMethod(r => r.BatteryStateAsync(new BatteryStateRequest())).ConfigureAwait(false);
            response.Status.EnsureSuccess();
            return new BatteryState(response);
        }

        /// <summary>
        /// Reads the the versioning information for Vector, including Vector’s OS version and engine build id.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; The task result contains robot version information.</returns>
        public async Task<VersionState> ReadVersionState()
        {
            var response = await RunMethod(r => r.VersionStateAsync(new VersionStateRequest())).ConfigureAwait(false);
            response.Status.EnsureSuccess();
            return new VersionState(response);
        }

        /// <summary>
        /// Request the list of the current feature flags.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the robot setting.</returns>
        /// <remarks>To see which flags are enabled, use the Get Feature Flag command.</remarks>
        public async Task<IReadOnlyList<string>> GetFeatureFlagList()
        {
            var response = await RunMethod(client => client.GetFeatureFlagListAsync(new FeatureFlagListRequest())).ConfigureAwait(false);
            response.Status.EnsureSuccess();
            return response.List.ToList().AsReadOnly();
        }
        
        /// <summary>
        /// Request the current setting of a feature flag.
        /// </summary>
        /// <param name="name">The name of the feature to retrieve information about</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the robot setting.</returns>
        public async Task<bool> GetFeatureFlag(string name)
        {
            var response = await RunMethod(client => client.GetFeatureFlagAsync(new FeatureFlagRequest()
            {
                FeatureName = name
            })).ConfigureAwait(false);
            response.Status.EnsureSuccess();

            // Is the feature name valid?
            if (!response.ValidFeature) return false;
            return response.FeatureEnabled;
        }


        /// <summary>
        /// Gets the settings from the robot.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the robot setting.</returns>
        public async Task<RobotSettings> ReadRobotSettings()
        {
            var request = new PullJdocsRequest();
            request.JdocTypes.Add(JdocType.RobotSettings);
            var response = await RunMethod(r => r.PullJdocsAsync(request)).ConfigureAwait(false);
            response.Status.EnsureSuccess();
            return RobotSettings.FromNamedJdoc(response.NamedJdocs.FirstOrDefault());
        }

        /// <summary>
        /// Updates the robot settings.
        /// </summary>
        /// <param name="settings">The settings to update.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the updated settings.</returns>
        public async Task<RobotSettings> UpdateRobotSettings(RobotSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            var response = await SendHttpRequest<UpdateSettingsResponse, UpdateSettingsRequest>("/v1/update_settings", new UpdateSettingsRequest()
            {
                Settings = settings.ToRobotSettingsConfig()
            }).ConfigureAwait(false);
            if (response.Code != ResultCode.SettingsAccepted) return null;
            return RobotSettings.FromJdoc(response.Doc);
        }

        /// <summary>
        /// Gets the robot lifetime stats.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the robot lifetime stats.</returns>
        public async Task<RobotLifetimeStats> ReadRobotLifetimeStats()
        {
            var request = new PullJdocsRequest();
            request.JdocTypes.Add(JdocType.RobotLifetimeStats);
            var response = await RunMethod(r => r.PullJdocsAsync(request)).ConfigureAwait(false);
            response.Status.EnsureSuccess();
            return RobotLifetimeStats.FromNamedJdoc(response.NamedJdocs.FirstOrDefault());
        }

        /// <summary>
        /// Requests information about the most recent attention transfer
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the latest attention transfer.</returns>
        public async Task<Types.LatestAttentionTransfer> GetLatestAttentionTransfer()
        {
            var response = await RunMethod(client => client.GetLatestAttentionTransferAsync(new LatestAttentionTransferRequest())).ConfigureAwait(false);
            response.Status.EnsureSuccess();
            // There wasn't any reason given 
            if (response.LatestAttentionTransfer.AttentionTransfer == null) return null;
            return new Types.LatestAttentionTransfer(response.LatestAttentionTransfer);
        }


        /// <summary>
        /// Disconnects from the Robot
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task Disconnect()
        {
            return Disconnect(false);
        }

        /// <summary>
        /// Disconnects from the Robot.
        /// </summary>
        /// <param name="forced">if set to <c>true</c> the shutdown is forced due to disconnect from Vector.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Ignoring exceptions on shutdown")]
        internal async Task Disconnect(bool forced)
        {
            // If we are already disconnecting do allow re-entry
            if (disconnecting) return;

            // If no channel, we've already disconnected
            if (channel == null) return;

            // Mark as disconnected to the disconnect event doesn't cause a loop here
            disconnecting = true;

            // Exception triggered during teardown
            Exception exception = null;

            try
            {
                // End all component processing
                await Task.WhenAll(
                    Vision.Teardown(forced),
                    Audio.Teardown(forced),
                    Camera.Teardown(forced),
                    Animation.Teardown(forced),
                    Behavior.Teardown(forced),
                    Control.Teardown(forced),
                    Events.Teardown(forced),
                    Screen.Teardown(forced),
                    World.Teardown(forced),
                    Faces.Teardown(forced),
                    Photos.Teardown(forced),
                    NavMap.Teardown(forced)
                ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Save exception for later
                exception = ex;
            }

            // Clear the status;
            Status = new RobotStatus(0);
            Acceleration = new Acceleration();
            CarryingObjectId = -1;
            Gyro = new AngularVelocity();
            HeadAngleRad = 0;
            HeadTrackingObjectId = -1;
            LastImageTimestamp = 0;
            LeftWheelSpeedMmps = 0;
            LiftHeightMm = 0;
            LocalizedToObjectId = -1;
            Pose = new Pose();
            PoseAngleRad = 0;
            PosePitchRad = 0;
            Proximity = new ProximitySensorData();
            RightWheelSpeedMmps = 0;
            Touch = new TouchSensorData();
            IPAddress = null;

            try
            {
                await channel.ShutdownAsync().ConfigureAwait(false);
            }
            catch
            {
                // Ignore all exceptions
            }

            grpcClient = null;
            channel = null;
            httpClient?.Dispose();
            httpClient = null;

            // No longer disconnecting
            disconnecting = false;

            // Raise disconnected event
            Disconnected?.Invoke(this, new DisconnectedEventArgs());

            // Rethrow exception if one is triggered during teardown
            if (exception != null) System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception).Throw();
        }

        /// <summary>
        /// Creates the GRPC channel credentials.
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        private static ChannelCredentials CreateChannelCredentials(string certificate, string guid)
        {
            return ChannelCredentials.Create(
                new SslCredentials(certificate),
                CallCredentials.FromInterceptor((context, metadata) =>
                {
                    metadata.Add("authorization", $"Bearer {guid}");
                    return Task.CompletedTask;
                })
            );
        }

        /// <summary>
        /// Creates the GRPC channel.
        /// </summary>
        /// <param name="channelCredentials">The channel credentials.</param>
        /// <param name="hostAndPort">The host and port.</param>
        /// <param name="robotName">Name of the robot.</param>
        /// <returns></returns>
        private static Channel CreateChannel(ChannelCredentials channelCredentials, string hostAndPort, string robotName)
        {
            // Append the port if it doesn't exist
            if (!hostAndPort.Contains(':')) hostAndPort += ":443";
            // Create the channel
            return new Channel(
                hostAndPort,
                channelCredentials,
                new ChannelOption[] { new ChannelOption("grpc.ssl_target_name_override", robotName) }
            );
        }

        /// <summary>
        /// Creates the HTTP client for REST calls from the certificate and Guid.
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed by HTTP Client")]
        private static HttpClient CreateHttpClient(string certificate, string guid)
        {
            certificate = certificate.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "");

            string thumbprint;
            using (var cert = new X509Certificate2(Convert.FromBase64String(certificate))) thumbprint = cert.Thumbprint;
            var httpClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => cert.Thumbprint == thumbprint
            }, true);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", guid);
            return httpClient;
        }

        /// <summary>
        /// Connects to the channel and completes the connection process
        /// </summary>
        /// <param name="connectionTask">The connection task.</param>
        /// <exception cref="VectorNotFoundException">Unable to establish a connection to Vector.</exception>
        /// <exception cref="VectorInvalidVersionException">Your SDK version is not compatible with Vector’s version.</exception>
        private static async Task<ExternalInterfaceClient> ConnectToGrpc(Channel channel, Task connectionTask)
        {
            try
            {
                // Open the channel
                await connectionTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // If failed to open channel throw exception
                throw new VectorNotFoundException("Unable to establish a connection to Vector.", ex);
            }

            // This is a temporary variable containing client instance that is only stored
            // when we have successfully connected to the robot.
            var connectClient = new ExternalInterfaceClient(channel);

            try
            {
                // Get the client protocol version (also verifies connectivity)
                var result = await connectClient.ProtocolVersionAsync(new ProtocolVersionRequest()
                {
                    ClientVersion = (long)ProtocolVersion.Current,
                    MinHostVersion = (long)ProtocolVersion.Minimum
                });

                // Check the version
                if (result.Result != ProtocolVersionResponse.Types.Result.Success || (long)ProtocolVersion.Minimum > result.HostVersion)
                {
                    throw new VectorInvalidVersionException("Your SDK version is not compatible with Vector’s version.");
                }
            }
            catch (Grpc.Core.RpcException ex)
            {
                throw TranslateGrpcException(ex);
            }

            return connectClient;
        }

        /// <summary>
        /// Sends the HTTP REST request to Vector
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="address">The address (path of REST call) .</param>
        /// <param name="request">The request instance.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result of the command.</returns>
        /// <exception cref="VectorNotConnectedException">Vector is not connected.</exception>
        internal async Task<TResponse> SendHttpRequest<TResponse, TRequest>(string address, TRequest request) where TRequest : IHttpJsonData where TResponse : IHttpJsonData
        {
            if (!IsConnected) throw new VectorNotConnectedException("Vector is not connected.");

            var body = JsonConvert.SerializeObject(request, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            using (HttpContent content = new StringContent(body, System.Text.Encoding.UTF8, "application/json"))
            {
                var uri = new Uri("https://" + channel.ResolvedTarget + address);
                var result = await httpClient.PostAsync(uri, content).ConfigureAwait(false);
                result.EnsureSuccessStatusCode();
                var data = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<TResponse>(data);
            }
        }        

        /// <summary>
        /// Generate a deadline for GRPC calls
        /// </summary>
        /// <param name="timeout">The timeout in milliseconds.</param>
        /// <returns>Deadline value</returns>
        internal static DateTime GrpcDeadline(int timeout = DefaultConnectionTimeout) => DateTime.UtcNow.AddMilliseconds(timeout);

        /// <summary>
        /// Runs the client behavior method.  If control was lost, this method will attempt to gain control.  If control was not requested
        /// this method will throw an exception.
        /// </summary>
        /// <typeparam name="T">The result type of the command</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result of the command.</returns>
        internal Task<T> RunControlMethod<T>(Func<ExternalInterfaceClient, AsyncUnaryCall<T>> command, [CallerMemberName]string methodName = null)
        {
            return RunControlMethod(robot => command(robot).ResponseAsync, methodName);
        }

        /// <summary>
        /// Runs the client behavior method.  If control was lost, this method will attempt to gain control.  If control was not requested
        /// this method will throw an exception.
        /// </summary>
        /// <typeparam name="T">The result type of the command</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result of the command.</returns>
        /// <exception cref="VectorNotConnectedException">Vector is not connected.</exception>
        /// <exception cref="VectorControlException">Unable to acquire control of Vector.</exception>
        internal async Task<T> RunControlMethod<T>(Func<ExternalInterfaceClient, Task<T>> command, [CallerMemberName]string methodName = null)
        {
            // Cannot run method if not connected
            if (!IsConnected) throw new VectorNotConnectedException("Vector is not connected.");

            // If we don't have control and we aren't maintaining control then throw exception
            if (!this.Control.HasControl && !Control.MaintainBehaviorControl)
            {
                throw new VectorControlException($"Method {methodName ?? "unknown"} requires behavior control.");
            }

            // If we don't have control, request it.
            if (!this.Control.HasControl) await Control.RequestControl().ConfigureAwait(false);

            // If we still don't have control, throw exception
            if (!this.Control.HasControl) throw new VectorControlException("Unable to acquire control of Vector.");

            try
            {
                return await command(grpcClient).ConfigureAwait(false);
            }
            catch (Grpc.Core.RpcException ex)
            {
                throw TranslateGrpcException(ex);
            }
        }

        /// <summary>
        /// Runs the client method.
        /// </summary>
        /// <typeparam name="T">The result type of the command</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result of the command.</returns>
        internal Task<T> RunMethod<T>(Func<ExternalInterfaceClient, AsyncUnaryCall<T>> command)
        {
            return RunMethod(robot => command(robot).ResponseAsync);
        }

        /// <summary>
        /// Runs the client method.
        /// </summary>
        /// <typeparam name="T">The result type of the command</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result of the command.</returns>
        internal async Task<T> RunMethod<T>(Func<ExternalInterfaceClient, Task<T>> command)
        {
            // Cannot run method if not connected
            // We don't use IsConnected here because these methods might run during shutdown
            if (grpcClient == null) throw new VectorNotConnectedException("Vector is not connected.");

            try
            {
                return await command(grpcClient).ConfigureAwait(false);
            }
            catch (Grpc.Core.RpcException ex)
            {
                throw TranslateGrpcException(ex);
            }
        }

        /// <summary>
        /// Starts a client stream.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="command">The command to run.</param>
        /// <returns>The async GRPC stream</returns>
        internal AsyncServerStreamingCall<TResponse> StartStream<TResponse>(Func<ExternalInterfaceClient, AsyncServerStreamingCall<TResponse>> command)
        {
            // Cannot run method if not connected
            if (!IsConnected) throw new VectorNotConnectedException("Vector is not connected.");

            try
            {
                return command(grpcClient);
            }
            catch (Grpc.Core.RpcException ex)
            {
                throw TranslateGrpcException(ex);
            }
        }

        /// <summary>
        /// Starts a duplex client stream.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="command">The command to run.</param>
        /// <returns>The duplex async GRPC stream</returns>
        internal AsyncDuplexStreamingCall<TRequest, TResponse> StartStream<TRequest, TResponse>(Func<ExternalInterfaceClient, AsyncDuplexStreamingCall<TRequest, TResponse>> command)
        {
            // Cannot run method if not connected
            if (!IsConnected) throw new VectorNotConnectedException("Vector is not connected.");

            try
            {
                return command(grpcClient);
            }
            catch (Grpc.Core.RpcException ex)
            {
                throw TranslateGrpcException(ex);
            }
        }

        /// <summary>
        /// Converts a PEM string to a certificate.
        /// </summary>
        /// <param name="pemString">The PEM string.</param>
        /// <returns>X509Certificate2 certificate instance</returns>
        private static X509Certificate2 PemToCertificate(string pemString)
        {
            pemString = pemString.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "");
            return new X509Certificate2(Convert.FromBase64String(pemString));
        }

        /// <summary>
        /// Converts a PEM string to a certificate thumbprint.
        /// </summary>
        /// <param name="pemString">The PEM string.</param>
        /// <returns>The certificate thumbprint</returns>
        private static string PemToThumbprint(string pemString)
        {
            using (var certificate = PemToCertificate(pemString)) return certificate.Thumbprint;
        }
            
        #region Dispose Pattern

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
                if (disposing) Task.Run(() => Disconnect().ConfigureAwait(false)).Wait();
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

        /// <summary>
        /// Translates the GRPC exception into a Vector exception
        /// </summary>
        /// <param name="grpcException">The GRPC exception.</param>
        /// <returns>Translated exception</returns>
        internal static Exception TranslateGrpcException(Grpc.Core.RpcException grpcException)
        {
            switch (grpcException.StatusCode)
            {
                case Grpc.Core.StatusCode.Cancelled:
                    if (grpcException.Status.Detail == "Received http2 header with status: 401")
                    {
                        return new VectorUnauthenticatedException("Failed to authenticate communication with Vector.", grpcException);
                    }
                    return new TaskCanceledException("Operation was cancelled", grpcException);
                case Grpc.Core.StatusCode.DeadlineExceeded:
                    return new VectorTimeoutException("Communication with Vector timed out", grpcException);
                case Grpc.Core.StatusCode.Unauthenticated:
                    return new VectorUnauthenticatedException("Failed to authenticate communication with Vector.", grpcException);
                case Grpc.Core.StatusCode.Unavailable:
                    return new VectorUnavailableException("Unable to reach Vector.", grpcException);
                case Grpc.Core.StatusCode.Unimplemented:
                    return new VectorUnimplementedException("Vector does not handle this message.", grpcException);
                default:
                    return new VectorConnectionException("Connection to Vector failed.", grpcException);
            }
        }
    }
}
