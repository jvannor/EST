using System;
using System.Diagnostics;
using Xamarin.Forms;
using EST.ServiceContracts;

namespace EST.ViewModels
{
    public sealed class SettingsViewModel :ViewModelBase
    {
        #region Properties

        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        #endregion

        #region Methods

        public SettingsViewModel(ISettingsService settingsService, IAuthenticationService authenticationService) : base(settingsService)
        {
            Title = "General";

            UserName = settingsService?.UserName;
            this.authenticationService = authenticationService;
        }

        #endregion

        #region Commands

        public Command LogoutCommand => new Command(ExecuteLogoutCommand);

        public async void ExecuteLogoutCommand()
        {
            try
            {
                await authenticationService.Logout();
                await Shell.Current.GoToAsync("//Login");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SettingsViewModel::ExecuteLogoutCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }
        }

        #endregion

        #region Fields

        private IAuthenticationService authenticationService;
        private string userName;

        #endregion
    }
}
