using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Web;
using Mobile.Models;
using Mobile.ServiceContracts;
using Mobile.Utilities;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    internal class ReportsViewModel : ViewModelBase
    {
        #region Properties

        public Command GoToDetailsCommand => new Command(ExecuteGoToDetailsCommand);
        public Command LoadReportsCommand => new Command(ExecuteLoadReportsCommand);
        public Command RefreshReportsCommand => new Command(ExecuteRefreshReportsCommand);


        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }

            set
            {
                SetProperty(ref isRefreshing, value);
            }
        }

        public ObservableCollection<Report> Reports
        {
            get
            {
                return reports;
            }

            set
            {
                SetProperty(ref reports, value);
            }
        }

        public int ReportThreshold
        {
            get
            {
                return reportThreshold;
            }

            set
            {
                SetProperty(ref reportThreshold, value);
            }
        }

        public Command ReportThresholdReachedCommand => new Command(ExecuteReportThresholdReachedCommand);

        #endregion

        #region Methods

        public ReportsViewModel(ISettingsService ss, IReportsDataService rds) : base(ss)
        {
            IsBusy = false;
            isRefreshing = false;
            reportsDataService = rds;
            reports = new ObservableCollection<Report>();
            reportThreshold = 1;
            Title = "Reports";

            Init();

            MessagingCenter.Subscribe<ReportDetailViewModel, Report>(this, "CreateReport", ExecuteCreateReport);
            MessagingCenter.Subscribe<ReportDetailViewModel, Report>(this, "UpdateReport", ExecuteUpdateReport);
            MessagingCenter.Subscribe<ReportDetailViewModel, string>(this, "DeleteReport", ExecuteDeleteReport);
        }

        public async void Init()
        {
            var reports = await reportsDataService.GetReports(settingsService.UserName, 0, 10);
            foreach (var report in reports)
            {
                report.Created = report.Created.ToLocalTime();
                report.Modified = report.Modified.ToLocalTime();
                report.Observed = report.Observed.ToLocalTime();

                Reports.Add(report);
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

        public async void ExecuteGoToDetailsCommand(object parameter)
        {
            var report = parameter as Report;
            if (report != null)
            {
                var reportJson = JsonSerializer.Serialize(report);
                var reportString = HttpUtility.UrlEncode(reportJson);
                await Shell.Current.GoToAsync($"reportdetail?report={reportString}");
            }
        }

        public async void ExecuteLoadReportsCommand()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    ReportThreshold = 1;
                    Reports.Clear();

                    var reports = await reportsDataService.GetReports(settingsService.UserName, 0, 10);
                    foreach(var report in reports)
                    {
                        report.Created = report.Created.ToLocalTime();
                        report.Modified = report.Modified.ToLocalTime();
                        report.Observed = report.Observed.ToLocalTime();

                        Reports.Add(report);
                    }

                    IsBusy = false;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"ReportsViewModel::ExecuteLoadReportsCommand Exception: {ex.GetType().Name} - {ex.Message}");
            }
        }

        public async void ExecuteRefreshReportsCommand()
        {
            ExecuteLoadReportsCommand();
            IsRefreshing = false;
        }

        public async void ExecuteReportThresholdReachedCommand()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    var reports = await reportsDataService.GetReports(settingsService.UserName, Reports.Count / 10, 10);
                    if ((reports.Count() == 0) || (reports.Last().Id == Reports.Last().Id))
                    {
                        ReportThreshold = -1;
                    }
                    else
                    {
                        foreach(var report in reports)
                        {
                            report.Created = report.Created.ToLocalTime();
                            report.Modified = report.Modified.ToLocalTime();
                            report.Observed = report.Observed.ToLocalTime();

                            Reports.Add(report);
                        }
                    }

                    IsBusy = false;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"ReportsViewModel::ExecuteReportThresholdReachedCommand Exception: {ex.GetType().Name} - {ex.Message}");
            }
        }

        public async void ExecuteUpdateReport(ReportDetailViewModel mode, Report report)
        {
            var created = report.Created;
            var modified = report.Modified;
            var observed = report.Observed;

            report.Created = created.ToUniversalTime();
            report.Modified = modified.ToUniversalTime();
            report.Observed = observed.ToUniversalTime();
            await reportsDataService.UpdateReport(report);

            var temp = Reports.FirstOrDefault<Report>(r => report.Id == r.Id);
            temp.Observed = observed;
            temp.Category = report.Category;
            temp.Subcategory = report.Subcategory;
            temp.Detail = report.Detail;
            temp.Description = report.Description;
            temp.Tags = new ObservableCollection<string>(report.Tags);
        }

        #endregion

        #region Fields

        private bool isRefreshing;
        private IReportsDataService reportsDataService;
        private ObservableCollection<Report> reports;
        private int reportThreshold;

        #endregion
    }
}
