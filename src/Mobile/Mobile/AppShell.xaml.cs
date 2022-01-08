using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Mobile.Views;

namespace Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("reportdetail", typeof(ReportDetailView));
            Routing.RegisterRoute("reportdetailtags", typeof(ReportDetailTagsView));
        }
    }
}
