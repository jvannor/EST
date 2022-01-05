using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mobile.Models;
using Mobile.ServiceContracts;

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
                     
        public ReportsViewModel(IReportsDataService service)
        {
            System.Diagnostics.Debug.WriteLine("ReportsViewModel::ctor()");
            Title = "Reports";
            reportsDataService = service;
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

                var reports = await reportsDataService.GetReports("jvannor%40hotmail.com", reportsIndex / reportsPageSize, reportsPageSize);
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

                var reports = await reportsDataService.GetReports("jvannor%40hotmail.com", reportsIndex / reportsPageSize, reportsPageSize);
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
                await Shell.Current.GoToAsync($"reportdetail?Id={report.Id}");
            }
        }

        public async void ExecuteTestCommand()
        {
            Debug.WriteLine("ReportsViewModel::ExecuteTestCommand()");
            var newReports = await reportsDataService.GetReports("jvannor%40hotmail.com", 0, 10);
        }

        private IReportsDataService reportsDataService;

        private ObservableCollection<Report> reports = new ObservableCollection<Report>();
        private const int reportsPageSize = 10;
        private int reportsIndex = 0;
        private bool reportsDataFullyLoaded = false;

        private bool isRefreshing = false;
    }
}
