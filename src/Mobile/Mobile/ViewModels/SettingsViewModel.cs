using System;
using Mobile.ServiceContracts;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    internal class SettingsViewModel :ViewModelBase
    {
        public Command LogoutCommand => new Command(ExecuteLogoutCommand);

        public SettingsViewModel(IAuthenticationService authentication)
        {
            System.Diagnostics.Debug.WriteLine("SettingsViewModel::ctor()");
            authenticationService = authentication;
            Title = "General Settings";
        }

        public async void ExecuteLogoutCommand()
        {
            await authenticationService.Logout();
            await Shell.Current.GoToAsync("//login");
        }

        private IAuthenticationService authenticationService;
    }
}
