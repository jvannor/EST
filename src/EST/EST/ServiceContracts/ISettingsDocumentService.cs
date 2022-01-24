using System.Threading.Tasks;
using EST.Models;

namespace EST.ServiceContracts
{
    internal interface ISettingsDocumentService
    {
        Task<SettingsDocument> GetSettingsDocument(string author, string subject);

        Task UpdateSettingsDocument(SettingsDocument document);
    }
}