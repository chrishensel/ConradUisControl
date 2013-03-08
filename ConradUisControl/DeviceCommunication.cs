﻿using System;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace ConradUisControl
{
    static class DeviceCommunication
    {
        #region Constants

        private const int ReadWriteTimeout = 10000;
        private const int RequestTimeout = 5000;

        #endregion

        internal static string GetConfiguredDeviceHostUri()
        {
            return string.Format("{0}:{1}", CucConfiguration.DeviceIpAddress, CucConfiguration.DevicePort);
        }

        /// <summary>
        /// Connects to the device, authenticates using the standard password, and makes a request using the given relative URI.
        /// </summary>
        /// <param name="relativeUri">The URI. Must be relative to 'http://IP/". </param>
        /// <returns>The XML response from the device. Weird: Sometimes the response cannot be fetched (an exception occurs)!</returns>
        internal static XDocument MakeRequest(string relativeUri)
        {
            string uri = string.Format("http://{0}:{1}/{2}", CucConfiguration.DeviceIpAddress, CucConfiguration.DevicePort, relativeUri);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            string username = CucConfiguration.Username;
            string password = CucConfiguration.Password;
            string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + encoded);

            request.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
            request.Method = "GET";
            request.IfModifiedSince = new DateTime(0L);

            request.ReadWriteTimeout = ReadWriteTimeout;
            request.Timeout = RequestTimeout;

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    return XDocument.Load(response.GetResponseStream());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Properties.Resources.MakeRequestError, uri, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Returns the outlet status.
        /// </summary>
        /// <returns>The outlet status, represented by booleans. -or- null, if status could not be retrieved.</returns>
        internal static bool[] GetOutletStatus()
        {
            XDocument response = MakeRequest("outlet_status.xml");
            if (response == null)
            {
                return null;
            }

            XElement outletStatus = response.Root.Element("outlet_status");

            string[] temp = outletStatus.Value.Split(',');

            bool[] status = new bool[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                bool v = (temp[i] == "1") ? true : false;
                status[i] = v;
            }

            return status;
        }

        internal static bool SetOutletStatus(int outletIndex, bool enabled)
        {
            if (!IsOutletIndexAvailable(outletIndex))
            {
                return false;
            }

            string enable = enabled ? "1" : "0";

            string uri = string.Format("control.cgi?outlet={0}&command={1}&time={2}", outletIndex, enable, GetTime());

            // Response is ignored here.
            XDocument response = MakeRequest(uri);
            if (response != null)
            {
                // TODO
            }

            return true;
        }

        private static long GetTime()
        {
            DateTime st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
            return (long)t.TotalMilliseconds;
        }

        internal static bool IsOutletIndexAvailable(int outletIndex)
        {
            return outletIndex >= 0 && outletIndex <= CucConfiguration.OutletCount;
        }
    }
}
