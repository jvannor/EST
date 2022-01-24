using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EST.Utilities;

namespace EST.ServiceContracts
{
    internal interface IAuthenticationService
    {
        Task<bool> Authenticated();

        Task<bool> Login();

        Task<bool> Logout();

        Task<Credentials> GetCredentials();

        Task<Credentials> RefreshCredentials(bool force=false);
    }
}
