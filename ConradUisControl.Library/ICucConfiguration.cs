using System;

namespace ConradUisControl.Library
{
    /// <summary>
    /// Defines the configuration.
    /// </summary>
    public interface ICucConfiguration
    {
        /// <summary>
        /// Gets the port on the local machine to listen on.
        /// </summary>
        int ListenPort { get; }
        /// <summary>
        /// Gets the IP address of the device to listen on.
        /// </summary>
        string DeviceIpAddress { get; }
        /// <summary>
        /// Gets the port of the device to listen on.
        /// </summary>
        int DevicePort { get; }
        /// <summary>
        /// Gets the username on the device to use for authentication.
        /// </summary>
        string Username { get; }
        /// <summary>
        /// Gets the password on the device to use for authentication.
        /// </summary>
        string Password { get; }
        /// <summary>
        /// Gets the outlet count of the device.
        /// </summary>
        int OutletCount { get; }
    }
}
