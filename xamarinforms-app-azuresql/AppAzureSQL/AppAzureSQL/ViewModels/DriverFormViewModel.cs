using AppAzureSQL.Models;
using AppAzureSQL.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAzureSQL.ViewModels
{
    public class DriverFormViewModel : BaseViewModel
    {
        private int id;

        Command cancelCommand;
        public Command CancelCommand => cancelCommand ?? (cancelCommand = new Command(CancelAction));

        Command _GetLocationCommand;
        public Command GetLocationCommand => _GetLocationCommand ?? (_GetLocationCommand = new Command(GetLocationAction));

        Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(SaveAction));

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
        public DriverFormViewModel()
        {
            DriverSelected = new DriverModel();
        }

        public DriverFormViewModel(DriverModel driver)
        {
            Name = driver.Name;
            Status = driver.Status;
            Picture = driver.Picture;
            Latitude = driver.ActualPosition.Latitude;
            Longitude = driver.ActualPosition.Longitude;
            id = driver.Id;
            DriverSelected = driver;
        }

        /*********************ACTIONS***********************/
        private async void SaveAction()
        {
            IsBusy = true;

            if (id == 0)
            {
                ApiResponse response = await new ApiService().PostDataAsync("driver", new DriverModel
                {
                    Name = this.Name,
                    Status = this.Status,
                    Picture = this.Picture,
                    ActualPosition = new PositionModel
                    {
                        Longitude = this.Longitude,
                        Latitude = this.Latitude
                    }
                });
                if (response == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Driver App", "Error", "Ok");
                    return;
                }
                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Driver App", response.Message, "Ok");
                    return;
                }
                DriversViewModel.GetInstance().RefreshDrivers();
                await Application.Current.MainPage.DisplayAlert("Driver App", response.Message, "Ok");
            }
            else
            {
                ApiResponse response = await new ApiService().PutDataAsync("driver", new DriverModel
                {
                    Id = id,
                    Name = this.Name,
                    Status = this.Status,
                    Picture = this.Picture,
                    ActualPosition = new PositionModel
                    {
                        Longitude = this.Longitude,
                        Latitude = this.Latitude
                    }
                });
                if (response == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Driver App", "Error", "Ok");
                    return;
                }
                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Driver App", response.Message, "Ok");
                    return;
                }
                DriversViewModel.GetInstance().RefreshDrivers();
                await Application.Current.MainPage.DisplayAlert("Driver App", response.Message, "Ok");
            }

            IsBusy = false;
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void GetLocationAction()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException)
            {
                // Handle permission exception
            }
            catch (Exception)
            {
                // Unable to get location
            }
        }

        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
