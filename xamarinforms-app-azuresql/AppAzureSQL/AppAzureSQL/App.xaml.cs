using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppAzureSQL.Services;
using AppAzureSQL.Views;

namespace AppAzureSQL
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage()
            {
                BackgroundColor = (Color)App.Current.Resources["primaryColor"],
                Title = "App Driver"
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
