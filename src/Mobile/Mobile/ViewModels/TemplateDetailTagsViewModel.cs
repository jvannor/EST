using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public TemplateDetailTagsViewModel(ISettingsService settingsService, ISettingsDocumentService settingsDocumentService) : base(settingsService)
        {
            Title = "Tags";
            SelectedTags = new ObservableCollection<object>();
            Tags = new ObservableCollection<string>();
            this.settingsDocumentService = settingsDocumentService;

            Init();
        }

        private async void Init()
        {
            var doc = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
            Tags = doc.Tags;
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

        public async void ExecuteSaveCommand(object parameter)
        {
            MessagingCenter.Send(this, "UpdateTags", SelectedTags.Select(x => x as string));
            await Shell.Current.GoToAsync($"..?");
        }

        #endregion

        #region Fields

        private ObservableCollection<object> selectedTags;
        private ObservableCollection<string> tags;
        private ISettingsDocumentService settingsDocumentService;

        #endregion
    }
}
