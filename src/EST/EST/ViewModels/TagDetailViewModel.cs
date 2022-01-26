using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Web;
using Xamarin.Forms;
using EST.Models;
using EST.ServiceContracts;


namespace EST.ViewModels
{
    public sealed class TagDetailViewModel : ViewModelBase, IQueryAttributable
    {
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
            var decoded = HttpUtility.UrlDecode(query["Tag"]);
            tag1 = Tag = decoded;
        }

        #endregion

        #region Commands

        public Command CancelCommand => new Command(ExecuteCancelCommand);

        public async void ExecuteCancelCommand()
        {
            await Shell.Current.GoToAsync("..?");
        }

        public Command DeleteCommand => new Command(ExecuteDeleteCommand);

        public async void ExecuteDeleteCommand()
        {
            var confirm = await dialogService.InputBox("Confirmation", "Are you sure that you want to delete this item?", "Yes", "No");
            if (confirm)
            {
                MessagingCenter.Send(this, "DeleteTag", tag1);
                await Shell.Current.GoToAsync("..?");
            }
        }

        public Command UpdateCommand => new Command(ExecuteUpdateCommand);

        public async void ExecuteUpdateCommand()
        {
            MessagingCenter.Send(this, "UpdateTag", (tag1, tag2));
            await Shell.Current.GoToAsync("..?");
        }

        #endregion

        #region Fields

        private IDialogService dialogService;
        private string tag1;
        private string tag2;

        #endregion
    }
}
