using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Mobile.Models;
using Mobile.ServiceContracts;
using System.Web;

namespace Mobile.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        public Command TestCommand => new Command(ExecuteTestCommand);
        public Command NewReportCommand => new Command(ExecuteNewReportCommand);

        public HomeViewModel(ISettingsService ss, IReportsDataService rds) : base(ss)
        {
            Title = "Home";
            reportDataService = rds;

            MessagingCenter.Subscribe<ReportDetailViewModel, Report>(this, "CreateReport", ExecuteCreateReport);
        }

        public async void ExecuteTestCommand()
        {
            await Shell.Current.GoToAsync("reportdetail");
        }

        public async void ExecuteNewReportCommand()
        {
            var userName = settingsService.UserName;
            var timestamp = DateTime.Now.ToLocalTime();

            var report = new Report(settingsService.DefaultReportTemplate);
            report.Author = report.Subject = settingsService.UserName;
            report.Created = report.Modified = report.Observed = DateTime.Now.ToLocalTime();

            var reportJson = JsonSerializer.Serialize(report);
            var encodedReport = HttpUtility.UrlEncode(reportJson);
            await Shell.Current.GoToAsync($"reportdetail?report={encodedReport}");
        }

        public async void ExecuteCreateReport(ReportDetailViewModel model, Report report)
        {
            await reportDataService.CreateReport(report);
        }

        private IReportsDataService reportDataService;
    }
}
