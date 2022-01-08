using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Mobile.Models;
using Mobile.ServiceContracts;

namespace Mobile.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        public Command TestCommand => new Command(ExecuteTestCommand);

        public HomeViewModel(ISettingsService settings) : base(settings)
        {
            System.Diagnostics.Debug.WriteLine("HomeViewModel::ctor()");
            Title = "Home";

            MessagingCenter.Subscribe<ReportDetailViewModel, Report>(this, "CreateReport", ExecuteCreateReport);
        }

        public async void ExecuteTestCommand()
        {
            System.Diagnostics.Debug.WriteLine("HomeViewModel::ExecuteTestCommand()");
            await Shell.Current.GoToAsync("reportdetail");
        }

        public async void ExecuteCreateReport(ReportDetailViewModel model, Report report)
        {
        }
    }
}
