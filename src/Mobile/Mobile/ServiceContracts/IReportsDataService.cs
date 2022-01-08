using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mobile.Models;

namespace Mobile.ServiceContracts
{
    internal interface IReportsDataService
    {
        Task<Report> GetReport(string id);

        Task<Report> CreateReport(Report report);

        Task<IEnumerable<Report>> GetReports(string subject, int page = 0, int size = 10);

        Task<Report> UpdateReport(Report report);

        Task DeleteReport(string id);
    }
}
