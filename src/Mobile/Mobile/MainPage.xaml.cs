using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mobile.Core;
using Mobile.ServiceContracts;
using Xamarin.Essentials;
using System.Text.Json.Nodes;

namespace Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            authenticationService = AppContainer.Resolve<IAuthenticationService>();
            genericRestService = AppContainer.Resolve<IGenericRestService>();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            await authenticationService?.SignIn();
        }

        private async void Logoff_Clicked(object sender, EventArgs e)
        {
            await authenticationService?.SignOut();
        }

        private async void InvokeRestService_Clicked(object sender, EventArgs e)
        {
            var token = await SecureStorage.GetAsync("est.mobile.accessToken");
            var result = await genericRestService.Get<JsonArray>("https://api.dev.detroitcyclecar.com:6001/weatherforecast", token);
        }

        private IAuthenticationService authenticationService;
        private IGenericRestService genericRestService;
    }
}
