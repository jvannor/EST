using System.Threading.Tasks;

namespace EST.ServiceContracts
{
    public interface IDialogService
    {
        Task<bool> InputBox(string title, string message, string accept, string cancel);

        Task MessageBox(string title, string message, string cancel);
    }
}