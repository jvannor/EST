using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Mobile.Models;
using Mobile.ServiceContracts;

namespace Mobile.Services
{
    internal class SettingsService : ISettingsService
    {
        public string UserName
        {
            get { return Preferences.Get("UserName", string.Empty); }
            set { Preferences.Set("UserName", value); }
        }

        public Report DefaultReportTemplate
        {
            get
            {
                return defaultReportTemplate;
            }
        }

        public List<string> DefaultTags
        {
            get { return defaultTags; }
        }

        public void AddItem(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public string GetItem(string key)
        {
            return Preferences.Get(key, string.Empty);
        }

        private readonly Report defaultReportTemplate = new Report
        {
            Id = string.Empty,
            ReportType = "Seizure Report",
            Revision = 1,
            Category = "Unclassified",
            Subcategory = "-",
            Detail = "-",
            Description = string.Empty
        };

        private readonly List<string> defaultTags = new List<string>
        {
            "Reflexive",
            "Sound",
            "Proprioceptive",
            "Episode",
            "Electric",
            "One Second",
            "Two Seconds",
            "Three Seconds",
            "Five Seconds",
            "Home",
            "School",
            "Mild",
            "Medium",
            "Severe"
        };
    }
}
