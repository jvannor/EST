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
    public sealed class TemplateDetailTagsViewModel : ViewModelBase, IQueryAttributable
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

        public TemplateDetailTagsViewModel(
            ISettingsService settingsService,
            ISettingsDocumentService settingsDocumentService) : base(settingsService)
        {
            Title = "Tags";
            SelectedTags = new ObservableCollection<object>();
            Tags = new ObservableCollection<string>();
            initialized = false;
            this.settingsDocumentService = settingsDocumentService;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey("SelectedTags"))
            {
                var json = HttpUtility.UrlDecode(query["SelectedTags"]);
                var tags = JsonSerializer.Deserialize<List<string>>(json);
                SelectedTags = new ObservableCollection<object>(tags);
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
                    Tags = doc.Tags;
                    initialized = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TemplateDetailTagsViewModel::ExecuteAppearingCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsBusy = false;
        }

        public Command SaveCommand => new Command(ExecuteSaveCommand);

        public async void ExecuteSaveCommand(object parameter)
        {
            IsBusy = true;
            MessagingCenter.Send(this, "UpdateTags", SelectedTags.Select(x => x as string));
            await Shell.Current.GoToAsync($"..?");
        }

        #endregion

        #region Fields

        private bool initialized;
        private ObservableCollection<object> selectedTags;
        private ObservableCollection<string> tags;
        private ISettingsDocumentService settingsDocumentService;

        #endregion
    }
}
