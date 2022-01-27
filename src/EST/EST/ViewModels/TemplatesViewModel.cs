using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using EST.Models;
using EST.ServiceContracts;

namespace EST.ViewModels
{
    public sealed class TemplatesViewModel : ViewModelBase
    {
        #region Properties

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        public ObservableCollection<ReportTemplate> ReportTemplates
        {
            get { return reportTemplates; }
            set { SetProperty(ref reportTemplates, value); }
        }

        #endregion

        #region Methods

        public TemplatesViewModel(
            IAuthenticationService authenticationService,
            IDialogService dialogService,
            ISettingsService settingsService) : base(authenticationService, dialogService, settingsService)
        {
            Title = "Templates";

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
                var target = ReportTemplates.Where(t => t.Title == template.Title).FirstOrDefault();
                if (target != null)
                {
                    ReportTemplates.Remove(target);
                    await settingsService.SetReportTemplates(ReportTemplates);
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
                var target = ReportTemplates.Where(t => t.Title == template.Title).FirstOrDefault();
                if (target != null)
                {
                    var idx = ReportTemplates.IndexOf(target);
                    ReportTemplates[idx] = template;
                    await settingsService.SetReportTemplates(reportTemplates);
                }
                else
                {
                    ReportTemplates.Add(template);
                    await settingsService.SetReportTemplates(reportTemplates);
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
                if (ReportTemplates == null)
                {
                    ReportTemplates = new ObservableCollection<ReportTemplate>(await settingsService.GetReportTemplates());
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
                ReportTemplates = new ObservableCollection<ReportTemplate>(await settingsService.GetReportTemplates());
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

        private ObservableCollection<ReportTemplate> reportTemplates;
        private bool isRefreshing;

        #endregion
    }
}
