using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Web;
using Mobile.ServiceContracts;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    internal class TemplateDetailTagsViewModel : ViewModelBase, IQueryAttributable
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

        public TemplateDetailTagsViewModel(
            ISettingsService settingsService,
            ISettingsDocumentService settingsDocumentService) : base(settingsService)
        {
            Title = "Tags";
            SelectedTags = new ObservableCollection<object>();
            Tags = new ObservableCollection<string>();
            this.settingsDocumentService = settingsDocumentService;

            Init();
        }

        private async void Init()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    var doc = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
                    Tags = doc.Tags;
                    IsBusy = false;
                }
                catch(Exception ex)
                {
                    IsBusy = false;
                    Debug.WriteLine($"TemplateDetailTagsViewModel::Init() encountered an exception; {ex.GetType().Name}; {ex.Message}");
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
                    var json = HttpUtility.UrlDecode(query["SelectedTags"]);
                    var tags = JsonSerializer.Deserialize<List<string>>(json);
                    SelectedTags = new ObservableCollection<object>(tags);
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

        private ObservableCollection<object> selectedTags;
        private ObservableCollection<string> tags;
        private ISettingsDocumentService settingsDocumentService;

        #endregion
    }
}
