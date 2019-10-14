using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Prism.Mvvm;

namespace Prism.CastleWindsor.Wpf.Legacy
{
    public static class CastleWindsorExtensions
    {
        public static IWindsorContainer RegisterTypeForNavigation(this IWindsorContainer container, Type type, string name)
        {
            return container.Register(Component.For(type).Named(name).LifestyleTransient());
        }

        public static IWindsorContainer RegisterTypeForNavigation<T>(this IWindsorContainer container, string name = default)
        {
            Type type = typeof(T);
            var viewName = string.IsNullOrWhiteSpace(name) ? type.Name : name;
            return container.RegisterTypeForNavigation(typeof(T), viewName);
        }

        public static IWindsorContainer RegisterTypeForNavigation<TView, TViewModel>(this IWindsorContainer container, string name = default)
        {
            return container.RegisterTypeForNavigationWithViewModel<TViewModel>(typeof(TView), name);
        }

        private static IWindsorContainer RegisterTypeForNavigationWithViewModel<TViewModel>(this IWindsorContainer container, Type viewType, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = viewType.Name;
            }

            ViewModelLocationProvider.Register(viewType.ToString(), typeof(TViewModel));

            return container.RegisterTypeForNavigation(viewType, name);
        }
    }
}
