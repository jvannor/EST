using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mobile.ServiceContracts;

namespace Mobile.Services
{
    internal class DialogService : IDialogService
    {
        #region Methods

        public async Task MessageBox(string title, string message, string cancel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> InputBox(string title, string message, string accept, string cancel)
        {
            return await App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        #endregion
    }
}
