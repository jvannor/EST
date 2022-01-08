using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models
{
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string ReportType { get; set; }

        public string Author { get; set; }

        public string Subject { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int Revision { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }

        public string Detail { get; set; }

        public DateTime Observed { get; set; } 

        public List<string> Tags { get; set; } 

        public string Description { get; set; }
    }
}
