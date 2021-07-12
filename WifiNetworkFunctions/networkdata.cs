using System;
using System.Collections.Generic;
using System.Text;

namespace WifiNetworkFunctions
{
    public class networkdata
    {
        public string id = System.Guid.NewGuid().ToString();
        public string ssid = "";
        public string networkname = "";
        public string open = "";
        public string crypto = "";
        public string frequency = "";
        public string dateadded = DateTime.Now.ToShortDateString();
    }
}
