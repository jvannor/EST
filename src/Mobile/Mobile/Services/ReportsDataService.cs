using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mobile.ServiceContracts;
using Mobile.Models;
using Mobile.Utilities;

namespace Mobile.Services
{
    internal class ReportsDataService : IReportsDataService
    {
        public ReportsDataService(IAuthenticationService authentication, IGenericRestService service)
        {
            authenticationService = authentication;
            genericRestService = service;
        }

        public async Task<IEnumerable<Report>> GetReports(string subject, int page = 0, int size = 10)
        {
            var uriBuilder = new UriBuilder(Constants.Api);
            uriBuilder.Path = Constants.ReportsApiEndpoint;
            uriBuilder.Query = $"subject={subject}&page={page}&size={size}";

            var credentials = await authenticationService.GetCredentials();
            var reports = await genericRestService.Get<List<Report>>(uriBuilder.ToString(), credentials.AccessToken);
            return reports;
        }

        public async Task<Report> GetReport(string id)
        {
            var requestUri = $"{Constants.Api}{Constants.ReportsApiEndpoint}/{id}";

            var credentials = await authenticationService.GetCredentials();
            var report = await genericRestService.Get<Report>(requestUri, credentials.AccessToken);
            return report;
        }

        private IAuthenticationService authenticationService;
        private IGenericRestService genericRestService;
    }
}
