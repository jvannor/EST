using System;
using System.Collections.Generic;

namespace Mobile.Models
{
    public class Report
    {
        public string ReportType { get; set; }

        public string Author { get; set; }

        public string Subject { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int Revision { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }

        public string Detail { get; set; }

        public List<string> Tags { get; set; } =
            new List<string>();

        public string Description { get; set; }
    }
}
