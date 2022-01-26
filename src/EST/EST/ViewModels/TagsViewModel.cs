using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Web;
using Xamarin.Forms;
using EST.Models;
using EST.ServiceContracts;
using System.Threading.Tasks;

namespace EST.ViewModels
{
    public sealed class TagsViewModel : ViewModelBase
    {
        #region Properties

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        public SettingsDocument SettingsDocument
        {
            get { return settingsDocument; }
            set { SetProperty(ref settingsDocument, value); }
        }

        #endregion

        #region Methods

        public TagsViewModel(
            ISettingsService settingsService,
            ISettingsDocumentService settingsDocumentService) : base(settingsService)
        {
            Title = "Tags";
            IsRefreshing = false;
            this.settingsDocumentService = settingsDocumentService;

            MessagingCenter.Subscribe<TagDetailViewModel, string>(this, "DeleteTag", ExecuteDeleteTag);
            MessagingCenter.Subscribe<TagDetailViewModel, (string, string)>(this, "UpdateTag", ExecuteUpdateTag);
        }

        public async void ExecuteDeleteTag(TagDetailViewModel model, string tag)
        {
            try
            {
                var target = SettingsDocument.Tags.Where(t => t == tag).FirstOrDefault();
                if (target != null)
                {
                    SettingsDocument.Tags.Remove(tag);
                    await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TagsViewModel::ExecuteDeleteTag() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }
        }

        public async void ExecuteUpdateTag(TagDetailViewModel model, (string, string) tag)
        {
            try
            {
                if (string.IsNullOrEmpty(tag.Item1) && (!string.IsNullOrEmpty(tag.Item2)))
                {
                    SettingsDocument.Tags.Add(tag.Item2);
                    await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
                }
                else if (SettingsDocument.Tags.Contains(tag.Item1))
                {
                    var i = SettingsDocument.Tags.IndexOf(tag.Item1);
                    SettingsDocument.Tags[i] = tag.Item2;
                    await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TagsViewModel::ExecuteUpdateTag() encountered an exception; {ex.GetType().Name}; {ex.Message}");
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
                if (SettingsDocument == null)
                {
                    SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"TagsViewModel::ExecuteAppearingCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsBusy = false;
        }

        public Command EditCommand => new Command(ExecuteEditCommand);

        public async void ExecuteEditCommand(object parameter)
        {
            var encoded = HttpUtility.UrlEncode((string)parameter);
            await Shell.Current.GoToAsync($"TagDetails?Tag={encoded}");
        }

        public Command NewCommand => new Command(ExecuteNewCommand);

        public async void ExecuteNewCommand()
        {
            await Shell.Current.GoToAsync("TagDetails?Tag=");
        }

        public Command RefreshCommand => new Command(ExecuteRefreshCommand);

        public async void ExecuteRefreshCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TagsViewModel::ExecuteRefreshCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }

            IsBusy = false;
            IsRefreshing = false;
        }
             
        #endregion

        #region Fields

        private ISettingsDocumentService settingsDocumentService;
        private SettingsDocument settingsDocument;
        private bool isRefreshing;

        #endregion
    }
}
