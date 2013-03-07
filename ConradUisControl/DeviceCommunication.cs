using System;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace ConradUisControl
{
    static class DeviceCommunication
    {
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
            string username = "admin";
            string password = "";
            string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + encoded);

            request.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
            request.Method = "GET";
            request.IfModifiedSince = new DateTime(0L);

            using (WebResponse response = request.GetResponse())
            {
                try
                {
                    return XDocument.Load(response.GetResponseStream());
                }
                catch (Exception)
                {

                }
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
            // Usage:
            //    outlet_uis(index, 2)
            //
            // function outlet_uis(olid, on_off)
            // {
            // var dttoday = new Date();
            // var answer = confirm("Sind Sie sicher??");
            // if(answer) {
            // var execute_uri = 'control.cgi?outlet='+ olid + '&command=' + on_off + "&time=" + dttoday.getTime();
            // document.getElementById('execute_cgi').src = execute_uri;
            // }

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
    }
}
