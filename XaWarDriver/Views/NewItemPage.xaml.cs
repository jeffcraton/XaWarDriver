using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XaWarDriver.Models;
using XaWarDriver.ViewModels;

namespace XaWarDriver.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Networkreadings Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}