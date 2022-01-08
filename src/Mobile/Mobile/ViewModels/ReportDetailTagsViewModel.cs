﻿using System;
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
        public Command SaveCommand => new Command(ExecuteSaveCommand);

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

        public ReportDetailTagsViewModel(ISettingsService settings) : base(settings)
        {
            Title = "Tags";

            tags = new ObservableCollection<string>(settings.DefaultTags);
            selectedTags = new ObservableCollection<object>();
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
            var tags = from tag in SelectedTags
                       select tag as string;

            MessagingCenter.Send(this, "UpdateTags", tags);
            await Shell.Current.GoToAsync($"..?");
        }

        private ObservableCollection<object> selectedTags;
        private ObservableCollection<string> tags;
    }
}
