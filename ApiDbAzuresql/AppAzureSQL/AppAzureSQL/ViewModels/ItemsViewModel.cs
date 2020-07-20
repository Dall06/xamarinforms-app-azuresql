using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AppAzureSQL.Models;
using AppAzureSQL.Views;
using AppAzureSQL.Services;

namespace AppAzureSQL.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<DriverModel> Drivers { get; set; }
        public Command LoadDriversCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Drivers";
            Drivers = new ObservableCollection<DriverModel>();
            LoadDriversCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<NewItemPage, DriverModel>(this, "AddItem", async (obj, item) =>
            //{
            //    var newDriver = item as DriverModel;
            //    Drivers.Add(newDriver);
            //    await DataStore.AddItemAsync(newItem);
            //});
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Drivers.Clear();
                var response = await new ApiService().GetDataAsync<DriverModel>("driver");

                if (response != null && response.Result != null) 
                {
                    Drivers = (ObservableCollection<DriverModel>)response.Result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}