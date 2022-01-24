using System;
using System.Diagnostics;
using Xamarin.Forms;
using EST.ServiceContracts;

namespace EST.ViewModels
{
    internal class LoadingViewModel : ViewModelBase
    {
        #region Commands

        public Command AppearingCommand => new Command(ExecuteAppearingCommand);

        #endregion

        #region Methods

        public LoadingViewModel(ISettingsService settings, IAuthenticationService authentication) : base(settings)
        {
            Title = "Loading";
            authenticationService = authentication;
        }

        public async void ExecuteAppearingCommand()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    var authenticated = await authenticationService.Authenticated();
                    if (authenticated)
                    {
                        await Shell.Current.GoToAsync("//Home");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("//Login");
                    }

                    IsBusy = false;
                }
            }
            catch(Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine($"LoadingViewModel::ExecuteAppearingCommand() encountered an unexpected exception, {ex.GetType().Name}; {ex.Message}");
            }
        }

        #endregion

        #region Fields

        private IAuthenticationService authenticationService;

        #endregion
    }
}
