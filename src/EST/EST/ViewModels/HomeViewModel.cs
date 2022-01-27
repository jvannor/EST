using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using EST.Utilities;
using EST.Models;
using EST.ServiceContracts;
using System.Linq;

namespace EST.ViewModels
{
    public sealed class HomeViewModel : ViewModelBase
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

        public HomeViewModel(
            IAuthenticationService authenticationService,
            IDialogService dialogService,
            ISettingsService settingsService,
            IReportsDataService reportDataService) : base(authenticationService, dialogService, settingsService)
        {
            Title = "Epileptic Seizure Tracker";
            this.reportDataService = reportDataService;
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
                Debug.WriteLine($"HomeViewModel::ExecuteAppearingCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsBusy = false;
        }

        public Command NewReportCommand => new Command(ExecuteNewReportCommand);

        public async void ExecuteNewReportCommand(object parameter)
        {
            var template = parameter as ReportTemplate;
            if (template != null)
            {
                var timeStamp = DateTime.Now.ToLocalTime();
                var report = new Report
                {
                    Id = string.Empty,
                    ReportType = "Seizure Report",
                    Author = await authenticationService.GetAuthor(),
                    Subject = await authenticationService.GetSubject(),
                    Revision = 1,
                    Created = timeStamp,
                    Modified = timeStamp,
                    Observed = timeStamp,
                    Category = template.Content.Category,
                    Subcategory = template.Content.Subcategory,
                    Detail = template.Content.Detail,
                    Description = string.Empty,
                    Tags = new ObservableCollection<string>(template.Content.Tags)
                };

                var reportJson = JsonSerializer.Serialize(report);
                var encodedReport = HttpUtility.UrlEncode(reportJson);
                await Shell.Current.GoToAsync($"ReportDetail?Report={encodedReport}");
            }
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
            catch(Exception ex)
            {
                Debug.WriteLine($"HomeViewModel::ExecuteRefreshCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsRefreshing = false;
            IsBusy = false;
        }

        #endregion

        #region Fields

        private readonly IReportsDataService reportDataService;
        private bool isRefreshing;
        private ObservableCollection<ReportTemplate> reportTemplates;

        #endregion
    }
}
