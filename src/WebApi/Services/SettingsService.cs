using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApi.Models;

namespace WebApi.Services
{
    public class SettingsService
    {
        public SettingsService(IOptions<WebApiDatabaseSettings> webApiDatbaseSettings)
        {
            var mongoClient = new MongoClient(webApiDatbaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(webApiDatbaseSettings.Value.DatabaseName);
            settingsCollection = mongoDatabase.GetCollection<SettingsDocument>(webApiDatbaseSettings.Value.SettingsCollectionName);
        }

        public async Task<SettingsDocument?> GetOneAsync(string id) =>
            await settingsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<SettingsDocument> GetOneAsync(string author, string subject) =>
            await settingsCollection.Find(x => x.Author == author && x.Subject == subject)
            .SortByDescending(x => x.Created)
            .FirstOrDefaultAsync();

        public async Task CreateAsync(SettingsDocument newSettings) =>
            await settingsCollection.InsertOneAsync(newSettings);

        public async Task UpdateAsync(string id, SettingsDocument updatedSettings) =>
            await settingsCollection.ReplaceOneAsync(x => x.Id == id, updatedSettings);

        private readonly IMongoCollection<SettingsDocument> settingsCollection;
    }
}
