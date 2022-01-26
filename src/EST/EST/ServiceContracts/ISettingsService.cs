using System;
using System.Collections.Generic;
using EST.Models;

namespace EST.ServiceContracts
{
    public interface ISettingsService
    {
        string UserName { get; set; }

        void AddItem(string key, string value);

        string GetItem(string key);  
    }
}
