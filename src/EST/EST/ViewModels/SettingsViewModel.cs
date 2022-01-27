using System;
using System.Diagnostics;
using Xamarin.Forms;
using EST.ServiceContracts;

namespace EST.ViewModels
{
    public sealed class SettingsViewModel :ViewModelBase
    {
        #region Properties

        public string Author
        {
            get { return author; }
            set { SetProperty(ref author, value); }
        }

        public string Subject
        {
            get { return subject; }
            set { SetProperty(ref subject, value); }
        }

        #endregion

        #region Methods

        public SettingsViewModel(
            IAuthenticationService authenticationService,
            IDialogService dialogService,
            ISettingsService settingsService) : base(authenticationService, dialogService, settingsService)
        {
            Title = "General";
        }

        #endregion

        #region Commands

        public Command AppearingCommand => new Command(ExecuteAppearingCommand);

        public async void ExecuteAppearingCommand()
        {
            Author = await authenticationService.GetAuthor();
            Subject = await authenticationService.GetSubject();
        }

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

        private string author;
        private string subject;

        #endregion
    }
}
