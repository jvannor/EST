using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Xamarin.Forms;

namespace EST.Models
{
    public class ReportTemplateContent : BindableObject
    {
        #region Properties

        public string Category
        {
            get { return category.Item3; }
            set
            {
                if (!string.IsNullOrEmpty(value) && category.Item3 != value)
                {
                    category = CLASSIFICATIONS.Where(x => x.Item2 == 0 && x.Item3 == value).First();
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public ObservableCollection<string> Categories
        {
            get { return categories; }
            private set { SetProperty(ref categories, value); }
        }

        public string Subcategory
        {
            get { return subcategory.Item3; }
            set
            {
                if (!string.IsNullOrEmpty(value) && subcategory.Item3 != value)
                {
                    subcategory = CLASSIFICATIONS.Where(x => x.Item2 == category.Item1 && x.Item3 == value).First();
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public ObservableCollection<string> Subcategories
        {
            get { return subcategories; }
            private set { SetProperty(ref subcategories, value); }
        }

        public string Detail
        {
            get { return detail.Item3; }
            set
            {
                if (!string.IsNullOrEmpty(value) && detail.Item3 != value)
                {
                    detail = CLASSIFICATIONS.Where(x => x.Item2 == subcategory.Item1 && x.Item3 == value).First();
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public ObservableCollection<string> Details
        {
            get { return details; }
            private set { SetProperty(ref details, value); }
        }

        public ObservableCollection<string> Tags
        {
            get { return tags; }
            set { SetProperty(ref tags, value); }
        }

        #endregion

        #region Methods

        public ReportTemplateContent()
        {
            PropertyChanged += PropertyChangedHandler;
            Categories = new ObservableCollection<string>(CLASSIFICATIONS.Where(x => x.Item2 == 0).Select(x => x.Item3));
            Category = Categories.First();

            Tags = new ObservableCollection<string>();
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Category))
            {
                Subcategories = new ObservableCollection<string>(CLASSIFICATIONS.Where(x => x.Item2 == category.Item1).Select(x => x.Item3));
                Subcategory = Subcategories.First();
            }
            else if (e.PropertyName == nameof(Subcategory))
            {
                Details = new ObservableCollection<string>(CLASSIFICATIONS.Where(x => x.Item2 == subcategory.Item1).Select(x => x.Item3));
                Detail = Details.First();
            }
        }


        #endregion

        #region Fields

        private (int, int, string) category;
        private (int, int, string) subcategory;
        private (int, int, string) detail;

        private ObservableCollection<string> categories;
        private ObservableCollection<string> subcategories;
        private ObservableCollection<string> details;

        private ObservableCollection<string> tags;

        private readonly List<(int, int, string)> CLASSIFICATIONS = new List<(int, int, string)>()
        {
            (1000, 0, "Unclassified"),
            (1100, 1000, "-"),
            (1110, 1100, "-"),
            (2000, 0, "Unknown Onset"),
            (2100, 2000, "Motor"),
            (2110, 2100, "Tonic-Clonic"),
            (2120, 2100, "EpilepticSpasms"),
            (2200, 2000, "Nonmotor"),
            (2210, 2200, "Behavior Arrest"),
            (3000, 0, "Focal Onset"),
            (3100, 3000, "Motor Onset"),
            (3110, 3100, "Automatisms"),
            (3120, 3100, "Atonic"),
            (3130, 3100, "Clonic"),
            (3140, 3100, "Epileptic Spasms"),
            (3150, 3100, "Hyperkinetic"),
            (3160, 3100, "Myoclonic"),
            (3170, 3100, "Tonic"),
            (3200, 3000, "Nonmotor Onset"),
            (3210, 3200, "Autonomic"),
            (3220, 3200, "Behavior Arrest"),
            (3230, 3200, "Cognitive"),
            (3240, 3200, "Emotional"),
            (3250, 3200, "Sensory"),
            (4000, 0, "Generalized Onset"),
            (4100, 4000, "Motor"),
            (4110, 4100, "Tonic-Clonic"),
            (4120, 4100, "Clonic"),
            (4130, 4100, "Tonic"),
            (4140, 4100, "Myoclonic"),
            (4150, 4100, "Myoclonic-Tonic-Clonic"),
            (4160, 4100, "Myoclonic-Atonic"),
            (4170, 4100, "Atonic"),
            (4180, 4100, "Epileptic Spasms"),
            (4200, 4000, "Nonmotor (Absence)"),
            (4210, 4200, "Typical"),
            (4220, 4200, "Atypical"),
            (4230, 4200, "Myclonic"),
            (4240, 4200, "Eyelid Myoclonia")
        };

        #endregion
    }
}
