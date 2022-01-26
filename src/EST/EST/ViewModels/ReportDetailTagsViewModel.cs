using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Web;
using Xamarin.Forms;
using EST.ServiceContracts;
using System.Threading.Tasks;

namespace EST.ViewModels
{
    public sealed class ReportDetailTagsViewModel : ViewModelBase, IQueryAttributable
    {
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

        public ReportDetailTagsViewModel(
            ISettingsService settingsService,
            ISettingsDocumentService settingsDocumentService) : base(settingsService)
        {
            Title = "Tags";
            Tags = new ObservableCollection<string>();
            SelectedTags = new ObservableCollection<object>();
            initialized = false;

            this.settingsDocumentService = settingsDocumentService;
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

        #endregion

        #region Commands

        public Command AppearingCommand => new Command(ExecuteAppearingCommand);

        public async void ExecuteAppearingCommand()
        {
            IsBusy = true;

            try
            {
                if (!initialized)
                {
                    var doc = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
                    Tags = new ObservableCollection<string>(doc.Tags);
                    initialized = true;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"ReportDetailTagsViewModel::Init() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsBusy = false;
        }

        public Command SaveCommand => new Command(ExecuteSaveCommand);

        public async void ExecuteSaveCommand(object parameter)
        {
            MessagingCenter.Send(this, "UpdateTags", SelectedTags.Select(x => x as string));
            await Shell.Current.GoToAsync($"..?");
        }

        #endregion

        #region Fields

        private ISettingsDocumentService settingsDocumentService;
        private bool initialized;
        private ObservableCollection<object> selectedTags;
        private ObservableCollection<string> tags;

        #endregion
    }
}
