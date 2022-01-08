using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Web;
using Xamarin.Forms;
using Mobile.Models;
using Mobile.ServiceContracts;
using Mobile.Utilities;

namespace Mobile.ViewModels
{
    internal class ReportDetailViewModel : ViewModelBase, IQueryAttributable
    {
        public Command GoToTagsCommand => new Command(ExecuteGoToTagsCommand);
        public Command SaveCommand => new Command(ExecuteSaveCommand);
        public Command DeleteCommand => new Command(ExecuteDeleteCommand);

        public string Id
        {
            get { return id;  }
            set
            {
                if (string.Compare(id, value) != 0)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ReportType
        {
            get { return reportType; }
            set
            {
                if (string.Compare(reportType, value) != 0)
                {
                    reportType = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Author
        {
            get { return author; }
            set
            {
                if (string.Compare(author, value) != 0)
                {
                    author = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Subject
        {
            get { return subject; }
            set
            {
                if (string.Compare(subject, value) != 0)
                {
                    subject = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Created
        {
            get { return created; }
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
            get { return modified; }
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
            get { return revision; }
            set
            {
                if (revision != value)
                {
                    revision = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Observed
        {
            get { return observed; }
            set
            {
                if (observed != value)
                {
                    observed = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeSpan Time
        {
            get { return time; }
            set
            {
                if (time != value)
                {
                    time = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Category
        {
            get { return category; }
            set
            {
                if (!string.IsNullOrEmpty(value) && (string.Compare(category, value) != 0))
                {
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
        }

        public List<string> CategoryChoices
        {
            get { return categoryChoices; }
            private set
            {
                if (!categoryChoices.SequenceEqual(value))
                {
                    categoryChoices = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Subcategory
        {
            get { return subcategory; }
            set
            {
                if (!string.IsNullOrEmpty(value) && (string.Compare(subcategory, value) != 0))
                {
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
        }

        public List<string> SubcategoryChoices
        {
            get { return subcategoryChoices; }
            private set
            {
                if (!subcategoryChoices.SequenceEqual(value))
                {
                    subcategoryChoices = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Detail
        {
            get { return detail; }
            set
            {
                if (!string.IsNullOrEmpty(value) && (string.Compare(detail, value) != 0))
                {
                    detail = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> DetailChoices
        {
            get { return detailChoices; }
            private set
            {
                if (!detailChoices.SequenceEqual(value))
                {
                    detailChoices = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Tags
        {
            get { return tags; }
            set
            {
                if (!tags.SequenceEqual<string>(value))
                {
                    tags = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (string.Compare(value, description) != 0)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }

        public ReportDetailViewModel(ISettingsService settings, IReportsDataService rds) : base(settings)
        {
            Title = "Report";

            var query1 = from item in Constants.SeizureClassifications
                         select item.Value;

            CategoryChoices = query1.ToList();
            Category = query1.First();

            MessagingCenter.Subscribe<ReportDetailTagsViewModel, IEnumerable<string>>(this, "UpdateTags", ExecuteUpdateTags);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.ContainsKey("report"))
            {
                var reportString = query["report"];
                var reportJson = HttpUtility.UrlDecode(reportString);
                var report = JsonSerializer.Deserialize<Report>(reportJson);

                Id = report.Id;
                ReportType = report.ReportType;
                Author = report.Author;
                Subject = report.Subject;
                Created = report.Created;
                Modified = report.Modified;
                Revision = report.Revision;
                Observed = report.Observed;
                Date = report.Observed.Date;
                Time = report.Observed.TimeOfDay;
                Category = report.Category;
                Subcategory = report.Subcategory;
                Detail = report.Detail;
                Tags = new ObservableCollection<string>(report.Tags);
                Description = report.Description;
            }
        }

        public async void ExecuteGoToTagsCommand(object parameter)
        {
            var selectedTagsJson = JsonSerializer.Serialize(tags.ToArray<string>());
            var encodedSelectedTags = HttpUtility.UrlEncode(selectedTagsJson);
            await Shell.Current.GoToAsync($"reportdetailtags?SelectedTags={encodedSelectedTags}");
        }

        public async void ExecuteSaveCommand(object parameter)
        {
            var report = new Report
            {
                Id = Id,
                ReportType = ReportType,
                Author = Author,
                Subject = Subject,
                Created = Created,
                Modified = Modified,
                Revision = Revision,
                Observed = Date + Time,
                Category = Category,
                Subcategory = Subcategory,
                Detail = Detail,
                Tags = new List<string>(Tags),
                Description = Description
            };

            if (string.IsNullOrEmpty(report.Id))
            {
                MessagingCenter.Send(this, "CreateReport", report);
            }
            else
            {
                MessagingCenter.Send(this, "UpdateReport", report);
            }

            await Shell.Current.GoToAsync("..?");
        }

        public async void ExecuteDeleteCommand(object parameter)
        {
            var report = new Report
            {
                Id = Id,
                ReportType = ReportType,
                Author = Author,
                Subject = Subject,
                Created = Created,
                Modified = Modified,
                Revision = Revision,
                Observed = Date + Time,
                Category = Category,
                Subcategory = Subcategory,
                Detail = Detail,
                Tags = new List<string>(Tags),
                Description = Description
            };

            if (!string.IsNullOrEmpty(report.Id))
            {
                MessagingCenter.Send(this, "DeleteReport", report);
            }

            await Shell.Current.GoToAsync("..?");
        }

        public void ExecuteUpdateTags(ReportDetailTagsViewModel model, IEnumerable<string> tags)
        {
            if (!Tags.SequenceEqual<string>(tags))
            {
                Tags = new ObservableCollection<string>(tags);
            }
        }

        private string id;
        private string reportType;
        private string author;
        private string subject;
        private DateTime created;
        private DateTime modified;
        private int revision;
        private DateTime observed;
        private string description;

        private DateTime date;
        private TimeSpan time;

        private string category;
        private List<string> categoryChoices = new List<string>();

        private string subcategory;
        private List<string> subcategoryChoices = new List<string>();

        private string detail;
        private List<string> detailChoices = new List<string>();

        private ObservableCollection<string> tags = new ObservableCollection<string>();

    }
}
