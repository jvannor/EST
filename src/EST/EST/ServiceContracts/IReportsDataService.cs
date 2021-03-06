using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EST.Models;

namespace EST.ServiceContracts
{
    public interface IReportsDataService
    {
        Task<Report> GetReport(string id);

        Task<Report> CreateReport(Report report);

        Task<IEnumerable<Report>> GetReports(int page = 0, int size = 10);

        Task<Report> UpdateReport(Report report);

        Task DeleteReport(string id);
    }
}
