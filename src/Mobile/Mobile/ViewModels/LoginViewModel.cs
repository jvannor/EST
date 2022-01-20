using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using Mobile.ServiceContracts;

namespace Mobile.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        #region Commands

        public Command LoginCommand => new Command(OnLogin);

        #endregion

        #region Methods

        public LoginViewModel(ISettingsService settings, IAuthenticationService authentication) : base(settings)
        {
            authenticationService = authentication;
        }

        public async void OnLogin()
        {
            try
            {
                var success = await authenticationService.Login();
                if (success)
                {
                    await Shell.Current.GoToAsync("//Home");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"LoginViewModel::OnLogin() experienced an unexpected exception, {ex.GetType().Name}; {ex.Message}");
            }
        }

        #endregion

        #region Fields

        private IAuthenticationService authenticationService;

        #endregion
    }
}
