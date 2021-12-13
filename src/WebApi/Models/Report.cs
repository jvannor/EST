namespace WebApi.Models
{
    public class Report
    {
        public Guid Id { get; set; }

        public string ReportType { get; set; }

        public string Author { get; set; }

        public string Subject { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int Revision { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }

        public List<string> Tags { get; private set; } 

        public string Description { get; set; }
    }
}
