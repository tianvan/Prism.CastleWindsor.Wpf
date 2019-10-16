
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using Castle.MicroKernel;
using Castle.Windsor;

using CommonServiceLocator;

using Prism.CastleWindsor.Legacy;
using Prism.IocContainer.Wpf.Tests.Support;
using Prism.IocContainer.Wpf.Tests.Support.Mocks;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;

using Xunit;

namespace Prism.CastleWindsor.Wpf.Tests
{
    [Collection("ServiceLocator")]
    public class CastleWindsorBootstrapperFixture : BootstrapperFixtureBase
    {
        [StaFact]
        public void ContainerDefaultsToNull()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            IWindsorContainer container = bootstrapper.BaseContainer;

            Assert.Null(container);
        }

        [StaFact]
        public void CanCreateConcreteBootstrapper()
        {
            new DefaultCastleWindsorBootstrapper();
        }

        [StaFact]
        public void CreateContainerShouldInitializeContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            IWindsorContainer container = bootstrapper.CallCreateContainer();

            Assert.NotNull(container);
            Assert.IsAssignableFrom<IWindsorContainer>(container);
        }

        [StaFact]
        public void ConfigureContainerAddsModuleCatalogToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            IModuleCatalog returnedCatalog = bootstrapper.BaseContainer.Resolve<IModuleCatalog>();
            Assert.NotNull(returnedCatalog);
            Assert.True(returnedCatalog is ModuleCatalog);
        }

        [StaFact]
        public void ConfigureContainerAddsLoggerFacadeToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            ILoggerFacade returnedCatalog = bootstrapper.BaseContainer.Resolve<ILoggerFacade>();
            Assert.NotNull(returnedCatalog);
        }

        [StaFact]
        public void ConfigureContainerAddsRegionNavigationJournalEntryToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            IRegionNavigationJournalEntry actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournalEntry>();
            IRegionNavigationJournalEntry actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournalEntry>();

            Assert.NotNull(actual1);
            Assert.NotNull(actual2);
            Assert.NotSame(actual1, actual2);
        }

        [StaFact]
        public void ConfigureContainerAddsRegionNavigationJournalToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            IRegionNavigationJournal actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournal>();
            IRegionNavigationJournal actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournal>();

            Assert.NotNull(actual1);
            Assert.NotNull(actual2);
            Assert.NotSame(actual1, actual2);
        }

        [StaFact]
        public void ConfigureContainerAddsRegionNavigationServiceToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            IRegionNavigationService actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationService>();
            IRegionNavigationService actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationService>();

            Assert.NotNull(actual1);
            Assert.NotNull(actual2);
            Assert.NotSame(actual1, actual2);
        }

        [StaFact]
        public void ConfigureContainerAddsNavigationTargetHandlerToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            IRegionNavigationContentLoader actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationContentLoader>();
            IRegionNavigationContentLoader actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationContentLoader>();

            Assert.NotNull(actual1);
            Assert.NotNull(actual2);
            Assert.Same(actual1, actual2);
        }

        [StaFact]
        public void RegisterFrameworkExceptionTypesShouldRegisterActivationException()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.CallRegisterFrameworkExceptionTypes();

            Assert.True(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(ActivationException)));
        }

        [StaFact]
        public void RegisterFrameworkExceptionTypesShouldRegisterResolutionFailedException()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.CallRegisterFrameworkExceptionTypes();

            Assert.True(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(ComponentResolutionException)));
            Assert.True(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(ComponentNotFoundException)));
            Assert.True(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(ComponentRegistrationException)));
            Assert.True(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(CircularDependencyException)));
        }
    }

    internal class DefaultCastleWindsorBootstrapper : CastleWindsorBootstrapper
    {
        public List<string> MethodCalls = new List<string>();
        public bool InitializeModulesCalled;
        public bool ConfigureRegionAdapterMappingsCalled;
        public RegionAdapterMappings DefaultRegionAdapterMappings;
        public bool CreateLoggerCalled;
        public bool CreateModuleCatalogCalled;
        public bool ConfigureContainerCalled;
        public bool CreateShellCalled;
        public bool CreateContainerCalled;
        public bool ConfigureModuleCatalogCalled;
        public bool InitializeShellCalled;
        public bool ConfigureServiceLocatorCalled;
        public bool ConfigureViewModelLocatorCalled;
        public bool ConfigureDefaultRegionBehaviorsCalled;
        public DependencyObject ShellObject = new UserControl();

        public DependencyObject BaseShell => Shell;
        public IWindsorContainer BaseContainer
        {
            get => Container;
            set => Container = value;
        }

        public MockLoggerAdapter BaseLogger => Logger as MockLoggerAdapter;

        public IWindsorContainer CallCreateContainer()
        {
            return CreateContainer();
        }

        protected override IWindsorContainer CreateContainer()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            CreateContainerCalled = true;
            return base.CreateContainer();
        }

        protected override void ConfigureContainer()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            ConfigureContainerCalled = true;
            base.ConfigureContainer();
        }

        protected override ILoggerFacade CreateLogger()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            CreateLoggerCalled = true;
            return new MockLoggerAdapter();
        }

        protected override DependencyObject CreateShell()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            CreateShellCalled = true;
            return ShellObject;
        }

        protected override void ConfigureServiceLocator()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            ConfigureServiceLocatorCalled = true;
            base.ConfigureServiceLocator();
        }

        protected override void ConfigureViewModelLocator()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            ConfigureViewModelLocatorCalled = true;
            base.ConfigureViewModelLocator();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            CreateModuleCatalogCalled = true;
            return base.CreateModuleCatalog();
        }

        protected override void ConfigureModuleCatalog()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            ConfigureModuleCatalogCalled = true;
            base.ConfigureModuleCatalog();
        }

        protected override void InitializeShell()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            InitializeShellCalled = true;
            // no op
        }

        protected override void InitializeModules()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            InitializeModulesCalled = true;
            base.InitializeModules();
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            ConfigureDefaultRegionBehaviorsCalled = true;
            return base.ConfigureDefaultRegionBehaviors();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            ConfigureRegionAdapterMappingsCalled = true;
            RegionAdapterMappings regionAdapterMappings = base.ConfigureRegionAdapterMappings();

            DefaultRegionAdapterMappings = regionAdapterMappings;

            return regionAdapterMappings;
        }

        protected override void RegisterFrameworkExceptionTypes()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            base.RegisterFrameworkExceptionTypes();
        }

        public void CallRegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();
        }
    }
}
