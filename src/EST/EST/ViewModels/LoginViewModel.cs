using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using EST.ServiceContracts;

namespace EST.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        #region Commands

        public Command LoginCommand => new Command(OnLogin);

        #endregion

        #region Methods

        public LoginViewModel(ISettingsService settings, IAuthenticationService authentication) : base(settings)
        {
            Title = "Login";
            authenticationService = authentication;
        }

        public async void OnLogin()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    var success = await authenticationService.Login();
                    if (success)
                    {
                        await Shell.Current.GoToAsync("//Home");
                    }

                    IsBusy = false;
                }
            }
            catch(Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine($"LoginViewModel::OnLogin() experienced an unexpected exception, {ex.GetType().Name}; {ex.Message}");
            }
        }

        #endregion

        #region Fields

        private IAuthenticationService authenticationService;

        #endregion
    }
}
