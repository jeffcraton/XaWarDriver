using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Net.Wifi;
using System.Threading;
using XaWarDriver.Models;
using Android.App;
using Android.OS;

namespace XaWarDriver.Services
{
    [Service]
    [IntentFilter(new String[] { "com.xamarin.WifiService" })]
    public class Wifi : Service
    {
        private Context context = null;
        private static WifiManager wifi;
        private WifiReceiver wifiReceiver;
        public static Dictionary<string, Networkreadings> WiFiNetworks;
        private int _ScanIntervalInSeconds = 60;
        public IBinder Binder { get; private set; }

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
        public Wifi()
        {
            WiFiNetworks = new Dictionary<string, Networkreadings>();
        }
        public Wifi(Context ctx)
        {
            this.context = ctx;
            WiFiNetworks = new Dictionary<string, Networkreadings>();
        }

        public void GetWifiNetworks()
        {
            WiFiNetworks = new Dictionary<string, Networkreadings>();

            // Get a handle to the Wifi
            wifi = (WifiManager)context.GetSystemService(Context.WifiService);

            // Start a scan and register the Broadcast receiver to get the list of Wifi Networks
            wifiReceiver = new WifiReceiver();
            context.RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            wifi.StartScan();            
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        class WifiReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                IList<ScanResult> scanwifinetworks = wifi.ScanResults;
                foreach (ScanResult wifinetwork in scanwifinetworks)
                {
                    if (!WiFiNetworks.ContainsKey(wifinetwork.Bssid))
                        {
                        WiFiNetworks.Add(wifinetwork.Bssid, new Networkreadings {
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
}
