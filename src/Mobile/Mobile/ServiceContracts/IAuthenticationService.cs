using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.ServiceContracts
{
    internal interface IAuthenticationService
    {
        Task<bool> SignIn();

        Task<bool> SignOut();
    }
}
