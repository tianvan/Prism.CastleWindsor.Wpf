using CommonServiceLocator;
using Prism.CastleWindsor.Wpf.Ioc;
using Prism.CastleWindsor.Wpf.Regions;
using Prism.Ioc;
using Prism.Regions;

namespace Prism.CastleWindsor.Wpf
{
    public abstract class PrismApplication : PrismApplicationBase
    {
        protected override IContainerExtension CreateContainerExtension()
        {
            return new CastleWindsorExtension();
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);

            containerRegistry.RegisterSingleton<IRegionNavigationContentLoader, CastleWinsorRegionNavigationContentLoader>();
            containerRegistry.RegisterSingleton<IServiceLocator, CastleWinsorServiceLocatorAdapter>();
        }
    }
}
