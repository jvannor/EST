using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Web;
using Mobile.Models;
using Mobile.ServiceContracts;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    internal class TemplateDetailViewModel : ViewModelBase, IQueryAttributable
    {
        #region Commands

        public Command GoToTagsCommand => new Command(ExecuteGoToTagsCommand);

        public Command SaveCommand => new Command(ExecuteSaveCommand);

        public Command DeleteCommand => new Command(ExecuteDeleteCommand);

        #endregion

        #region Properties

        public ReportTemplate ReportTemplate
        {
            get { return reportTemplate; }
            set { SetProperty(ref reportTemplate, value); }
        }

        #endregion

        #region Methods

        public TemplateDetailViewModel(
            ISettingsService settingsService,
            IDialogService dialogService) : base(settingsService)
        {
            Title = "Template";
            this.dialogService = dialogService;

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

        public async void ExecuteGoToTagsCommand(object parameter)
        {
            
            var json = JsonSerializer.Serialize(ReportTemplate.Content.Tags);
            var encoded = HttpUtility.UrlEncode(json);
            await Shell.Current.GoToAsync($"TemplateDetailTags?SelectedTags={encoded}");
        }

        public async void ExecuteDeleteCommand(object parameter)
        {
            var confirm = await dialogService.InputBox("Confirmation", "Are you sure that you want to delete this item?", "Yes", "no");
            if (confirm)
            {
                MessagingCenter.Send(this, "DeleteReportTemplate", ReportTemplate);
                await Shell.Current.GoToAsync("..?");
            }
        }

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

        private IDialogService dialogService;
        private ReportTemplate reportTemplate;

        #endregion
    }
}
