using System;
using Xamarin.Forms;
using Mobile.Models;
using Mobile.ServiceContracts;
using System.Web;
using System.Text.Json;
using System.Linq;

namespace Mobile.ViewModels
{
    internal class TemplatesViewModel : ViewModelBase
    {
        #region Commands

        public Command EditCommand => new Command(ExecuteEditCommand);
        public Command NewCommand => new Command(ExecuteNewCommand);
        public Command RefreshCommand => new Command(ExecuteRefreshCommand);

        #endregion

        #region Properties

        public SettingsDocument SettingsDocument
        {
            get
            {
                return settingsDocument;
            }

            set
            {
                if (settingsDocument != value)
                {
                    settingsDocument = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }

            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        public TemplatesViewModel(ISettingsService settingsService, ISettingsDocumentService settingsDocumentService) : base(settingsService)
        {
            Title = "Templates";
            this.settingsDocumentService = settingsDocumentService;

            MessagingCenter.Subscribe<TemplateDetailViewModel, ReportTemplate>(this, "DeleteReportTemplate", ExecuteDeleteReportTemplate);
            MessagingCenter.Subscribe<TemplateDetailViewModel, ReportTemplate>(this, "SaveReportTemplate", ExecuteSaveReportTemplate);

            Init();
        }

        private async void Init()
        {
            SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
        }

        public async void ExecuteEditCommand(object parameter)
        {
            var json = JsonSerializer.Serialize(parameter);
            var encoded = HttpUtility.UrlEncode(json);
            await Shell.Current.GoToAsync($"TemplateDetail?Template={encoded}");
        }

        public async void ExecuteNewCommand()
        {
            await Shell.Current.GoToAsync("TemplateDetail?Template=");
        }

        public async void ExecuteRefreshCommand()
        {
            if (!IsBusy)
            {
                IsBusy = true;

                SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
                IsRefreshing = false;

                IsBusy = false;
            }
        }

        public async void ExecuteDeleteReportTemplate(TemplateDetailViewModel mode, ReportTemplate template)
        {
            var target = SettingsDocument.Templates.Where(t => t.Title == template.Title).FirstOrDefault();
            if (target != null)
            {
                SettingsDocument.Templates.Remove(target);
                await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
            }
        }

        public async void ExecuteSaveReportTemplate(TemplateDetailViewModel model, ReportTemplate template)
        {
            var target = SettingsDocument.Templates.Where(t => t.Title == template.Title).FirstOrDefault();
            if (target != null)
            {
                var idx = SettingsDocument.Templates.IndexOf(target);
                SettingsDocument.Templates[idx] = template;
                await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
            }
            else
            {
                SettingsDocument.Templates.Add(template);
                await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);

            }
        }

        #endregion

        #region Fields

        private ISettingsDocumentService settingsDocumentService;
        private SettingsDocument settingsDocument;
        private bool isRefreshing;

        #endregion
    }
}
