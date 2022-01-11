namespace WebApi.Models
{
    public class WebApiDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CollectionName { get; set; } = null!;

        public string SettingsCollectionName { get; set; } = null;
    }
}