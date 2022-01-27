using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Web;
using Xamarin.Forms;
using EST.Models;
using EST.ServiceContracts;

namespace EST.ViewModels
{
    public sealed class TemplateDetailViewModel : ViewModelBase, IQueryAttributable
    {
        #region Properties

        public ReportTemplate ReportTemplate
        {
            get { return reportTemplate; }
            set { SetProperty(ref reportTemplate, value); }
        }

        #endregion

        #region Methods

        public TemplateDetailViewModel(
            IAuthenticationService authenticationService,
            IDialogService dialogService,
            ISettingsService settingsService) : base(authenticationService, dialogService, settingsService)
        {
            Title = "Template";
            MessagingCenter.Subscribe<TemplateDetailTagsViewModel, IEnumerable<string>>(this, "UpdateTags", ExecuteUpdateTags);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey("Template"))
            {
                if (!string.IsNullOrEmpty(query["Template"]))
                {
                    var decoded = HttpUtility.UrlDecode(query["Template"]);
                    ReportTemplate = JsonSerializer.Deserialize<ReportTemplate>(decoded);
                }
                else
                {
                    ReportTemplate = new ReportTemplate();
                }
            }
        }

        #endregion

        #region Commands

        public Command DeleteCommand => new Command(ExecuteDeleteCommand);

        public async void ExecuteDeleteCommand(object parameter)
        {
            var confirm = await dialogService.InputBox("Confirmation", "Are you sure that you want to delete this item?", "Yes", "no");
            if (confirm)
            {
                MessagingCenter.Send(this, "DeleteReportTemplate", ReportTemplate);
                await Shell.Current.GoToAsync("..?");
            }
        }

        public Command GoToTagsCommand => new Command(ExecuteGoToTagsCommand);

        public async void ExecuteGoToTagsCommand(object parameter)
        {
            var json = JsonSerializer.Serialize(ReportTemplate.Content.Tags);
            var encoded = HttpUtility.UrlEncode(json);
            await Shell.Current.GoToAsync($"TemplateDetailTags?SelectedTags={encoded}");
        }

        public Command SaveCommand => new Command(ExecuteSaveCommand);

        public async void ExecuteSaveCommand(object parameter)
        {
            MessagingCenter.Send(this, "SaveReportTemplate", ReportTemplate);
            await Shell.Current.GoToAsync("..?");
        }

        public void ExecuteUpdateTags(TemplateDetailTagsViewModel model, IEnumerable<string> tags)
        {
             ReportTemplate.Content.Tags = new ObservableCollection<string>(tags);
        }

        #endregion

        #region Fields

        private ReportTemplate reportTemplate;

        #endregion
    }
}
