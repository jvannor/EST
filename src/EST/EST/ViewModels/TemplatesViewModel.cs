using System;
using System.Diagnostics;
using System.Web;
using System.Text.Json;
using System.Linq;
using Xamarin.Forms;
using EST.Models;
using EST.ServiceContracts;
using System.Threading.Tasks;

namespace EST.ViewModels
{
    public sealed class TemplatesViewModel : ViewModelBase
    {
        #region Properties

        public SettingsDocument SettingsDocument
        {
            get { return settingsDocument; }
            set { SetProperty(ref settingsDocument, value); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        #endregion

        #region Methods

        public TemplatesViewModel(
            ISettingsService settingsService,
            ISettingsDocumentService settingsDocumentService) : base(settingsService)
        {
            Title = "Templates";
            this.settingsDocumentService = settingsDocumentService;

            MessagingCenter.Subscribe<TemplateDetailViewModel, ReportTemplate>(this, "DeleteReportTemplate", ExecuteDeleteReportTemplate);
            MessagingCenter.Subscribe<TemplateDetailViewModel, ReportTemplate>(this, "SaveReportTemplate", ExecuteSaveReportTemplate);
        }

        public async void ExecuteDeleteReportTemplate(TemplateDetailViewModel mode, ReportTemplate template)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var target = SettingsDocument.Templates.Where(t => t.Title == template.Title).FirstOrDefault();
                if (target != null)
                {
                    await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
                    SettingsDocument.Templates.Remove(target);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TemplatesViewModel::ExecuteDeleteReportTemplate() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsBusy = false;
        }

        public async void ExecuteSaveReportTemplate(TemplateDetailViewModel model, ReportTemplate template)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
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
                    await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
                    SettingsDocument.Templates.Add(template);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TemplatesViewModel::ExecuteSaveReportTemplate() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsBusy = false;
        }

        #endregion

        #region Commands

        public Command AppearingCommand => new Command(ExecuteAppearingCommand);

        public async void ExecuteAppearingCommand()
        {
            IsBusy = true;

            try
            {
                if (SettingsDocument == null)
                {
                    SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"TemplatesViewModel::ExecuteAppearingCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsBusy = false;
        }

        public Command EditCommand => new Command(ExecuteEditCommand);

        public async void ExecuteEditCommand(object parameter)
        {
            var json = JsonSerializer.Serialize(parameter);
            var encoded = HttpUtility.UrlEncode(json);
            await Shell.Current.GoToAsync($"TemplateDetail?Template={encoded}");
        }

        public Command NewCommand => new Command(ExecuteNewCommand);

        public async void ExecuteNewCommand()
        {
            await Shell.Current.GoToAsync("TemplateDetail?Template=");
        }

        public Command RefreshCommand => new Command(ExecuteRefreshCommand);

        public async void ExecuteRefreshCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TemplatesViewModel::ExecuteRefreshCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsRefreshing = false;
            IsBusy = false;
        }

        #endregion

        #region Fields

        private ISettingsDocumentService settingsDocumentService;
        private SettingsDocument settingsDocument;
        private bool isRefreshing;

        #endregion
    }
}
