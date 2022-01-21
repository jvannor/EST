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
        #region Commands

        public Command GoToDetailsCommand => new Command(ExecuteGoToDetailsCommand);

        public Command LoadReportsCommand => new Command(ExecuteLoadReportsCommand);

        public Command RefreshReportsCommand => new Command(ExecuteRefreshReportsCommand);

        public Command ReportThresholdReachedCommand => new Command(ExecuteReportThresholdReachedCommand);

        #endregion

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
            ISettingsService settingsService,
            IReportsDataService reportsDataService) : base(settingsService)
        {
            Title = "Reports";
            Reports = new ObservableCollection<Report>();
            ReportThreshold = 1;
            this.reportsDataService = reportsDataService;

            MessagingCenter.Subscribe<ReportDetailViewModel, Report>(this, "CreateReport", ExecuteCreateReport);
            MessagingCenter.Subscribe<ReportDetailViewModel, Report>(this, "UpdateReport", ExecuteUpdateReport);
            MessagingCenter.Subscribe<ReportDetailViewModel, string>(this, "DeleteReport", ExecuteDeleteReport);

            Init();
        }

        public async void Init()
        {
            try
            {
                IsBusy = true;
                var reports = await reportsDataService.GetReports(settingsService.UserName, 0, 10);
                foreach (var report in reports)
                {
                    report.Created = report.Created.ToLocalTime();
                    report.Modified = report.Modified.ToLocalTime();
                    report.Observed = report.Observed.ToLocalTime();

                    Reports.Add(report);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"ReportsViewModel::Init() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void ExecuteCreateReport(ReportDetailViewModel model, Report report)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                var temp = Reports.Union<Report>(new[] { report })
                    .GroupBy(r => r.Id)
                    .Select(group => group.FirstOrDefault())
                    .OrderByDescending(r => r.Observed);

                Reports = new ObservableCollection<Report>(temp);
                IsBusy = false;
            }
        }

        public async void ExecuteDeleteReport(ReportDetailViewModel model, string parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                Reports.Remove(Reports.FirstOrDefault(target => target.Id == parameter));
                IsBusy = false;
            }
        }

        public async void ExecuteGoToDetailsCommand(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;

                var report = parameter as Report;
                if (report != null)
                {
                    var reportJson = JsonSerializer.Serialize(report);
                    var reportString = HttpUtility.UrlEncode(reportJson);
                    await Shell.Current.GoToAsync($"ReportDetail?Report={reportString}");
                }

                IsBusy = false;
            }
        }

        public async void ExecuteLoadReportsCommand()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    ReportThreshold = 1;
                    Reports.Clear();

                    var reports = await reportsDataService.GetReports(settingsService.UserName, 0, 10);
                    foreach (var report in reports)
                    {
                        report.Created = report.Created.ToLocalTime();
                        report.Modified = report.Modified.ToLocalTime();
                        report.Observed = report.Observed.ToLocalTime();

                        Reports.Add(report);
                    }

                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    Debug.WriteLine($"ReportsViewModel::ExecuteLoadReportsCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                }
            }
        }

        public async void ExecuteRefreshReportsCommand()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                ExecuteLoadReportsCommand();
                IsRefreshing = false;
                IsBusy = false;
            }
        }

        public async void ExecuteReportThresholdReachedCommand()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;

                    var reports = await reportsDataService.GetReports(settingsService.UserName, Reports.Count / 10, 10);
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

                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    Debug.WriteLine($"ReportsViewModel::ExecuteReportThresholdReachedCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                }
            }
        }

        public async void ExecuteUpdateReport(ReportDetailViewModel mode, Report report)
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;

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

                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    Debug.WriteLine($"ReportsViewModel::ExecuteUpdateReport() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                }
            }
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
