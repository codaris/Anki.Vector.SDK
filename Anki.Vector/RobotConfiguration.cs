// <copyright file="RobotConfiguration.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Anki.Vector.Exceptions;
using IniParser;
using IniParser.Model;

namespace Anki.Vector
{
    /// <summary>
    /// The robot configuration information
    /// </summary>
    /// <seealso cref="Anki.Vector.IRobotConfiguration" />
    public class RobotConfiguration : IRobotConfiguration
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Gets or sets the certificate.
        /// </summary>
        public string Certificate { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the robot
        /// </summary>
        public IPAddress IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the name of the robot.  This is in the form "Vector-XXXX"
        /// </summary>
        public string RobotName { get; set; }

        /// <summary>Gets or sets the robot serial number.</summary>
        public string SerialNumber { get; set; }

        /// <summary>The unique identifier INI field key</summary>
        private const string GuidKey = "guid";

        /// <summary>The ip INI field key</summary>
        private const string IpKey = "ip";

        /// <summary>The robot name INI field key</summary>
        private const string NameKey = "name";

        /// <summary>The cert INI field key</summary>
        private const string CertKey = "cert";

        /// <summary>
        /// Gets the default SDK configuration file path.
        /// </summary>
        private static string DefaultSdkConfigFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".anki_vector", "sdk_config.ini");

        /// <summary>
        /// Loads the configuration file using the default SDK configuration file path.
        /// </summary>
        /// <returns>Robot configurations</returns>
        public static IEnumerable<RobotConfiguration> Load()
        {
            return Load(DefaultSdkConfigFilePath);
        }

        /// <summary>
        /// Loads the first configuration entry from the default configuration file.
        /// </summary>
        /// <returns>Robot configuration</returns>
        public static RobotConfiguration LoadDefault() => Load().FirstOrDefault();

        /// <summary>
        /// Loads the configuration file using the specified SDK configuration file path.
        /// </summary>
        /// <param name="sdkConfigFilePath">The SDK configuration file path.</param>
        /// <returns>Robot configurations</returns>
        public static IEnumerable<RobotConfiguration> Load(string sdkConfigFilePath)
        {
            if (!File.Exists(sdkConfigFilePath)) yield break;
            var parser = new FileIniDataParser();
            var configData = parser.ReadFile(sdkConfigFilePath, Encoding.ASCII);
            foreach (var section in configData.Sections) yield return ReadConfig(section.SectionName, configData[section.SectionName]);
        }

        /// <summary>
        /// Adds the specified robot configuration to the configuration file using the default SDK configuration file path.
        /// <para>The robot configuration is appended if new or updated if already exists</para>
        /// </summary>
        /// <param name="robot">The robot.</param>
        public static void AddOrUpdate(IRobotConfiguration robot)
        {
            AddOrUpdate(DefaultSdkConfigFilePath, robot);
        }

        /// <summary>
        /// Adds the specified robot configuration to the configuration file using the specified SDK configuration file path.
        /// <para>The robot configuration is appended if new or updated if already exists</para>
        /// </summary>
        /// <param name="sdkConfigFilePath">The SDK configuration file path.</param>
        /// <param name="robot">The robot configuration.</param>
        public static void AddOrUpdate(string sdkConfigFilePath, IRobotConfiguration robot)
        {
            var list = new List<IRobotConfiguration>() { robot };
            SaveFile(sdkConfigFilePath, list);
        }

        /// <summary>
        /// Stores the specified robot configurations using the default SDK configuration file path.
        /// <para>Any robot configurations not in the list will be removed.</para>
        /// </summary>
        /// <param name="robots">The robot configurations.</param>
        public static void Save(IEnumerable<IRobotConfiguration> robots)
        {
            Save(DefaultSdkConfigFilePath, robots);
        }

        /// <summary>
        /// Stores the specified robot configurations using the specified SDK configuration file path.
        /// <para>Any robot configurations not in the list will be removed.</para>
        /// </summary>
        /// <param name="sdkConfigFilePath">The SDK configuration file path.</param>
        /// <param name="robots">The robots configurations.</param>
        public static void Save(string sdkConfigFilePath, IEnumerable<IRobotConfiguration> robots)
        {
            SaveFile(sdkConfigFilePath, robots, true);
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="sdkConfigFilePath">The SDK configuration file path.</param>
        /// <param name="robots">The robots.</param>
        /// <param name="replaceAll">if set to <c>true</c> replace all entries with specified robots.</param>
        private static void SaveFile(string sdkConfigFilePath, IEnumerable<IRobotConfiguration> robots, bool replaceAll = false)
        {
            string sdkConfigDir = Path.GetDirectoryName(sdkConfigFilePath);
            var parser = new FileIniDataParser();
            var configData = File.Exists(sdkConfigFilePath) ? parser.ReadFile(sdkConfigFilePath) : new IniData();
            var serialNumbers = new List<string>();

            // Save each robot
            foreach (var robot in robots)
            {
                robot.Validate();
                if (!configData.Sections.ContainsSection(robot.SerialNumber))
                {
                    configData.Sections.AddSection(robot.SerialNumber);
                }
                WriteConfig(robot, sdkConfigDir, configData[robot.SerialNumber]);
                serialNumbers.Add(robot.SerialNumber);
            }

            if (replaceAll)
            {
                // Remove any robots that no longer exist
                foreach (var section in configData.Sections.ToArray())
                {
                    if (!serialNumbers.Contains(section.SectionName)) configData.Sections.RemoveSection(section.SectionName);
                }
            }

            // Write the file
            parser.WriteFile(sdkConfigFilePath, configData, Encoding.ASCII);
        }

        /// <summary>
        /// Creates the configuration.
        /// </summary>
        /// <param name="serialNumber">The serial number.</param>
        /// <param name="data">The data.</param>
        /// <returns>A filled robot configuration instance</returns>
        private static RobotConfiguration ReadConfig(string serialNumber, KeyDataCollection data)
        {
            try
            {
                return new RobotConfiguration()
                {
                    SerialNumber = serialNumber,
                    Guid = data[GuidKey],
                    IPAddress = data.ContainsKey(IpKey) ? IPAddress.Parse(data[IpKey]) : null,
                    RobotName = data[NameKey],
                    Certificate = File.ReadAllText(data[CertKey])
                };
            }
            catch (Exception ex)
            {
                throw new VectorConfigurationException("Invalid robot configuration in file", ex);
            }
        }

        /// <summary>
        /// Updates the configuration file data
        /// </summary>
        /// <param name="robot">The robot configuration.</param>
        /// <param name="ankiVectorPath">The anki_vector folder path.</param>
        /// <param name="data">The configuration file data.</param>
        private static void WriteConfig(IRobotConfiguration robot, string ankiVectorPath, KeyDataCollection data)
        {
            data[GuidKey] = robot.Guid;
            data[NameKey] = robot.RobotName;
            if (!data.ContainsKey(CertKey)) data[CertKey] = Path.Combine(ankiVectorPath, robot.RobotName + "-" + robot.SerialNumber + ".cert");
            if (robot.IPAddress != null) data[IpKey] = robot.IPAddress.ToString();

            if (!File.Exists(data[CertKey]))
            {
                File.WriteAllText(data[CertKey], robot.Certificate);
            }
        }
    }
}
