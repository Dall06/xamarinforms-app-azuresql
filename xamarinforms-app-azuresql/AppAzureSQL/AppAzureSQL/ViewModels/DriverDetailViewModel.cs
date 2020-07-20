using AppAzureSQL.Models;
using AppAzureSQL.Services;
using AppAzureSQL.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppAzureSQL.ViewModels
{
    public class DriverDetailViewModel : BaseViewModel
    {
        private readonly int id;

        /*********************COMMANDS***********************/
        Command deleteCommand;
        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(DeleteAction));

        Command editCommand;
        public Command EditCommand => editCommand ?? (editCommand = new Command(EditAction));

        Command cancelCommand;
        public Command CancelCommand => cancelCommand ?? (cancelCommand = new Command(CancelAction));

        /*********************BINDABLE PROPS***********************/
        private DriverModel driverSel;
        public DriverModel DriverSelected
        {
            get => driverSel;
            set => SetProperty(ref driverSel, value);
        }

        string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        string _Status;
        public string Status
        {
            get => _Status;
            set => SetProperty(ref _Status, value);
        }

        string _Picture;
        public string Picture
        {
            get => _Picture;
            set => SetProperty(ref _Picture, value);
        }

        double _Latitude;
        public double Latitude
        {
            get => _Latitude;
            set => SetProperty(ref _Latitude, value);
        }

        double _Longitude;
        public double Longitude
        {
            get => _Longitude;
            set => SetProperty(ref _Longitude, value);
        }

        /*********************CONSTRUCTORS***********************/
        public DriverDetailViewModel()
        {
            DriverSelected = new DriverModel();
        }

        public DriverDetailViewModel(DriverModel driver)
        {
            Name = driver.Name;
            Status = driver.Status;
            Picture = driver.Picture;
            Latitude = driver.ActualPosition.Latitude;
            Longitude = driver.ActualPosition.Longitude;
            id = driver.Id;
            DriverSelected = driver;
        }

        private async void CancelAction()
        {
            // await Application.Current.MainPage.Navigation.PopAsync();
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void DeleteAction()
        {
            IsBusy = true;

            if(id == 0)
                await Application.Current.MainPage.DisplayAlert("Invalid Id", "Error", "Ok");
            else
            {
                ApiResponse response = await new ApiService().DeleteDataAsync("driver", id);
                if (response == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Drivers app", "Error", "Ok");
                    return;
                }
                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Drivers app", response.Message, "Ok");
                    return;
                }
                DriversViewModel.GetInstance().RefreshDrivers();
                await Application.Current.MainPage.DisplayAlert("Drivers app", response.Message, "Ok");

                IsBusy = false;
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        private async void EditAction()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new DriverFormPage(DriverSelected))
            {
                BackgroundColor = Color.FromHex("#0F1923")
            });
        }

    }
}
