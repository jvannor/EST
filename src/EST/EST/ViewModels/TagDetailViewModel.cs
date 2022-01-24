using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Web;
using Xamarin.Forms;
using EST.Models;
using EST.ServiceContracts;


namespace EST.ViewModels
{
    internal class TagDetailViewModel : ViewModelBase, IQueryAttributable
    {
        #region Commands

        public Command CancelCommand => new Command(ExecuteCancelCommand);
        public Command DeleteCommand => new Command(ExecuteDeleteCommand);
        public Command UpdateCommand => new Command(ExecuteUpdateCommand);

        #endregion

        #region Properties

        public string Tag
        {
            get { return tag2; }
            set { SetProperty(ref tag2, value); }
        }

        #endregion

        #region Methods

        public TagDetailViewModel(ISettingsService settingsService, IDialogService dialogService) : base(settingsService)
        {
            Title = "Tag";
            this.dialogService = dialogService;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                var decoded = HttpUtility.UrlDecode(query["Tag"]);
                tag1 = Tag = decoded;
                IsBusy = false;
            }
        }

        private async void ExecuteCancelCommand()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Shell.Current.GoToAsync("..?");
                IsBusy = false;
            }
        }

        private async void ExecuteDeleteCommand()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                var confirm = await dialogService.InputBox("Confirmation", "Are you sure that you want to delete this item?", "Yes", "No");
                if (confirm)
                {
                    MessagingCenter.Send(this, "DeleteTag", tag1);
                    await Shell.Current.GoToAsync("..?");
                }
                IsBusy = false;
            }
        }

        private async void ExecuteUpdateCommand()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                MessagingCenter.Send(this, "UpdateTag", (tag1, tag2));
                await Shell.Current.GoToAsync("..?");
                IsBusy = false;
            }
        }

        #endregion

        #region Fields

        private IDialogService dialogService;
        private string tag1;
        private string tag2;

        #endregion
    }
}
