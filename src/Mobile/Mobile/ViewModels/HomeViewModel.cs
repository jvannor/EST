using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Mobile.Utilities;
using Mobile.Models;
using Mobile.ServiceContracts;
using System.Linq;
using System.Collections.ObjectModel;

namespace Mobile.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        #region Commands

        public Command RefreshCommand => new Command(ExecuteRefreshCommand);

        public Command NewReportCommand => new Command(ExecuteNewReportCommand);

        #endregion

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

            Init();
        }

        private async void Init()
        {
            SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
        }

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

        public async void ExecuteRefreshCommand()
        {
            SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
            IsRefreshing = false;
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
