using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Mobile.Models
{
    public class SettingsDocument : BindableObject
    {
        #region Properties

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

        public ObservableCollection<ReportTemplate> Templates
        {
            get
            {
                return templates;
            }

            set
            {
                if (templates != value)
                {
                    templates = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Fields

        private string id;
        private string author;
        private string subject;
        private DateTime created;
        private DateTime modified;
        private int revision;
        private ObservableCollection<string> tags;
        private ObservableCollection<ReportTemplate> templates;

        #endregion
    }
}
