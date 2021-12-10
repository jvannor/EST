using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Mobile.ServiceContracts;
using Mobile.Services;

namespace Mobile.Core
{
    internal static class AppContainer
    {
        static AppContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<NavigationService>().As<INavigationService>();
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
