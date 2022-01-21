using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        #region Commands

        public Command EditCommand => new Command(ExecuteEditCommand);

        public Command NewCommand => new Command(ExecuteNewCommand);

        public Command RefreshCommand => new Command(ExecuteRefreshCommand);

        #endregion

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

            Init();
        }

        private async void Init()
        {
            try
            {
                IsBusy = true;
                SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"TagsViewModel::Init() encountered an exception; {ex.GetType().Name}; {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void ExecuteDeleteTag(TagDetailViewModel model, string tag)
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    var target = SettingsDocument.Tags.Where(t => t == tag).FirstOrDefault();
                    if (target != null)
                    {
                        SettingsDocument.Tags.Remove(tag);
                        await settingsDocumentService.UpdateSettingsDocument(SettingsDocument);
                    }
                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    Debug.WriteLine($"TagsViewModel::ExecuteDeleteTag() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                }
            }
        }

        public async void ExecuteEditCommand(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                var encoded = HttpUtility.UrlEncode((string)parameter);
                await Shell.Current.GoToAsync($"TagDetails?Tag={encoded}");
                IsBusy = false;
            }
        }

        public async void ExecuteNewCommand()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Shell.Current.GoToAsync("TagDetails?Tag=");
                IsBusy = false;
            }
        }

        public async void ExecuteRefreshCommand()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
                    IsBusy = false;
                }
                catch(Exception ex)
                {
                    IsBusy = false;
                    Debug.WriteLine($"TagsViewModel::ExecuteRefreshCommand() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                }

                IsRefreshing = false;
            }
        }

        public async void ExecuteUpdateTag(TagDetailViewModel model, (string, string) tag)
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;

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

                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    Debug.WriteLine($"TagsViewModel::ExecuteUpdateTag() encountered an exception; {ex.GetType().Name}; {ex.Message}");
                }
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
