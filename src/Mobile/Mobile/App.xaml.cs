using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobile.Core;
using Mobile.ServiceContracts;

namespace Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
