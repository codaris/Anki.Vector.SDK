// <copyright file="Client.cs" company="Wayne Venables">
//     Copyright (c) 2020 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Anki.Vector.Exceptions;
using Anki.Vector.ExternalInterface;
using Grpc.Core;
using Newtonsoft.Json;
using static Anki.Vector.ExternalInterface.ExternalInterface;

namespace Anki.Vector
{
    /// <summary>
    /// This class abstracts all the gRPC connection login and provides access to the underlying Vector interface.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal class Client : IDisposable
    {
        /// <summary>
        /// Gets the current IP address of the robot.  Can be null if remote Vector connection is used.
        /// </summary>
        public IPAddress IPAddress { get; }

        /// <summary>
        /// The GRPC channel
        /// </summary>
        private Channel channel = null;

        /// <summary>
        /// The gRPC client
        /// </summary>
        private ExternalInterfaceClient grpcClient = null;

        /// <summary>
        /// The HTTP client for REST calls to Vector
        /// </summary>
        private HttpClient httpClient = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="ipAddress">The ip address.</param>
        private Client(Channel channel, string certificate, string guid, IPAddress ipAddress) 
        {
            this.channel = channel;
            this.httpClient = CreateHttpClient(certificate, guid);
            this.grpcClient = new ExternalInterfaceClient(channel);
            this.IPAddress = ipAddress;
        }

        /// <summary>
        /// Connect to Vector on the local network using the specified robotConfiguration and IP address
        /// </summary>
        /// <param name="robotConfiguration">The robot configuration.</param>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the connected client.</returns>
        public static async Task<Client> Connect(IRobotConfiguration robotConfiguration, IPAddress ipAddress, int timeout)
        {
            var channelCredentials = CreateChannelCredentials(robotConfiguration.Certificate, robotConfiguration.Guid);
            var channel = await ConnectChannel(channelCredentials, ipAddress.ToString(), robotConfiguration.RobotName, timeout).ConfigureAwait(false);
            return await ConnectClient(channel, robotConfiguration.Certificate, robotConfiguration.Guid, ipAddress).ConfigureAwait(false);
        }

        /// <summary>
        /// Connects to the robot using the remote connection information.  This is used for connecting to Vector when he's not on the LAN.  
        /// </summary>
        /// <param name="robotConfiguration">The robot configuration.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the connected client.</returns>
        public static async Task<Client> RemoteConnect(IRemoteRobotConfiguration robotConfiguration, int timeout)
        {
            // Create the channel credentials
            var channelCredentials = CreateChannelCredentials(robotConfiguration.Certificate, robotConfiguration.Guid);
            var channel = await ConnectChannel(channelCredentials, robotConfiguration.RemoteHost, robotConfiguration.RobotName, timeout).ConfigureAwait(false);
            return await ConnectClient(channel, robotConfiguration.Certificate, robotConfiguration.Guid, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Connects to the robot for authentication.
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        /// <param name="host">The host.</param>
        /// <param name="robotName">Name of the robot.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the connected client.</returns>
        public static async Task<Client> ConnectForAuth(string certificate, string host, string robotName, int timeout)
        {
            // Create the channel credentials
            var channelCredentials = CreateChannelCredentials(certificate, null);
            var channel = await ConnectChannel(channelCredentials, host, robotName, timeout).ConfigureAwait(false);
            return new Client(channel, certificate, null, null);
        }

        /// <summary>
        /// Creates and connects the client instance.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the connected client.</returns>
        /// <exception cref="VectorInvalidVersionException">Your SDK version is not compatible with Vector’s version.</exception>
        private static async Task<Client> ConnectClient(Channel channel, string certificate, string guid, IPAddress ipAddress = null)
        {
            Client client = null;

            try
            {
                // Create the client
                client = new Client(channel, certificate, guid, ipAddress);

                // Get the client protocol version (also verifies connectivity)
                var result = await client.RunCommand(grpc => grpc.ProtocolVersionAsync(new ProtocolVersionRequest()
                {
                    ClientVersion = (long)ExternalInterface.ProtocolVersion.Current,
                    MinHostVersion = (long)ExternalInterface.ProtocolVersion.Minimum
                }));

                // Check the version
                if (result.Result != ProtocolVersionResponse.Types.Result.Success || (long)ExternalInterface.ProtocolVersion.Minimum > result.HostVersion)
                {
                    throw new VectorInvalidVersionException("Your SDK version is not compatible with Vector’s version.");
                }
            }
            catch (Exception)
            {
                client?.Dispose();
                throw;
            }

            return client;
        }

        /// <summary>
        /// Runs a command against the gRPC client instance.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result of the command.</returns>
        public async Task<TResult> RunCommand<TResult>(Func<ExternalInterfaceClient, Task<TResult>> command)
        {
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
        /// Runs a command against the gRPC client instance.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result of the command.</returns>
        public TResult RunCommand<TResult>(Func<ExternalInterfaceClient, TResult> command)
        {
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
        /// Creates the GRPC channel credentials.
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        /// <param name="guid">The unique identifier (optional).</param>
        /// <returns>The constructed channel credentials</returns>
        private static ChannelCredentials CreateChannelCredentials(string certificate, string guid)
        {
            return ChannelCredentials.Create(
                new SslCredentials(certificate),
                CallCredentials.FromInterceptor((context, metadata) =>
                {
                    if (guid != null) metadata.Add("authorization", $"Bearer {guid}");
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
        /// <param name="timeout">The timeout.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the connected channel.</returns>
        /// <exception cref="VectorNotFoundException">Unable to establish a connection to Vector.</exception>
        private static async Task<Channel> ConnectChannel(ChannelCredentials channelCredentials, string hostAndPort, string robotName, int timeout)
        {
            // Append the port if it doesn't exist
            if (!hostAndPort.Contains(':')) hostAndPort += ":443";
            
            // Create the channel
            var channel = new Channel(
                hostAndPort,
                channelCredentials,
                new ChannelOption[] { new ChannelOption("grpc.ssl_target_name_override", robotName) }
            );

            // Connect to the channel
            try
            {
                // Open the channel
                await channel.ConnectAsync(GrpcDeadline(timeout)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // If failed to open channel throw exception
                channel?.ShutdownAsync();
                throw new VectorNotFoundException("Unable to establish a connection to Vector.", ex);
            }

            return channel;
        }

        /// <summary>
        /// Creates the HTTP client for REST calls from the certificate and Guid.
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>The HTTP client to connect to Vector</returns>
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
            if (guid != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", guid);
            }
            return httpClient;
        }

        /// <summary>
        /// Sends the HTTP REST command to Vector
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="address">The address (path of REST call) .</param>
        /// <param name="request">The request instance.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result of the command.</returns>
        /// <exception cref="VectorNotConnectedException">Vector is not connected.</exception>
        internal async Task<TResponse> RunHttpCommand<TResponse, TRequest>(string address, TRequest request) where TRequest : IHttpJsonData where TResponse : IHttpJsonData
        {
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
        internal static DateTime GrpcDeadline(int timeout) => DateTime.UtcNow.AddMilliseconds(timeout);

        /// <summary>
        /// Translates the GRPC exception into a Vector exception
        /// </summary>
        /// <param name="grpcException">The GRPC exception.</param>
        /// <returns>Translated exception</returns>
        private static Exception TranslateGrpcException(Grpc.Core.RpcException grpcException)
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

        #region IDisposable Support

        /// <summary>To detect redundant calls </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Ignore all exceptions on channel shutdown")]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (channel != null)
                    {
                        try
                        {
                            channel.ShutdownAsync().ConfigureAwait(false);
                        }
                        catch
                        {
                            // Ignore all exceptions
                        }
                    }
                    channel = null;
                    httpClient?.Dispose();
                    httpClient = null;
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}
