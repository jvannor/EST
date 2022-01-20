using System;
using Mobile.ServiceContracts;
using Xamarin.Forms;

namespace Mobile.ViewModels
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
            UserName = settingsService.UserName;

            this.authenticationService = authenticationService;
        }

        public async void ExecuteLogoutCommand()
        {
            await authenticationService.Logout();
            await Shell.Current.GoToAsync("//Login");
        }

        #endregion

        #region Fields

        private IAuthenticationService authenticationService;
        private string userName;

        #endregion
    }
}
