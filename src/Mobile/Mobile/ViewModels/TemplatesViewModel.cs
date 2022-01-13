using System;
using Mobile.Models;
using Mobile.ServiceContracts;

namespace Mobile.ViewModels
{
    internal class TemplatesViewModel : ViewModelBase
    {
        #region Properties

        public SettingsDocument SettingsDocument
        {
            get
            {
                return settingsDocument;
            }

            set
            {
                if (settingsDocument != value)
                {
                    settingsDocument = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        public TemplatesViewModel(ISettingsService ss, ISettingsDocumentService sds) : base(ss)
        {
            settingsDocumentService = sds;

            Init();
        }

        private async void Init()
        {
            SettingsDocument = await settingsDocumentService.GetSettingsDocument(settingsService.UserName, settingsService.UserName);
        }

        #endregion

        #region Fields

        private ISettingsDocumentService settingsDocumentService;
        private SettingsDocument settingsDocument;

        #endregion
    }
}
