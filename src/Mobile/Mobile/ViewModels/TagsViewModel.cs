using System;
using Mobile.ServiceContracts;

namespace Mobile.ViewModels
{
    internal class TagsViewModel : ViewModelBase
    {
        public TagsViewModel(ISettingsService settings) : base(settings)
        {
            Title = "Tags Setings";
        }
    }
}
