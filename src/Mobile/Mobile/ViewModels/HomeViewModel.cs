﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Mobile.Utilities;
using Mobile.Models;
using Mobile.ServiceContracts;
using System.Linq;
using System.Collections.ObjectModel;

namespace Mobile.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        #region Properties

        public Command NewReportCommand => new Command(ExecuteNewReportCommand);

        #endregion

        #region Methods

        public HomeViewModel(ISettingsService ss, ISettingsDocumentService sds, IReportsDataService rds) : base(ss)
        {
            Title = "Home";
            settingsDocumentService = sds;
            reportDataService = rds;
        }

        public async void ExecuteNewReportCommand()
        {
            var userName = settingsService.UserName;
            var timestamp = DateTime.Now.ToLocalTime();

            var doc = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
            var template = doc.Templates.First().Content;
            var report = new Report
            {
                Author = settingsService.UserName,
                Subject = settingsService.UserName,
                Created = DateTime.Now.ToLocalTime(),
                Modified = DateTime.Now.ToLocalTime(),
                Observed = DateTime.Now.ToLocalTime(),
                Category = template.Category,
                Subcategory = template.Subcategory,
                Detail = template.Detail,
                Tags = new ObservableCollection<string>(template.Tags)
            };

            var reportJson = JsonSerializer.Serialize(report);
            var encodedReport = HttpUtility.UrlEncode(reportJson);
            await Shell.Current.GoToAsync($"reportdetail?report={encodedReport}");
        }

        #endregion

        #region Fields

        private ISettingsDocumentService settingsDocumentService;
        private IReportsDataService reportDataService;

        #endregion
    }
}
