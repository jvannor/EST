using System;
using Xamarin.Forms;
using Mobile.Models;
using Mobile.ServiceContracts;
using System.Collections.Generic;
using System.Web;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace Mobile.ViewModels
{
    internal class ReportDetailViewModel : ViewModelBase, IQueryAttributable
    {
        #region Commands

        public Command GoToTagsCommand => new Command(ExecuteGoToTagsCommand);

        public Command CancelCommand => new Command(ExecuteCancelCommand);

        public Command DeleteCommand => new Command(ExecuteDeleteCommand);

        public Command SaveCommand => new Command(ExecuteSaveCommand);

        #endregion

        #region Properties

        public Report Report
        {
            get { return report; }
            set { SetProperty(ref report, value); }
        }

        #endregion

        #region Methods

        public ReportDetailViewModel(
            ISettingsService settingsService,
            IDialogService dialogService,
            IReportsDataService reportsDataService) : base(settingsService)
        {
            Title = "Report";
            this.dialogService = dialogService;
            this.reportsDataService = reportsDataService;
            
            MessagingCenter.Subscribe<ReportDetailTagsViewModel, IEnumerable<string>>(this, "UpdateTags", ExecuteUpdateTags);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (!IsBusy)
            {
                IsBusy = true;

                if (query.ContainsKey("Report"))
                {
                    var encoded = query["Report"];
                    var json = HttpUtility.UrlDecode(encoded);
                    Report = JsonSerializer.Deserialize<Report>(json);
                }

                IsBusy = false;
            }
        }

        public async void ExecuteCancelCommand(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Shell.Current.GoToAsync("..?");
                IsBusy = false;
            }
        }

        public async void ExecuteDeleteCommand(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;

                var confirm = await dialogService.InputBox("Confirmation", "Are you sure that you want to delete this item?", "Yes", "No");
                if (confirm)
                {
                    if (!string.IsNullOrEmpty(Report.Id))
                    {
                        await reportsDataService.DeleteReport(Report.Id);
                        MessagingCenter.Send(this, "DeleteReport", Report.Id);
                    }

                    await Shell.Current.GoToAsync("..?");
                }

                IsBusy = false;
            }
        }

        public async void ExecuteGoToTagsCommand(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                var json = JsonSerializer.Serialize(Report.Tags);
                var encoded = HttpUtility.UrlEncode(json);
                await Shell.Current.GoToAsync($"ReportDetailTags?SelectedTags={encoded}");
                IsBusy = false;
            }
        }

        public async void ExecuteSaveCommand(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if (string.IsNullOrEmpty(Report.Id))
                {
                    Report = await reportsDataService.CreateReport(Report);
                    Report.Created = Report.Created.ToLocalTime();
                    Report.Modified = report.Modified.ToLocalTime();
                    Report.Observed = report.Observed.ToLocalTime();
                    MessagingCenter.Send(this, "CreateReport", Report);
                }
                else
                {
                    Report = await reportsDataService.UpdateReport(Report);
                    Report.Created = Report.Created.ToLocalTime();
                    Report.Modified = Report.Modified.ToLocalTime();
                    Report.Observed = Report.Observed.ToLocalTime();
                    MessagingCenter.Send(this, "UpdateReport", Report);
                }

                await Shell.Current.GoToAsync("..?");
                IsBusy = false;
            }
        }

        public void ExecuteUpdateTags(ReportDetailTagsViewModel model, IEnumerable<string> tags)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                Report.Tags = new ObservableCollection<string>(tags);
                IsBusy = false;
            }
        }

        #endregion

        #region Fields

        private Report report;
        private IReportsDataService reportsDataService;
        private IDialogService dialogService;

        #endregion
    }
}
