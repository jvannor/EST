using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mobile.Models;
using Mobile.ServiceContracts;
using System.Web;

namespace Mobile.ViewModels
{
    internal class ReportsViewModel : ViewModelBase
    {
        public Command LoadMoreDataCommand => new Command(ExecuteLoadMoreDataCommand);
        public Command RefreshCommand => new Command(ExecuteRefreshCommand);
        public Command TestCommand => new Command(ExecuteTestCommand);
        public Command GoToDetailsCommand => new Command(ExecuteGoToDetailsCommand);

        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
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
                reports = value;
                OnPropertyChanged();
            }
        }
                     
        public ReportsViewModel(ISettingsService settings, IReportsDataService service) : base(settings)
        {
            Title = "Reports";

            reportsDataService = service;
            userName = settingsService.UserName;

            MessagingCenter.Subscribe<ReportDetailViewModel, Report>(this, "UpdateReport", ExecuteUpdateReport);
            MessagingCenter.Subscribe<ReportDetailViewModel, Report>(this, "DeleteReport", ExecuteDeleteReport);
        }

        public async void ExecuteRefreshCommand()
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                IsRefreshing = true;
                reportsDataFullyLoaded = false;

                Reports.Clear();
                reportsIndex = 0;

                var reports = await reportsDataService.GetReports(userName, reportsIndex / reportsPageSize, reportsPageSize);
                foreach (var report in reports)
                {
                    // local time adjustments
                    report.Created = report.Created.ToLocalTime();
                    report.Modified = report.Modified.ToLocalTime();
                    report.Observed = report.Observed.ToLocalTime();

                    Reports.Add(report);
                    reportsIndex++;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"ExecuteRefreshCommand() encountered an unexpected exception, {ex.GetType().Name}; {ex.Message}");
            }
            finally
            {
                IsRefreshing = false;
                IsBusy = false;
            }
        }

        public async void ExecuteLoadMoreDataCommand()
        {
            try
            {
                if (IsBusy) return;
                if (reportsDataFullyLoaded) return;
                IsBusy = true;

                var reports = await reportsDataService.GetReports(userName, reportsIndex / reportsPageSize, reportsPageSize);
                var count = reports.Count();
                if (reportsIndex == ((reportsIndex / reportsPageSize) * reportsPageSize) + count)
                {
                    reportsDataFullyLoaded = true;
                    return;
                }

                foreach(var report in reports)
                {
                    // date time adjustments
                    report.Created = report.Created.ToLocalTime();
                    report.Modified = report.Modified.ToLocalTime();
                    report.Observed = report.Observed.ToLocalTime();
                    
                    Reports.Add(report);
                }
                reportsIndex += count;
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"ExecuteLoadMoreDataCommand() encountered an unexpected exception, {ex.GetType().Name}; {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
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

        public async void ExecuteTestCommand()
        {
            Debug.WriteLine("ReportsViewModel::ExecuteTestCommand()");
            var newReports = await reportsDataService.GetReports(userName, 0, 10);
        }

        public async void ExecuteUpdateReport(ReportDetailViewModel model, Report report)
        {
            report.Created = report.Created.ToUniversalTime();
            report.Modified = report.Modified.ToUniversalTime();
            report.Observed = report.Observed.ToUniversalTime();

            await reportsDataService.UpdateReport(report);
        }

        public async void ExecuteDeleteReport(ReportDetailViewModel model, Report report)
        {
        }
        

        private IReportsDataService reportsDataService;
        private string userName = string.Empty; 

        private ObservableCollection<Report> reports = new ObservableCollection<Report>();
        private const int reportsPageSize = 10;
        private int reportsIndex = 0;
        private bool reportsDataFullyLoaded = false;

        private bool isRefreshing = false;
    }
}
