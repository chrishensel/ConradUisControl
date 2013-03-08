using System;
using System.Configuration;

namespace ConradUisControl
{
    static class CucConfiguration
    {
        public static int ListenPort
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["listenPort"]); }
        }

        public static string DeviceIpAddress
        {
            get { return ConfigurationManager.AppSettings["deviceIp"]; }
        }

        public static int DevicePort
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["devicePort"]); }
        }

        public static string Username
        {
            get { return ConfigurationManager.AppSettings["username"]; }
        }

        public static string Password
        {
            get { return ConfigurationManager.AppSettings["password"]; }
        }

        public static string HttpEnableOutletMethodScheme
        {
            get { return ConfigurationManager.AppSettings["httpEnableOutletMethodScheme"]; }
        }

        public static string HttpDisableOutletMethodScheme
        {
            get { return ConfigurationManager.AppSettings["httpDisableOutletMethodScheme"]; }
        }

        public static int OutletCount
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["outletCount"]); }
        }

    }
}
