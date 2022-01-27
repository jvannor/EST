using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using EST.Models;
using EST.ServiceContracts;
using EST.Utilities;

namespace EST.ViewModels
{
    public sealed class ReportsViewModel : ViewModelBase
    {
        #region Properties

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        public ObservableCollection<Report> Reports
        {
            get { return reports; }
            set { SetProperty(ref reports, value); }
        }

        public int ReportThreshold
        {
            get { return reportThreshold; }
            set { SetProperty(ref reportThreshold, value); }
        }

        #endregion

        #region Methods

        public ReportsViewModel(
            IAuthenticationService authenticationService,
            IDialogService dialogService,
            ISettingsService settingsService,
            IReportsDataService reportsDataService) : base(authenticationService, dialogService, settingsService)
        {
            Title = "Reports";
            Reports = new ObservableCollection<Report>();
            ReportThreshold = 1;

            this.reportsDataService = reportsDataService;

            MessagingCenter.Subscribe<ReportDetailViewModel, Report>(this, "CreateReport", ExecuteCreateReport);
            MessagingCenter.Subscribe<ReportDetailViewModel, Report>(this, "UpdateReport", ExecuteUpdateReport);
            MessagingCenter.Subscribe<ReportDetailViewModel, string>(this, "DeleteReport", ExecuteDeleteReport);
        }

        private async Task ExecuteLoadReportsCommand()
        {
            try
            {
                Reports.Clear();
                ReportThreshold = 1;

                var reports = await reportsDataService.GetReports(0, 10);
                foreach (var report in reports)
                {
                    report.Created = report.Created.ToLocalTime();
                    report.Modified = report.Modified.ToLocalTime();
                    report.Observed = report.Observed.ToLocalTime();

                    Reports.Add(report);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReportsViewModel::ExecuteLoadReportsCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                throw ex;
            }
        }

        public async void ExecuteCreateReport(ReportDetailViewModel model, Report report)
        {
            var temp = Reports.Union<Report>(new[] { report })
                .GroupBy(r => r.Id)
                .Select(group => group.FirstOrDefault())
                .OrderByDescending(r => r.Observed);

            Reports = new ObservableCollection<Report>(temp);
        }

        public async void ExecuteDeleteReport(ReportDetailViewModel model, string parameter)
        {
            Reports.Remove(Reports.FirstOrDefault(target => target.Id == parameter));
        }

        public async void ExecuteUpdateReport(ReportDetailViewModel mode, Report report)
        {
            try
            {
                var target = Reports.Where(r => r.Id == report.Id).FirstOrDefault();

                if (target != null)
                {
                    var created = report.Created;
                    report.Created = created.ToUniversalTime();

                    var modified = report.Modified;
                    report.Modified = modified.ToUniversalTime();

                    var observed = report.Observed;
                    report.Observed = report.Observed.ToUniversalTime();

                    await reportsDataService.UpdateReport(report);

                    target.Created = created;
                    target.Modified = modified;
                    target.Observed = observed;
                    target.Category = report.Category;
                    target.Subcategory = report.Subcategory;
                    target.Detail = report.Detail;
                    target.Description = report.Description;
                    target.Tags = new ObservableCollection<string>(report.Tags);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReportsViewModel::ExecuteUpdateReport() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }
        }

        #endregion

        #region Commands

        public Command AppearingCommand => new Command(ExecuteAppearingCommand);

        public async void ExecuteAppearingCommand()
        {
            IsBusy = true;

            if (Reports.Count == 0)
            {
                await ExecuteLoadReportsCommand();
            }

            IsBusy = false;
        }

        public Command RefreshReportsCommand => new Command(ExecuteRefreshReportsCommand);

        public async void ExecuteRefreshReportsCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            { 
                await ExecuteLoadReportsCommand();
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"ReportsViewModel::ExecuteRefreshReportsCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsRefreshing = false;
            IsBusy = false;
        }

        public Command ReportThresholdReachedCommand => new Command(ExecuteReportThresholdReachedCommand);

        public async void ExecuteReportThresholdReachedCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var reports = await reportsDataService.GetReports(Reports.Count / 10, 10);
                if ((reports.Count() == 0) || (reports.Last().Id == Reports.Last().Id))
                {
                    ReportThreshold = -1;
                }
                else
                {
                    foreach (var report in reports)
                    {
                        report.Created = report.Created.ToLocalTime();
                        report.Modified = report.Modified.ToLocalTime();
                        report.Observed = report.Observed.ToLocalTime();

                        Reports.Add(report);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReportsViewModel::ExecuteReportThresholdReachedCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsBusy = false;
        }

        public Command ReportDetailCommand => new Command(ExecuteReportDetailCommand);

        public async void ExecuteReportDetailCommand(object parameter)
        {
            var report = parameter as Report;
            if (report != null)
            {
                var reportJson = JsonSerializer.Serialize(report);
                var reportString = HttpUtility.UrlEncode(reportJson);
                await Shell.Current.GoToAsync($"ReportDetail?Report={reportString}");
            }
        }

        #endregion

        #region Fields

        private readonly IReportsDataService reportsDataService;
        private bool isRefreshing;
        private int reportThreshold;
        private ObservableCollection<Report> reports;

        #endregion
    }
}
