using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Mobile.Models
{
    public class Report
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("reportType")]
        public string ReportType { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("modified")]
        public DateTime Modified { get; set; }

        [JsonPropertyName("revision")]
        public int Revision { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("subcategory")]
        public string Subcategory { get; set; }

        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } =
            new List<string>();

        [JsonIgnore]
        public string DisplayTags
        {
            get
            {
                var builder = new StringBuilder();
                var query = from tag in Tags
                            orderby tag ascending
                            select tag;

                var prefix = string.Empty;
                foreach(var tag in query)
                {
                    builder.AppendFormat("{0}{1}", prefix, tag);
                    prefix = "; ";
                }

                return builder.ToString();
            }
        }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
