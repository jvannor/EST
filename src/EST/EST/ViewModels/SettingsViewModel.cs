using System;
using System.Diagnostics;
using Xamarin.Forms;
using EST.ServiceContracts;

namespace EST.ViewModels
{
    internal class SettingsViewModel :ViewModelBase
    {
        #region Commands

        public Command LogoutCommand => new Command(ExecuteLogoutCommand);

        #endregion

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

        public async void ExecuteLogoutCommand()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    await authenticationService.Logout();
                    await Shell.Current.GoToAsync("//Login");

                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
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
