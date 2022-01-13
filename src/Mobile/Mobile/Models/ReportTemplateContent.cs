using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Mobile.Models
{
    public class ReportTemplateContent : BindableObject
    {
        #region Properties

        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                if (category != value)
                {
                    category = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Subcategory
        {
            get
            {
                return subcategory;
            }

            set
            {
                if (subcategory != value)
                {
                    subcategory = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Detail
        {
            get
            {
                return detail;
            }

            set
            {
                if (detail != value)
                {
                    detail = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Tags
        {
            get
            {
                return tags;
            }

            set
            {
                if (tags != value)
                {
                    tags = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Fields

        private string category;
        private string subcategory;
        private string detail;
        private ObservableCollection<string> tags;

        #endregion
    }
}
