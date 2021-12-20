using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Mobile.ServiceContracts;
using Mobile.Services;
using Mobile.ViewModels;

namespace Mobile.Core
{
    internal static class AppContainer
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
            builder.RegisterType<SettingsViewModel>();
            builder.RegisterType<TagsViewModel>();
            builder.RegisterType<TemplatesViewModel>();

            // services
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<GenericRestService>().As<IGenericRestService>();

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
