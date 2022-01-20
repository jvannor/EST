using System;
using Xamarin.Forms;

namespace Mobile.Models
{
    public class ReportTemplate : BindableObject
    {
        #region Properties

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged();
                }
            }
        }

        public ReportTemplateContent Content
        {
            get
            {
                return content;
            }

            set
            {
                if (content != value)
                {
                    content = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        public ReportTemplate()
        {
            Title = string.Empty;
            Content = new ReportTemplateContent();
        }

        #endregion

        #region Fields

        private string title;
        private ReportTemplateContent content;

        #endregion
    }
}
