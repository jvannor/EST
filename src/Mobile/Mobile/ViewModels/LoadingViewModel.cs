using System;
using System.Diagnostics;
using Xamarin.Forms;
using Mobile.ServiceContracts;

namespace Mobile.ViewModels
{
    internal class LoadingViewModel : ViewModelBase
    {
        #region Commands

        public Command AppearingCommand => new Command(ExecuteAppearingCommand);

        #endregion

        #region Methods

        public LoadingViewModel(ISettingsService settings, IAuthenticationService authentication) : base(settings)
        {
            authenticationService = authentication;
        }

        public async void ExecuteAppearingCommand()
        {
            try
            {
                var authenticated = await authenticationService.Authenticated();
                if (authenticated)
                {
                    await Shell.Current.GoToAsync("//home");
                }
                else
                {
                    await Shell.Current.GoToAsync("//login");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"LoadingViewModel::ExecuteAppearingCommand() encountered an unexpected exception, {ex.GetType().Name}; {ex.Message}");
            }
        }

        #endregion

        #region Fields

        private IAuthenticationService authenticationService;

        #endregion
    }
}
