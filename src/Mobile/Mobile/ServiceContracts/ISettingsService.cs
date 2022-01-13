using System;
using System.Collections.Generic;
using Mobile.Models;

namespace Mobile.ServiceContracts
{
    internal interface ISettingsService
    {
        string UserName { get; set; }

        void AddItem(string key, string value);

        string GetItem(string key);  
    }
}
