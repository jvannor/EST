using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EST.ServiceContracts;
using EST.Models;
using EST.Utilities;

namespace EST.Services
{
    internal class ReportsDataService : IReportsDataService
    {
        public ReportsDataService(IAuthenticationService authentication, IGenericRestService service)
        {
            authenticationService = authentication;
            genericRestService = service;
        }

        public async Task<Report> CreateReport(Report report)
        {
            var requestUri = $"{Constants.Api}{Constants.ReportsApiEndpoint}";

            var credentials = await authenticationService.GetCredentials();
            var result = await genericRestService.Post<Report>(requestUri, report, credentials.AccessToken);
            return result;
        }

        public async Task<Report> GetReport(string id)
        {
            var requestUri = $"{Constants.Api}{Constants.ReportsApiEndpoint}/{id}";

            var credentials = await authenticationService.GetCredentials();
            var result = await genericRestService.Get<Report>(requestUri, credentials.AccessToken);
            return result;
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

        public async Task<Report> UpdateReport(Report report)
        {
            var requestUri = $"{Constants.Api}{Constants.ReportsApiEndpoint}/{report.Id}";

            var credentials = await authenticationService.GetCredentials();
            var result = await genericRestService.Put<Report>(requestUri, report, credentials.AccessToken);
            return result;
        }

        public async Task DeleteReport(string id)
        {
            var requestUri = $"{Constants.Api}{Constants.ReportsApiEndpoint}/{id}";

            var credentials = await authenticationService.GetCredentials();
            await genericRestService.Delete(requestUri, credentials.AccessToken);
        }

        private IAuthenticationService authenticationService;
        private IGenericRestService genericRestService;
    }
}
