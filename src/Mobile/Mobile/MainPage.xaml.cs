using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mobile.Core;
using Mobile.ServiceContracts;

namespace Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            authenticationService = AppContainer.Resolve<IAuthenticationService>();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            await authenticationService?.SignIn();
        }

        private async void Logoff_Clicked(object sender, EventArgs e)
        {
            await authenticationService?.SignOut();
        }

        private IAuthenticationService authenticationService;
    }
}
