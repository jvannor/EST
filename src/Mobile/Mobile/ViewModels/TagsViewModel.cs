using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Web;
using Mobile.Models;
using Mobile.ServiceContracts;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    internal class TagsViewModel : ViewModelBase
    {
        #region Properties

        public Command EditCommand => new Command(ExecuteEditCommand);
        public Command NewCommand => new Command(ExecuteNewCommand);
        public Command RefreshCommand => new Command(ExecuteRefreshCommand);

        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }

            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged();
                }
            }
        }

        public SettingsDocument SettingsDocument
        {
            get
            {
                return settingsDocument;
            }

            set
            {
                if (settingsDocument != value)
                {
                    settingsDocument = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        public TagsViewModel(ISettingsService ss, ISettingsDocumentService sds) : base(ss)
        {
            Title = "Tags";
            settingsDocumentService = sds;
            isRefreshing = false;

            Init();

            MessagingCenter.Subscribe<TagDetailViewModel, string>(this, "DeleteTag", ExecuteDeleteTag);
            MessagingCenter.Subscribe<TagDetailViewModel, (string, string)>(this, "UpdateTag", ExecuteUpdateTag);
        }

        private async void Init()
        {
            SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
        }

        public async void ExecuteDeleteTag(TagDetailViewModel model, string tag)
        {
            var target = SettingsDocument.Tags.Where(t => t == tag).FirstOrDefault();
            if (target != null)
            {
                SettingsDocument.Tags.Remove(tag);
                await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
            }
        }

        public async void ExecuteEditCommand(object parameter)
        {
            var encoded = HttpUtility.UrlEncode((string)parameter);
            await Shell.Current.GoToAsync($"TagDetails?Tag={encoded}");
        }

        public async void ExecuteNewCommand()
        {
            await Shell.Current.GoToAsync("TagDetails?Tag=");
        }

        public async void ExecuteRefreshCommand()
        {
            if (!IsBusy)
            {
                IsBusy = true;

                SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
                IsRefreshing = false;

                IsBusy = false;
            }
        }

        public async void ExecuteUpdateTag(TagDetailViewModel model, (string, string) tag)
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
             
        #endregion

        #region Fields

        private ISettingsDocumentService settingsDocumentService;
        private SettingsDocument settingsDocument;
        private bool isRefreshing;

        #endregion
    }
}
