using Mobile.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.ServiceContracts
{
    internal interface IAuthenticationService
    {
        Task<bool> Login();

        Task<bool> Logout();

        Task<Credentials> GetCredentials();

        Task<Credentials> RefreshCredentials(bool force=false);
    }
}
