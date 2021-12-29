using System;
using System.Collections.Generic;
using System.Linq;
using Mobile.Utilities;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    [QueryProperty(nameof(Id), "Id")]
    internal class ReportDetailViewModel : ViewModelBase
    {
        public string Id
        {
            get { return id;  }
            set
            {
                if (string.Compare(id, value) == 0)
                    return;

                id = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                if (value == date)
                    return;

                date = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan Time
        {
            get { return time; }
            set
            {
                if (value == time)
                    return;

                time = value;
                OnPropertyChanged();
            }
        }

        public string Category
        {
            get { return category; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (string.Compare(category, value) == 0)
                {
                    return;
                }

                category = value;
                OnPropertyChanged();

                var query1 = from item in Constants.SeizureClassifications
                             where item.Value.ToLower() == value.ToLower()
                             select item;

                var query2 = from item in query1.First()
                             select item.Value;

                SubcategoryChoices = query2.ToList();
                Subcategory = query2.First();
            }
        }

        public List<string> CategoryChoices
        {
            get { return categoryChoices; }
            private set
            {
                if (categoryChoices.SequenceEqual(value))
                    return;

                categoryChoices = value;
                OnPropertyChanged();
            }
        }

        public string Subcategory
        {
            get { return subcategory; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (string.Compare(subcategory, value) == 0)
                {
                    return;
                }

                subcategory = value;
                OnPropertyChanged();

                var query1 = from item in Constants.SeizureClassifications
                             where item.Value.ToLower() == Category.ToLower()
                             select item;

                var query2 = from item in query1.First()
                             where item.Value.ToLower() == value.ToLower()
                             select item;

                var query3 = from item in query2.First()
                             select item.Value;

                DetailChoices = query3.ToList();
                Detail = query3.First();
            }
        }

        public List<string> SubcategoryChoices
        {
            get { return subcategoryChoices; }
            private set
            {
                if (subcategoryChoices.SequenceEqual(value))
                    return;

                subcategoryChoices = value;
                OnPropertyChanged();
            }
        }

        public string Detail
        {
            get { return detail; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (string.Compare(detail, value) == 0)
                {
                    return;
                }

                detail = value;
                OnPropertyChanged();
            }
        }

        public List<string> DetailChoices
        {
            get { return detailChoices; }
            private set
            {
                if (detailChoices.SequenceEqual(value))
                    return;

                detailChoices = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (string.Compare(value, description) == 0)
                    return;

                description = value;
                OnPropertyChanged();
            }
        }

        public ReportDetailViewModel()
        {
            System.Diagnostics.Debug.WriteLine("ReportDetailsViewModel::ctor()");
            Title = "Report Detail";

            date = DateTime.UtcNow;
            time = date.TimeOfDay;

            var query1 = from item in Constants.SeizureClassifications
                         select item.Value;

            CategoryChoices = query1.ToList();
            Category = query1.First();
        }

        private string id;

        private DateTime date;
        private TimeSpan time;

        private string category;
        private List<string> categoryChoices = new List<string>();

        private string subcategory;
        private List<string> subcategoryChoices = new List<string>();

        private string detail;
        private List<string> detailChoices = new List<string>();

        private string description;
    }
}
