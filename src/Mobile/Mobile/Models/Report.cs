using System;
using System.Collections.Generic;

namespace Mobile.Models
{
    public class Report
    {
        public string Id { get; set; }

        public string ReportType { get; set; }

        public string Author { get; set; }

        public string Subject { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int Revision { get; set; }

        public DateTime Observed { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }

        public string Detail { get; set; }

        public List<string> Tags { get; set; } =
            new List<string>();

        public string Description { get; set; }

        public Report() { }

        public Report(Report source)
        {
            Id = source.Id;
            ReportType = source.ReportType;
            Author = source.Author;
            Subject = source.Subject;
            Created = source.Created;
            Modified = source.Modified;
            Revision = source.Revision;
            Observed = source.Observed;
            Category = source.Category;
            Subcategory = source.Subcategory;
            Detail = source.Detail;
            Tags = new List<string>(source.Tags);
            Description = source.Description;
        }
    }
}
