using System.Threading.Tasks;
using Mobile.Models;

namespace Mobile.ServiceContracts
{
    internal interface ISettingsDocumentService
    {
        Task<SettingsDocument> GetSettingsDocument(string author, string subject);

        Task UpdateSettingsDocument(SettingsDocument document);
    }
}