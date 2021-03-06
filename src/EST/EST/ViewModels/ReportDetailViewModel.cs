using System;
using System.Collections.Generic;
using System.Web;
using System.Text.Json;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using EST.Models;
using EST.ServiceContracts;
using System.Diagnostics;

namespace EST.ViewModels
{
    public sealed class ReportDetailViewModel : ViewModelBase, IQueryAttributable
    {
        #region Properties

        public Report Report
        {
            get { return report; }
            set { SetProperty(ref report, value); }
        }

        #endregion

        #region Methods

        public ReportDetailViewModel(
            IAuthenticationService authenticationService,
            IDialogService dialogService,
            ISettingsService settingsService,
            IReportsDataService reportsDataService) : base(authenticationService, dialogService, settingsService)
        {
            Title = "Report";
            this.reportsDataService = reportsDataService;
            
            MessagingCenter.Subscribe<ReportDetailTagsViewModel, IEnumerable<string>>(this, "UpdateTags", ExecuteUpdateTags);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey("Report"))
            {
                var encoded = query["Report"];
                var json = HttpUtility.UrlDecode(encoded);
                Report = JsonSerializer.Deserialize<Report>(json);
            }
        }

        public void ExecuteUpdateTags(ReportDetailTagsViewModel model, IEnumerable<string> tags)
        {
            Report.Tags = new ObservableCollection<string>(tags);
        }

        #endregion

        #region Commands

        public Command CancelCommand => new Command(ExecuteCancelCommand);

        public async void ExecuteCancelCommand(object parameter)
        {
            await Shell.Current.GoToAsync("..?");
        }

        public Command DeleteCommand => new Command(ExecuteDeleteCommand);

        public async void ExecuteDeleteCommand(object parameter)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReportDetailViewModel::ExecuteDeleteCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsBusy = false;
        }

        public Command GoToTagsCommand => new Command(ExecuteGoToTagsCommand);

        public async void ExecuteGoToTagsCommand(object parameter)
        {
            var json = JsonSerializer.Serialize(Report.Tags);
            var encoded = HttpUtility.UrlEncode(json);
            await Shell.Current.GoToAsync($"ReportDetailTags?SelectedTags={encoded}");
        }

        public Command SaveCommand => new Command(ExecuteSaveCommand);

        public async void ExecuteSaveCommand(object parameter)
        {
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
        }

        #endregion

        #region Fields

        private readonly IReportsDataService reportsDataService;
        private Report report;

        #endregion
    }
}
