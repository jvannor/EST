using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EST.Utilities;

namespace EST.ServiceContracts
{
    public interface IAuthenticationService
    {
        Task<bool> Authenticated();

        Task<bool> Login();

        Task<bool> Logout();

        Task<Credentials> GetCredentials();

        Task<Credentials> RefreshCredentials(bool force=false);

        Task<string> GetSubject();

        Task SetSubject(string subject);

        Task<string> GetAuthor();

        Task SetAuthor(string author);

    }
}
