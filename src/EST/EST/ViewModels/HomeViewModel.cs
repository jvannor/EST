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

        public SettingsDocument SettingsDocument
        {
            get { return settingsDocument; }
            set { SetProperty(ref settingsDocument, value); }
        }

        #endregion

        #region Methods

        public HomeViewModel(
            ISettingsService settingsService,
            ISettingsDocumentService settingsDocumentService,
            IReportsDataService reportDataService) : base(settingsService)
        {
            Title = "Epileptic Seizure Tracker";
            this.settingsDocumentService = settingsDocumentService;
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
                if (SettingsDocument == null)
                {
                    SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
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
                    Author = settingsService.UserName,
                    Subject = settingsService.UserName,
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
                SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
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

        private ISettingsDocumentService settingsDocumentService;
        private IReportsDataService reportDataService;
        private bool isRefreshing;
        private SettingsDocument settingsDocument;

        #endregion
    }
}
