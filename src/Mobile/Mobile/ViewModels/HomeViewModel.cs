using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        public Command TestCommand => new Command(ExecuteTestCommand);

        public HomeViewModel()
        {
            System.Diagnostics.Debug.WriteLine("HomeViewModel::ctor()");
            Title = "Home";
        }

        public async void ExecuteTestCommand()
        {
            System.Diagnostics.Debug.WriteLine("HomeViewModel::ExecuteTestCommand()");
            await Shell.Current.GoToAsync("reportdetail");
        }
    }
}
