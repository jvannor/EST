using System;
using System.Diagnostics;
using System.Web;
using System.Text.Json;
using System.Linq;
using Xamarin.Forms;
using EST.Models;
using EST.ServiceContracts;

namespace EST.ViewModels
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

        public TemplatesViewModel(
            ISettingsService settingsService,
            ISettingsDocumentService settingsDocumentService) : base(settingsService)
        {
            Title = "Templates";
            this.settingsDocumentService = settingsDocumentService;

            MessagingCenter.Subscribe<TemplateDetailViewModel, ReportTemplate>(this, "DeleteReportTemplate", ExecuteDeleteReportTemplate);
            MessagingCenter.Subscribe<TemplateDetailViewModel, ReportTemplate>(this, "SaveReportTemplate", ExecuteSaveReportTemplate);

            Init();
        }

        private async void Init()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
                    IsBusy = false;
                }
            }
            catch(Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine($"TemplatesViewModel::Init() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }
        }

        public async void ExecuteEditCommand(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;

                var json = JsonSerializer.Serialize(parameter);
                var encoded = HttpUtility.UrlEncode(json);
                await Shell.Current.GoToAsync($"TemplateDetail?Template={encoded}");

                IsBusy = false;
            }
        }

        public async void ExecuteNewCommand()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Shell.Current.GoToAsync("TemplateDetail?Template=");
                IsBusy = false;
            }
        }

        public async void ExecuteRefreshCommand()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
                    IsBusy = false;
                }
                catch(Exception ex)
                {
                    IsBusy = false;
                    Debug.WriteLine($"TemplatesViewModel::ExecuteRefreshCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                }

                IsRefreshing = false;
            }
        }

        public async void ExecuteDeleteReportTemplate(TemplateDetailViewModel mode, ReportTemplate template)
        {
            if (!IsBusy)
            {
                IsBusy = true;

                var target = SettingsDocument.Templates.Where(t => t.Title == template.Title).FirstOrDefault();
                if (target != null)
                {
                    try
                    {
                        await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
                        SettingsDocument.Templates.Remove(target);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"TemplatesViewModel::ExecuteDeleteReportTemplate() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                    }
                }

                IsBusy = false;
            }
        }

        public async void ExecuteSaveReportTemplate(TemplateDetailViewModel model, ReportTemplate template)
        {
            if (!IsBusy)
            {
                IsBusy = true;

                var target = SettingsDocument.Templates.Where(t => t.Title == template.Title).FirstOrDefault();
                if (target != null)
                {
                    try
                    {
                        var idx = SettingsDocument.Templates.IndexOf(target);
                        SettingsDocument.Templates[idx] = template;
                        await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"TemplatesViewModel::ExecuteSaveReportTemplate() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                    }
                }
                else
                {
                    try
                    {
                        await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
                        SettingsDocument.Templates.Add(template);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"TemplatesViewModel::ExecuteSaveReportTemplate() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                    }
                }

                IsBusy = false;
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
