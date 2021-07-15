using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Net.Wifi;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using XaWarDriver.Models;

namespace XaWarDriver.Services
{
    public class Wifi
    {
        private Context context = null;
        private static WifiManager wifi;
        private WifiReceiver wifiReceiver;
        public static List<Networkreadings> WiFiNetworks;
        private int _ScanIntervalInSeconds = 60;

        public int ScanIntervalInSeconds
        {
            get
            {
                return _ScanIntervalInSeconds;
            }
            set
            {
                _ScanIntervalInSeconds = value;
            }
        }

        public Wifi(Context ctx)
        {
            this.context = ctx;
        }

        public void GetWifiNetworks()
        {
            WiFiNetworks = new List<Networkreadings>();

            // Get a handle to the Wifi
            wifi = (WifiManager)context.GetSystemService(Context.WifiService);

            // Start a scan and register the Broadcast receiver to get the list of Wifi Networks
            wifiReceiver = new WifiReceiver();
            context.RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            wifi.StartScan();
        }

        class WifiReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                IList<ScanResult> scanwifinetworks = wifi.ScanResults;
                foreach (ScanResult wifinetwork in scanwifinetworks)
                {
                    WiFiNetworks.Add(new Networkreadings { 
                        ssid = wifinetwork.Bssid,
                        networkname = wifinetwork.Ssid,
                        crypto = wifinetwork.Capabilities,
                        open = wifinetwork.WifiStandard.ToString(),
                        frequency = wifinetwork.Frequency.ToString(),
                        datescanned = DateTime.Now,
                        sentToCloud = false
                    });

                }
            }
        }
    }
}
