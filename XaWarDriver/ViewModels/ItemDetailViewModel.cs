using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XaWarDriver.Models;

namespace XaWarDriver.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string _ssid;
        private string _neworkname;
        private string _open;
        private string _crypto;
        private string _frequency;
        public string Id { get; set; }

        public string ssid
        {
            get => _ssid;
            set => SetProperty(ref _ssid, value);
        }

        public string networkname
        {
            get => _neworkname;
            set => SetProperty(ref _neworkname, value);
        }
        public string open
        {
            get => _open;
            set => SetProperty(ref _open, value);
        }
        public string crypto
        {
            get => _crypto;
            set => SetProperty(ref _crypto, value);
        }
        public string frequency
        {
            get => _frequency;
            set => SetProperty(ref _frequency, value);
        }
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                ssid = item.ssid;
                networkname = item.networkname;
                open = item.open;
                crypto = item.crypto;
                frequency = item.frequency;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
