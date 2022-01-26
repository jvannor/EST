using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using EST.ServiceContracts;
using EST.Services;
using EST.ViewModels;

namespace EST.Core
{
    public static class AppContainer
    {
        static AppContainer()
        {
            var builder = new ContainerBuilder();

            // view models
            builder.RegisterType<LoadingViewModel>();
            builder.RegisterType<LoginViewModel>();
            builder.RegisterType<HomeViewModel>();
            builder.RegisterType<ReportsViewModel>();
            builder.RegisterType<ReportDetailViewModel>();
            builder.RegisterType<ReportDetailTagsViewModel>();
            builder.RegisterType<SettingsViewModel>();
            builder.RegisterType<TagsViewModel>();
            builder.RegisterType<TagDetailViewModel>();
            builder.RegisterType<TemplatesViewModel>();
            builder.RegisterType<TemplateDetailViewModel>();
            builder.RegisterType<TemplateDetailTagsViewModel>();

            // services
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<DialogService>().As<IDialogService>();
            builder.RegisterType<GenericRestService>().As<IGenericRestService>();
            builder.RegisterType<ReportsDataService>().As<IReportsDataService>();
            builder.RegisterType<SettingsService>().As<ISettingsService>();
            builder.RegisterType<SettingsDocumentService>().As<ISettingsDocumentService>();

            container = builder.Build();
        }

        public static object Resolve(Type typeName)
        {
            return container.Resolve(typeName);
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        private static IContainer container;
    }
}
