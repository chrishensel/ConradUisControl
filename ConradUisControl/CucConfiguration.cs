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
    }
}
