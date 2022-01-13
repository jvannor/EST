﻿using System;
using Xamarin.Forms;
using Mobile.ServiceContracts;
using System.Collections.Generic;
using System.Web;
using Mobile.Models;
using System.Text.Json;

namespace Mobile.ViewModels
{
    internal class TagDetailViewModel : ViewModelBase, IQueryAttributable
    {
        #region Properties

        public Command CancelCommand => new Command(ExecuteCancelCommand);
        public Command DeleteCommand => new Command(ExecuteDeleteCommand);
        public Command UpdateCommand => new Command(ExecuteUpdateCommand);


        public string Tag
        {
            get
            {
                return tag2;
            }

            set
            {
                if (tag2 != value)
                {
                    tag2 = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        public TagDetailViewModel(ISettingsService ss) : base(ss)
        {
            Title = "Tag";
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            var decoded = HttpUtility.UrlDecode(query["tag"]);
            tag1 = Tag = decoded;
        }

        private async void ExecuteCancelCommand()
        {
            await Shell.Current.GoToAsync("..?");
        }

        private async void ExecuteDeleteCommand()
        {
            if (!string.IsNullOrEmpty(tag1))
            {
                MessagingCenter.Send(this, "DeleteTag", tag1);
            }

            await Shell.Current.GoToAsync("..?");
        }

        private async void ExecuteUpdateCommand()
        {
            MessagingCenter.Send(this, "UpdateTag", (tag1, tag2));
            await Shell.Current.GoToAsync("..?");
        }

        #endregion

        #region Fields

        private string tag1;
        private string tag2;

        #endregion
    }
}