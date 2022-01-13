using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    internal class Widget : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Category
        {
            get
            {
                return category.Item3;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && category.Item3 != value)
                {
                    category = CATEGORIES.Where(x => x.Item2 == 0 && x.Item3 == value).First();
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public ObservableCollection<string> Categories
        {
            get
            {
                return categories;
            }
            
            set
            {
                if (categories != value)
                {
                    categories = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Subcategory
        {
            get
            {
                return subcategory.Item3; 
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && subcategory.Item3 != value)
                {
                    subcategory = CATEGORIES.Where(x => x.Item2 == category.Item1 && x.Item3 == value).First();
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public ObservableCollection<string> Subcategories
        {
            get
            {
                return subcategories;
            }

            set
            {
                if (subcategories != value)
                {
                    subcategories = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Detail
        {
            get
            {
                return detail.Item3;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && detail.Item3 != value)
                {
                    detail = CATEGORIES.Where(x => x.Item2 == subcategory.Item1 && x.Item3 == value).First();
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public ObservableCollection<string> Details
        {
            get
            {
                return details;
            }

            set
            {
                if (details != value)
                {
                    details = value;
                    OnPropertyChanged();
                }
            }
        }

        public Widget()
        {
            PropertyChanged += Widget_PropertyChanged;
            Categories = new ObservableCollection<string>(CATEGORIES.Where(x => x.Item2 == 0).Select(x => x.Item3));
            Category = Categories.First();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Widget_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Category))
            {
                Subcategories = new ObservableCollection<string>(CATEGORIES.Where(x => x.Item2 == category.Item1).Select(x => x.Item3));
                Subcategory = Subcategories.First();
            }
            else if (e.PropertyName == nameof(Subcategory))
            {
                Details = new ObservableCollection<string>(CATEGORIES.Where(x => x.Item2 == subcategory.Item1).Select(x => x.Item3));
                Detail = Details.First();
            }
        }

        private (int, int, string) category;
        private (int, int, string) subcategory;
        private (int, int, string) detail;

        private ObservableCollection<string> categories;
        private ObservableCollection<string> subcategories;
        private ObservableCollection<string> details;

        private readonly List<(int, int, string)> CATEGORIES = new List<(int, int, string)>()
        {
            (0,    -1,   "Root"),
            (1000, 0,    "Category 1"),
            (1100, 1000, "Subcategory A"),
            (1110, 1100, "Detail i"),
            (1120, 1100, "Detail ii"),
            (1200, 1000, "Subcategory B"),
            (1210, 1200, "Detail iii"),
            (1220, 1200, "Detail iv"),
            (2000, 0,    "Category 2"),
            (2100, 2000, "Subcategory C"),
            (2110, 2100, "Detail v"),
            (2120, 2100, "Detail vi")
        };

    }
}
