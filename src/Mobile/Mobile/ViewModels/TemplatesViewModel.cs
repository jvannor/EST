using System;
using Mobile.ServiceContracts;

namespace Mobile.ViewModels
{
    internal class TemplatesViewModel : ViewModelBase
    {
        public TemplatesViewModel(ISettingsService settings) : base(settings)
        {
            Title = "Templates Settings";
        }
    }
}
