using System.Linq;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Driver.Linq;
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

        //public async Task<List<Report>> GetAsync() =>
        //    await reportCollection.Find(_ => true).ToListAsync();
        
        public async Task<List<Report>> GetAsync(string subject, int page = 0, int size = 10)
        {
            var query = reportCollection.AsQueryable<Report>()
                .Where(r => r.Subject == subject)
                .OrderByDescending(r => r.Observed)
                .Skip(size * page)
                .Take(size);

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<List<Report>> GetAsync(string? subject = null, DateTime? begin = null, DateTime? end = null)
        {           
            var query = reportCollection.AsQueryable<Report>();
            if (!string.IsNullOrEmpty(subject))
                query = query.Where(r => r.Subject == subject);
            if (begin != null)
                query = query.Where(r => r.Created >= begin);
            if (end != null)
                query = query.Where(r => r.Created < end);
            
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<Report?> GetOneAsync(string id) =>
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