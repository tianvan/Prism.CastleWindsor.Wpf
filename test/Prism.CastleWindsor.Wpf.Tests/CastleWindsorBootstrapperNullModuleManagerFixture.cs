
using System.Windows;

using Castle.MicroKernel.Registration;

using CommonServiceLocator;

using Prism.CastleWindsor.Legacy;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;

using Xunit;

namespace Prism.CastleWindsor.Wpf.Tests
{
    public class CastleWindsorBootstrapperNullModuleManagerFixture
    {
        [Fact]
        public void RunShouldNotCallInitializeModulesWhenModuleManagerNotFound()
        {
            var bootstrapper = new NullModuleManagerBootstrapper();

            bootstrapper.Run();

            Assert.False(bootstrapper.InitializeModulesCalled);
        }

        private class NullModuleManagerBootstrapper : CastleWindsorBootstrapper
        {
            public bool InitializeModulesCalled;

            protected override void ConfigureContainer()
            {
                //base.RegisterDefaultTypesIfMissing();

                Container.Register(Component.For<ILoggerFacade>().Instance(Logger));

                Container.Register(Component.For<IModuleCatalog>().Instance(ModuleCatalog));
                RegisterTypeIfMissing(typeof(IServiceLocator), typeof(CastleWinsorServiceLocatorAdapter), true);
            }

            protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
            {
                return null;
            }

            protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
            {
                return null;
            }

            protected override DependencyObject CreateShell()
            {
                return null;
            }

            protected override void InitializeModules()
            {
                InitializeModulesCalled = true;
            }
        }
    }
}
