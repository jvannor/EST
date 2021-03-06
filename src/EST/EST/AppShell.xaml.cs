using System;
using System.Collections.Generic;
using Xamarin.Forms;
using EST.Views;

namespace EST
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("ReportDetail", typeof(ReportDetailView));
            Routing.RegisterRoute("ReportDetailTags", typeof(ReportDetailTagsView));
            Routing.RegisterRoute("TagDetails", typeof(TagDetailView));
            Routing.RegisterRoute("TemplateDetail", typeof(TemplateDetailView));
            Routing.RegisterRoute("TemplateDetailTags", typeof(TemplateDetailTagsView));
        }
    }
}
