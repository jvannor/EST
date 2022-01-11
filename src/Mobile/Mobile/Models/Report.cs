using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Mobile.Models
{
    public class Report : BindableObject
    {
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ReportType
        {
            get
            {
                return reportType;
            }

            set
            {
                if (reportType != value)
                {
                    reportType = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Author
        {
            get
            {
                return author;
            }

            set
            {
                if (author != value)
                {
                    author = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Subject
        {
            get
            {
                return subject;
            }

            set
            {
                if (subject != value)
                {
                    subject = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Created
        {
            get
            {
                return created;
            }

            set
            {
                if (created != value)
                {
                    created = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Modified
        {
            get
            {
                return modified;
            }

            set
            {
                if (modified != value)
                {
                    modified = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Observed
        {
            get
            {
                return observed;
            }

            set
            {
                if (observed != value)
                {
                    observed = value;
                    OnPropertyChanged();
                }
            }
        }


        public int Revision
        {
            get
            {
                return revision;
            }

            set
            {
                if (revision != value)
                {
                    revision = value;
                    OnPropertyChanged();
                }
            }
        }


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

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                if (description != value)
                {
                    description = value;
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

        private string id;
        private string reportType;
        private string author;
        private string subject;
        private DateTime created;
        private DateTime modified;
        private DateTime observed;
        private int revision;
        private string category;
        private string subcategory;
        private string detail;
        private string description;
        private ObservableCollection<string> tags;
    }
}
