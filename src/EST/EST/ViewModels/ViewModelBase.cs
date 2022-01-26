using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EST.ServiceContracts;

namespace EST.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        #endregion

        #region Methods

        public ViewModelBase(ISettingsService settings)
        {
            settingsService = settings;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Fields

        protected readonly ISettingsService settingsService;
        private bool isBusy = false;
        private string title = string.Empty;

        #endregion
    }
}
