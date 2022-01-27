using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EST.Models;

namespace EST.ServiceContracts
{
    public interface ISettingsService
    {
        Task<IEnumerable<string>> GetTags(string author = "", string subject = "");

        Task SetTags(IEnumerable<string> tags, string author = "", string subject = "");

        Task<IEnumerable<ReportTemplate>> GetReportTemplates(string author = "", string subject = "");

        Task SetReportTemplates(IEnumerable<ReportTemplate> templates, string author = "", string subject = "");
    }
}
