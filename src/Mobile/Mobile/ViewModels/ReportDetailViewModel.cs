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
        #region Properties

        public Command GoToTagsCommand => new Command(ExecuteGoToTagsCommand);
        public Command CancelCommand => new Command(ExecuteCancelCommand);
        public Command DeleteCommand => new Command(ExecuteDeleteCommand);
        public Command SaveCommand => new Command(ExecuteSaveCommand);

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                SetProperty(ref id, value);
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
                SetProperty(ref reportType, value);
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
                SetProperty(ref author, value);
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
                SetProperty(ref subject, value);
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
                SetProperty(ref created, value);
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
                SetProperty(ref modified, value);
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
                SetProperty(ref revision, value);
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
                SetProperty(ref observed, value);
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                SetProperty(ref date, value);
            }
        }

        public TimeSpan Time
        {
            get
            {
                return time;
            }

            set
            {
                SetProperty(ref time, value);
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
            get
            {
                return categoryChoices;
            }

            private set
            {
                SetProperty(ref categoryChoices, value);
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
            get
            {
                return subcategoryChoices;
            }

            private set
            {
                SetProperty(ref subcategoryChoices, value);
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
                SetProperty(ref detail, value);
            }
        }

        public List<string> DetailChoices
        {
            get
            {
                return detailChoices;
            }

            private set
            {
                SetProperty(ref detailChoices, value);
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
                SetProperty(ref tags, value);
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
                SetProperty(ref description, value);
            }
        }

        #endregion

        #region Methods

        public ReportDetailViewModel(ISettingsService settings, IReportsDataService rds) : base(settings)
        {
            reportsDataService = rds;
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
                Created = Created.ToUniversalTime(),
                Modified = Modified.ToUniversalTime(),
                Revision = Revision,
                Observed = (Date + Time).ToUniversalTime(),
                Category = Category,
                Subcategory = Subcategory,
                Detail = Detail,
                Description = Description,
                Tags = new ObservableCollection<string>(Tags)
            };

            if (string.IsNullOrEmpty(report.Id))
            {
                var result = await reportsDataService.CreateReport(report);
                result.Created = result.Created.ToLocalTime();
                result.Modified = result.Modified.ToLocalTime();
                result.Observed = result.Observed.ToLocalTime();

                MessagingCenter.Send(this, "CreateReport", result);
            }
            else
            {
                var result = await reportsDataService.UpdateReport(report);
                result.Created = result.Created.ToLocalTime();
                result.Modified = result.Modified.ToLocalTime();
                result.Observed = result.Observed.ToLocalTime();

                MessagingCenter.Send(this, "UpdateReport", result);
            }

            await Shell.Current.GoToAsync("..?");
        }

        public async void ExecuteCancelCommand(object parameter)
        {
            await Shell.Current.GoToAsync("..?");
        }

        public async void ExecuteDeleteCommand(object parameter)
        {
            await reportsDataService.DeleteReport(Id);

            MessagingCenter.Send(this, "DeleteReport", Id);
            await Shell.Current.GoToAsync("..?");
        }

        public void ExecuteUpdateTags(ReportDetailTagsViewModel model, IEnumerable<string> tags)
        {
            Tags = new ObservableCollection<string>(tags);
        }

        #endregion

        #region Fields

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
        private IReportsDataService reportsDataService;

        #endregion
    }
}
