using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mobile.Models;

namespace Mobile.ViewModels
{
    internal class ReportsViewModel : ViewModelBase
    {
        public Command TestCommand => new Command(ExecuteTestCommand);

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
        }

        public async void ExecuteTestCommand()
        {
            System.Diagnostics.Debug.WriteLine("ReportsViewModel::ExecuteTestCommand()");
            await Shell.Current.GoToAsync("reportdetail");
        }

        private ObservableCollection<Report> reports;
    }
}
