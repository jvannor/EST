using System;
namespace Mobile.ViewModels
{
    internal class ReportDetailViewModel : ViewModelBase
    {
        public ReportDetailViewModel()
        {
            System.Diagnostics.Debug.WriteLine("ReportDetailsViewModel::ctor()");
            Title = "Report Detail";
        }
    }
}
