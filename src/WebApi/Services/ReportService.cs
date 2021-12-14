using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApi.Models;

namespace WebApi.Services
{
    public class ReportService
    {
        public ReportService(IOptions<WebApiDatabaseSettings> webApiDatbaseSettings)
        {   
            var mongoClient = new MongoClient(webApiDatbaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(webApiDatbaseSettings.Value.DatabaseName);
            reportCollection = mongoDatabase.GetCollection<Report>(webApiDatbaseSettings.Value.CollectionName);
        }

        public async Task<List<Report>> GetAsync() =>
            await reportCollection.Find(_ => true).ToListAsync();
        
        public async Task<Report?> GetAsync(string id) =>
            await reportCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Report newReport) =>
            await reportCollection.InsertOneAsync(newReport);

        public async Task UpdateAsync(string id, Report updatedReport) =>
            await reportCollection.ReplaceOneAsync(x => x.Id == id, updatedReport);

        public async Task RemoveAsync(string id) =>
            await reportCollection.DeleteOneAsync(x => x.Id == id);

        private readonly IMongoCollection<Report> reportCollection;
    }
}