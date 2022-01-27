using System;
using System.Diagnostics;
using Xamarin.Forms;
using EST.ServiceContracts;

namespace EST.ViewModels
{
    public sealed class LoadingViewModel : ViewModelBase
    {
        #region Methods

        public LoadingViewModel(
            IAuthenticationService authenticationService,
            IDialogService dialogService,
            ISettingsService settingsService) : base(authenticationService, dialogService, settingsService)
        {
            Title = "Loading";
        }

        #endregion

        #region Commands

        public Command AppearingCommand => new Command(ExecuteAppearingCommand);

        public async void ExecuteAppearingCommand()
        {
            IsBusy = true;

            try
            {
                var authenticated = await authenticationService.Authenticated();
                if (authenticated)
                {
                    await Shell.Current.GoToAsync("//Home");
                }
                else
                {
                    await Shell.Current.GoToAsync("//Login");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"LoadingViewModel::ExecuteAppearingCommand() encountered an unexpected exception, {ex.GetType().Name}; {ex.Message}");
                throw ex;
            }

            IsBusy = false;
        }

        #endregion
    }
}
