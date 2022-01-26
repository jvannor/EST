using System.Threading.Tasks;
using EST.Models;

namespace EST.ServiceContracts
{
    public interface ISettingsDocumentService
    {
        Task<SettingsDocument> GetSettingsDocument(string author, string subject);

        Task UpdateSettingsDocument(SettingsDocument document);
    }
}