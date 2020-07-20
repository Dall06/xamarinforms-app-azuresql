using AppAzureSQL.Models;
using AppAzureSQL.Services;
using AppAzureSQL.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace AppAzureSQL.ViewModels
{
    public class DriversViewModel : BaseViewModel
    {
        static DriversViewModel _instance;

        Command _selectCommand;
        public Command SelectCommand => _selectCommand ?? (_selectCommand = new Command(SelectAction));

        //Command which refresh the data
        Command refreshCommand;
        public Command RefreshCommand => refreshCommand ?? (refreshCommand = new Command(RefreshDrivers));

        Command createCommand;
        public Command CreateCommand => createCommand ?? (createCommand = new Command(CreateAction));

        DriverModel driverSelected;
        public DriverModel DriverSelected
        {
            get => driverSelected;
            set => SetProperty(ref driverSelected, value);
        }

        ObservableCollection<DriverModel> _Drivers;
        public ObservableCollection<DriverModel> Drivers
        {
            get => _Drivers;
            set => SetProperty(ref _Drivers, value);
        }

        public DriversViewModel()
        {
            Title = "Drivers";
            _instance = this;
            LoadDrivers();
        }

        private async void LoadDrivers()
        {
            IsBusy = true;
            ApiResponse response = await new ApiService().GetDataAsync<DriverModel>("driver");

            if (response == null || !response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error loading drivers", response.Message, "Ok");
                return;
            }

            Drivers = (ObservableCollection<DriverModel>)response.Result;
            IsBusy = false;
        }

        public static DriversViewModel GetInstance()
        {
            if (_instance == null) _instance = new DriversViewModel();
            return _instance;
        }

        private async void SelectAction()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new DriverDetailPage(DriverSelected)) 
            {
                BackgroundColor = Color.FromHex("#0F1923")
            });
        }

        private async void CreateAction()
        {
            //Application.Current.MainPage.Navigation.PushAsync(new DriverDetailPage(DriverSelected));
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new DriverFormPage())
            {
                BackgroundColor = Color.FromHex("#0F1923")
            });
        }

        public void RefreshDrivers()
        {
            LoadDrivers();
        }
    }
}
