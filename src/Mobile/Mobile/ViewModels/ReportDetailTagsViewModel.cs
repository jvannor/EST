using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Web;
using Xamarin.Forms;
using Mobile.ServiceContracts;
using System.Diagnostics;

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
            set { SetProperty(ref selectedTags, value); }
        }

        public ObservableCollection<string> Tags
        {
            get { return tags; }
            set { SetProperty(ref tags, value); }
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
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    var doc = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
                    Tags = new ObservableCollection<string>(doc.Tags);
                    IsBusy = false;
                }
                catch(Exception ex)
                {
                    IsBusy = false;
                    Debug.WriteLine($"ReportDetailTagsViewModel::Init() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                }
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (!IsBusy)
            {
                IsBusy = true;
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
                IsBusy = false;
            }
        }

        public async void ExecuteSaveCommand(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                MessagingCenter.Send(this, "UpdateTags", SelectedTags.Select(x => x as string));
                await Shell.Current.GoToAsync($"..?");
                IsBusy = false;
            }
        }

        #endregion

        #region Fields

        private ISettingsDocumentService settingsDocumentService;
        private ObservableCollection<object> selectedTags;
        private ObservableCollection<string> tags;

        #endregion
    }
}
