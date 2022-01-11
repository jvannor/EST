namespace WebApi.Models
{
    public class ReportTemplateContent
    {
        public string Category { get; set; }

        public string Subcategory { get; set; }

        public string Detail { get; set; }

        public List<string> Tags { get; set; }
    }
}
