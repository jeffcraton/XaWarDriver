using System.ComponentModel;
using Xamarin.Forms;
using XaWarDriver.ViewModels;

namespace XaWarDriver.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}