using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mobile.Models;

namespace Mobile.ViewModels
{
    internal class ReportsViewModel : ViewModelBase
    {
        public Command LoadMoreDataCommand => new Command(ExecuteLoadMoreDataCommand);
        public Command RefreshCommand => new Command(ExecuteRefreshCommand);

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
                     
        public ReportsViewModel()
        {
            System.Diagnostics.Debug.WriteLine("ReportsViewModel::ctor()");
            Title = "Reports";

            AddTestData(20);
            itemCount = 20;
        }

        public async void ExecuteRefreshCommand()
        {
            Debug.WriteLine("ReportsViewModel::ExecuteRefreshCommand()");
            IsRefreshing = true;

            Reports.Clear();
            itemCount = 0;

            AddTestData(20);
            itemCount = 20;

            IsRefreshing = false;
        }

        public async void ExecuteLoadMoreDataCommand()
        {
            Debug.WriteLine("ReportsViewModel::ExecuteLoadMoreDataCommand()");
            if (itemCount < maxItemCount)
            {
                AddTestData(20);
                itemCount += 20;
            }
        }

        private void AddTestData(int count)
        {
            var category = flip ? "Category" : "CATEGORY";
            flip = !flip;

            for (int i=0; i<count; i++)
            {
                var report = new Report();
                report.ReportType = "Synthetic";
                report.Created = report.Modified = DateTime.UtcNow;
                report.Author = report.Subject = "tool@email.com";
                report.Revision = 1;
                report.Category = category;
                report.Subcategory = "Subcategory";
                report.Detail = "Detail";
                report.Description = "Test";
                Reports.Add(report);
            }
        }

        private ObservableCollection<Report> reports = new ObservableCollection<Report>();

        private bool isRefreshing = false;
        private int itemCount = 0;
        private bool flip = false;
        const int maxItemCount = 100;
        const int pageSize = 10;
    }
}
