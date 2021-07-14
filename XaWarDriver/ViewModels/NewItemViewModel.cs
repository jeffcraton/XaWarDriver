using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XaWarDriver.Models;

namespace XaWarDriver.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string _ssid;
        private string _networkname;
        private string _open;
        private string _crypto;
        private string _frequency;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(_ssid)
                && !String.IsNullOrWhiteSpace(_networkname);
        }

        public string ssid
        {
            get => _ssid;
            set => SetProperty(ref _ssid, value);
        }

        public string networkname
        {
            get => _networkname;
            set => SetProperty(ref _networkname, value);
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

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Networkreadings newItem = new Networkreadings()
            {
                Id = Guid.NewGuid().ToString(),
                ssid = _ssid,
                networkname = _networkname
            };

            await DataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
