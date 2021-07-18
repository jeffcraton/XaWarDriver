using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XaWarDriver.Services;
using System.Collections.ObjectModel;
using XaWarDriver.Models;
using Android.Content;

namespace XaWarDriver.ViewModels
{
    public class ScanningViewModel : BaseViewModel
    {
        private double _latitude = 0;
        private double _longitude = 0;
        public ObservableCollection<Networkreadings> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public ScanningViewModel()
        {
            Title = "Scanning";
            PullGPSLocation = new Command(async () => await GetGPSLocationAsync());
            ScanWifiNetworks = new Command(async () => await PullWifiDataAsync());
            SyncWithCloud = new Command(async () => await TransmitToCloudAsync());
        }

        public ICommand PullGPSLocation { get; }
        public ICommand ScanWifiNetworks { get; }
        public ICommand SyncWithCloud { get; }

        public async System.Threading.Tasks.Task GetGPSLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    _latitude = location.Latitude;
                    _longitude = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        public async System.Threading.Tasks.Task PullWifiDataAsync()
        {
            Intent wifiIntent = new Intent("common.xamarin.WifiService");            
        }

        public async System.Threading.Tasks.Task TransmitToCloudAsync()
        {
        }

    }
}