using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Mobile.Utilities;
using Mobile.Models;
using Mobile.ServiceContracts;

namespace Mobile.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        #region Properties

        public Command NewReportCommand => new Command(ExecuteNewReportCommand);

        #endregion

        #region Methods

        public HomeViewModel(ISettingsService ss, IReportsDataService rds) : base(ss)
        {
            Title = "Home";
            reportDataService = rds;
        }

        public async void ExecuteNewReportCommand()
        {
            var userName = settingsService.UserName;
            var timestamp = DateTime.Now.ToLocalTime();

            var report = Extensions.DeepCopy<Report>(settingsService.DefaultReportTemplate);
            report.Author = report.Subject = settingsService.UserName;
            report.Created = report.Modified = report.Observed = DateTime.Now.ToLocalTime();

            var reportJson = JsonSerializer.Serialize(report);
            var encodedReport = HttpUtility.UrlEncode(reportJson);
            await Shell.Current.GoToAsync($"reportdetail?report={encodedReport}");
        }

        #endregion

        #region Fields

        private IReportsDataService reportDataService;

        #endregion
    }
}
