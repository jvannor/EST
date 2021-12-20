using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mobile.Models;

namespace Mobile.ServiceContracts
{
    internal interface IReportsDataService
    {
        Task<IEnumerable<Report>> GetReports(string subject, int page = 0, int size = 10);
    }
}
