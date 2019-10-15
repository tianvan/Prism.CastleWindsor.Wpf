using CommonServiceLocator;

using Prism.CastleWindsor.Ioc;
using Prism.CastleWindsor.Regions;
using Prism.Ioc;
using Prism.Regions;

namespace Prism.CastleWindsor
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
