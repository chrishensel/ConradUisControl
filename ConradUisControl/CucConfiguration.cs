using System;
using System.Configuration;
using ConradUisControl.Library;

namespace ConradUisControl
{
    class CucConfiguration : ICucConfiguration
    {
        public int ListenPort
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["listenPort"]); }
        }

        public string DeviceIpAddress
        {
            get { return ConfigurationManager.AppSettings["deviceIp"]; }
        }

        public int DevicePort
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["devicePort"]); }
        }

        public string Username
        {
            get { return ConfigurationManager.AppSettings["username"]; }
        }

        public string Password
        {
            get { return ConfigurationManager.AppSettings["password"]; }
        }

        public int OutletCount
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["outletCount"]); }
        }

    }
}
