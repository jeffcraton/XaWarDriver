using System;
using System.Collections.Generic;
using System.Text;

namespace WifiNetworkFunctions
{
    public class gpsreadings
    {
        public string id = System.Guid.NewGuid().ToString();
        public string ssid = "";
        public string slat = "";
        public string slon = "";
        public string dateadded = DateTime.Now.ToShortDateString();
    }
}
