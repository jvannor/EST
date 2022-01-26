using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using EST.ServiceContracts;

namespace EST.ViewModels
{
    public sealed class LoginViewModel : ViewModelBase
    {
        #region Methods

        public LoginViewModel(ISettingsService settings, IAuthenticationService authenticationService, IDialogService dialogService) : base(settings)
        {
            Title = "Login";
            this.authenticationService = authenticationService;
            this.dialogService = dialogService;
        }

        #endregion

        #region Commands

        public Command LoginCommand => new Command(ExecuteLoginCommand);

        public async void ExecuteLoginCommand()
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
                await dialogService.MessageBox("Error", "Login failed", "OK");
            }
        }

        #endregion

        #region Fields

        private IAuthenticationService authenticationService;
        private IDialogService dialogService;

        #endregion
    }
}
