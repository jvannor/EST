using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
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
            Debug.WriteLine("ReportsViewModel::ExecuteRefreshCommand()");
            IsRefreshing = true;

            IsRefreshing = false;
        }

        public async void ExecuteLoadMoreDataCommand()
        {
            Debug.WriteLine("ReportsViewModel::ExecuteLoadMoreDataCommand()");
            var newReports = await reportsDataService.GetReports("jvannor@hotmail.com", 0, 10);
        }

        private ObservableCollection<Report> reports = new ObservableCollection<Report>();

        private bool isRefreshing = false;
        private int itemCount = 0;
        private bool flip = false;
        const int maxItemCount = 1000;
        const int pageSize = 10;

        private IReportsDataService reportsDataService;
    }
}
