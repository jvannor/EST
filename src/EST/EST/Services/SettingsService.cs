using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using EST.Models;
using EST.ServiceContracts;

namespace EST.Services
{
    internal class SettingsService : ISettingsService
    {
        public string UserName
        {
            get { return Preferences.Get("UserName", string.Empty); }
            set { Preferences.Set("UserName", value); }
        }

        public void AddItem(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public string GetItem(string key)
        {
            return Preferences.Get(key, string.Empty);
        }
    }
}
