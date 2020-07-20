using AppAzureSQL.Models;
using AppAzureSQL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAzureSQL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriverFormPage : ContentPage
    {
        public DriverFormPage()
        {
            InitializeComponent();

            BindingContext = new DriverFormViewModel();
        }

        public DriverFormPage(DriverModel driver)
        {
            InitializeComponent();

            BindingContext = new DriverFormViewModel(driver);
        }
    }
}