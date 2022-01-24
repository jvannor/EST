using System;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EST.Core;
using EST.ServiceContracts;

namespace EST
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Akavache.Registrations.Start("EST");
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            AppCenter.Start("ios=edd502da-b7f7-4919-b8a2-c1a513fc4843;" +
                            "android=917830b8-1d07-4f94-9399-47b8480cd8c5;" +
                            typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
