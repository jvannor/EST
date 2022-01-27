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

        public ObservableCollection<string> Tags
        {
            get { return tags; }
            set { SetProperty(ref tags, value); }
        }

        #endregion

        #region Methods

        public TagsViewModel(
            IAuthenticationService authenticationService,
            IDialogService dialogService,
            ISettingsService settingsService) : base(authenticationService, dialogService, settingsService)
        {
            Title = "Tags";
            IsRefreshing = false;

            MessagingCenter.Subscribe<TagDetailViewModel, string>(this, "DeleteTag", ExecuteDeleteTag);
            MessagingCenter.Subscribe<TagDetailViewModel, (string, string)>(this, "UpdateTag", ExecuteUpdateTag);
        }

        public async void ExecuteDeleteTag(TagDetailViewModel model, string tag)
        {
            try
            {
                var target = Tags.Where(t => t == tag).FirstOrDefault();
                if (target != null)
                {
                    Tags.Remove(tag);
                    await settingsService.SetTags(Tags);
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
                    Tags.Add(tag.Item2);
                    await settingsService.SetTags(Tags);
                }
                else if (Tags.Contains(tag.Item1))
                {
                    var i = Tags.IndexOf(tag.Item1);
                    Tags[i] = tag.Item2;
                    await settingsService.SetTags(Tags);
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
                if (Tags == null)
                {
                    Tags = new ObservableCollection<string>(await settingsService.GetTags());
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
                Tags = new ObservableCollection<string>(await settingsService.GetTags());
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

        private bool isRefreshing;
        private ObservableCollection<string> tags;

        #endregion
    }
}
