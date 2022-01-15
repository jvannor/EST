using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Web;
using Xamarin.Forms;
using Mobile.ServiceContracts;

namespace Mobile.ViewModels
{
    internal class ReportDetailTagsViewModel : ViewModelBase, IQueryAttributable
    {
        #region Commands

        public Command SaveCommand => new Command(ExecuteSaveCommand);

        #endregion

        #region Properties

        public ObservableCollection<object> SelectedTags
        {
            get { return selectedTags; }
            set
            {
                if (selectedTags != value)
                {
                    selectedTags = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Tags
        {
            get { return tags; }
            set
            {
                if (tags != value)
                {
                    tags = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        public ReportDetailTagsViewModel(ISettingsService ss, ISettingsDocumentService sds) : base(ss)
        {
            Title = "Tags";

            settingsDocumentService = sds;
            tags = new ObservableCollection<string>();
            selectedTags = new ObservableCollection<object>();

            Init();
        }

        public async void Init()
        {
            var doc = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
            Tags = new ObservableCollection<string>(doc.Tags);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey("SelectedTags"))
            {
                var encodedSelectedTags = query["SelectedTags"];
                if (!string.IsNullOrEmpty(encodedSelectedTags))
                {
                    var selectedTagsJson = HttpUtility.UrlDecode(encodedSelectedTags);
                    var selectedTags = JsonSerializer.Deserialize<string[]>(selectedTagsJson);
                    SelectedTags = new ObservableCollection<object>(selectedTags);
                }
            }
        }

        public async void ExecuteSaveCommand(object parameter)
        {
            MessagingCenter.Send(this, "UpdateTags", SelectedTags.Select(x => x as string));
            await Shell.Current.GoToAsync($"..?");
        }

        #endregion

        #region Fields

        private ISettingsDocumentService settingsDocumentService;
        private ObservableCollection<object> selectedTags;
        private ObservableCollection<string> tags;

        #endregion
    }
}
