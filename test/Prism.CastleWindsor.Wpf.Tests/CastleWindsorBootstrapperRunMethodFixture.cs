using System;
using System.Windows;
using System.Windows.Controls;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using CommonServiceLocator;

using Moq;

using Prism.CastleWindsor.Legacy;
using Prism.Events;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;

using Xunit;

namespace Prism.CastleWindsor.Wpf.Tests
{
    [Collection("ServiceLocator")]
    public class CastleWindsorBootstrapperRunMethodFixture
    {
        [StaFact]
        public void CanRunBootstrapper()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
        }

        [StaFact]
        public void RunShouldNotFailIfReturnedNullShell()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper { ShellObject = null };
            bootstrapper.Run();
        }

        [StaFact]
        public void RunConfiguresServiceLocatorProvider()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            Assert.True(ServiceLocator.Current is CastleWinsorServiceLocatorAdapter);
        }

        [StaFact]
        public void RunShouldInitializeContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            IWindsorContainer container = bootstrapper.BaseContainer;

            Assert.Null(container);

            bootstrapper.Run();

            container = bootstrapper.BaseContainer;

            Assert.NotNull(container);
            Assert.IsType<IWindsorContainer>(container);
        }

        [StaFact]
        public void RunAddsCompositionContainerToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            IWindsorContainer createdContainer = bootstrapper.CallCreateContainer();
            IWindsorContainer returnedContainer = createdContainer.Resolve<IWindsorContainer>();
            Assert.NotNull(returnedContainer);
            Assert.Equal(typeof(WindsorContainer), returnedContainer.GetType());
        }

        [StaFact]
        public void RunShouldCallInitializeModules()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            Assert.True(bootstrapper.InitializeModulesCalled);
        }

        [StaFact]
        public void RunShouldCallConfigureDefaultRegionBehaviors()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();

            Assert.True(bootstrapper.ConfigureDefaultRegionBehaviorsCalled);
        }

        [StaFact]
        public void RunShouldCallConfigureRegionAdapterMappings()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();

            Assert.True(bootstrapper.ConfigureRegionAdapterMappingsCalled);
        }

        [StaFact]
        public void RunShouldAssignRegionManagerToReturnedShell()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();

            Assert.NotNull(RegionManager.GetRegionManager(bootstrapper.BaseShell));
        }

        [StaFact]
        public void RunShouldCallCreateLogger()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();

            Assert.True(bootstrapper.CreateLoggerCalled);
        }

        [StaFact]
        public void RunShouldCallCreateModuleCatalog()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();

            Assert.True(bootstrapper.CreateModuleCatalogCalled);
        }

        [StaFact]
        public void RunShouldCallConfigureModuleCatalog()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();

            Assert.True(bootstrapper.ConfigureModuleCatalogCalled);
        }

        [StaFact]
        public void RunShouldCallCreateContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();

            Assert.True(bootstrapper.CreateContainerCalled);
        }

        [StaFact]
        public void RunShouldCallCreateShell()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();

            Assert.True(bootstrapper.CreateShellCalled);
        }

        [StaFact]
        public void RunShouldCallConfigureContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();

            Assert.True(bootstrapper.ConfigureContainerCalled);
        }

        [StaFact]
        public void RunShouldCallConfigureServiceLocator()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();

            Assert.True(bootstrapper.ConfigureServiceLocatorCalled);
        }

        [StaFact]
        public void RunShouldCallConfigureViewModelLocator()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();

            Assert.True(bootstrapper.ConfigureViewModelLocatorCalled);
        }

        [StaFact]
        public void RunRegistersInstanceOfILoggerFacade()
        {
            var mockedContainer = new Mock<IWindsorContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();
            mockedContainer.Verify(c => c.Register(Component.For<ILoggerFacade>().Instance(bootstrapper.BaseLogger)), Times.Once());
        }

        [StaFact]
        public void RunRegistersInstanceOfIModuleCatalog()
        {
            var mockedContainer = new Mock<IWindsorContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(Component.For(typeof(IModuleCatalog)).Instance(It.IsAny<object>())), Times.Once());
        }

        [StaFact]
        public void RunRegistersTypeForIServiceLocator()
        {
            var mockedContainer = new Mock<IWindsorContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(Component.For<IServiceLocator>().ImplementedBy<CastleWinsorServiceLocatorAdapter>()), Times.Once());
        }

        [StaFact]
        public void RunRegistersTypeForIModuleInitializer()
        {
            var mockedContainer = new Mock<IWindsorContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(Component.For<IModuleInitializer>().ImplementedBy(It.IsAny<Type>())), Times.Once());
        }

        [StaFact]
        public void RunRegistersTypeForIRegionManager()
        {
            var mockedContainer = new Mock<IWindsorContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(Component.For<IRegionManager>().ImplementedBy(It.IsAny<Type>())), Times.Once());
        }

        [StaFact]
        public void RunRegistersTypeForRegionAdapterMappings()
        {
            var mockedContainer = new Mock<IWindsorContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(Component.For<RegionAdapterMappings>().ImplementedBy(It.IsAny<Type>())), Times.Once());
        }

        [StaFact]
        public void RunRegistersTypeForIRegionViewRegistry()
        {
            var mockedContainer = new Mock<IWindsorContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(Component.For<IRegionViewRegistry>().ImplementedBy(It.IsAny<Type>())), Times.Once());
        }

        [StaFact]
        public void RunRegistersTypeForIRegionBehaviorFactory()
        {
            var mockedContainer = new Mock<IWindsorContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(Component.For<IRegionBehaviorFactory>().ImplementedBy(It.IsAny<Type>())), Times.Once());
        }

        [StaFact]
        public void RunRegistersTypeForIEventAggregator()
        {
            var mockedContainer = new Mock<IWindsorContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);

            bootstrapper.Run();

            mockedContainer.Verify(c => c.Register(Component.For<IEventAggregator>().ImplementedBy(It.IsAny<Type>())), Times.Once());
        }

        [StaFact]
        public void RunFalseShouldNotRegisterDefaultServicesAndTypes()
        {
            var mockedContainer = new Mock<IWindsorContainer>();
            SetupMockedContainerForVerificationTests(mockedContainer);

            var bootstrapper = new MockedContainerBootstrapper(mockedContainer.Object);
            bootstrapper.Run(false);
            mockedContainer.Verify(c => c.Register(Component.For<IEventAggregator>().ImplementedBy(It.IsAny<Type>())), Times.Never());
            mockedContainer.Verify(c => c.Register(Component.For<IRegionManager>().ImplementedBy(It.IsAny<Type>())), Times.Never());
            mockedContainer.Verify(c => c.Register(Component.For<RegionAdapterMappings>().ImplementedBy(It.IsAny<Type>())), Times.Never());
            mockedContainer.Verify(c => c.Register(Component.For<IServiceLocator>().ImplementedBy(It.IsAny<Type>())), Times.Never());
            mockedContainer.Verify(c => c.Register(Component.For<IModuleInitializer>().ImplementedBy(It.IsAny<Type>())), Times.Never());
        }

        [StaFact]
        public void ModuleManagerRunCalled()
        {
            // Have to use a non-mocked container because of IsRegistered<> extension method, Registrations property,and ContainerRegistration
            var container = new WindsorContainer();

            var mockedModuleInitializer = new Mock<IModuleInitializer>();
            var mockedModuleManager = new Mock<IModuleManager>();
            var regionAdapterMappings = new RegionAdapterMappings();
            var serviceLocatorAdapter = new CastleWinsorServiceLocatorAdapter(container);
            var regionBehaviorFactory = new RegionBehaviorFactory(serviceLocatorAdapter);

            container.Register(Component.For<IServiceLocator>().Instance(serviceLocatorAdapter));
            container.Register(Component.For<IModuleCatalog>().Instance(new ModuleCatalog()));
            container.Register(Component.For<IModuleInitializer>().Instance(mockedModuleInitializer.Object));
            container.Register(Component.For<IModuleManager>().Instance(mockedModuleManager.Object));
            container.Register(Component.For<RegionAdapterMappings>().Instance(regionAdapterMappings));

            container.Register(Component.For(typeof(RegionAdapterMappings)).ImplementedBy(typeof(RegionAdapterMappings)).LifestyleSingleton());
            container.Register(Component.For<IRegionManager>().ImplementedBy<RegionManager>().LifestyleSingleton());
            container.Register(Component.For<IEventAggregator>().ImplementedBy<EventAggregator>().LifestyleSingleton());
            container.Register(Component.For<IRegionViewRegistry>().ImplementedBy<RegionViewRegistry>().LifestyleSingleton());
            container.Register(Component.For<IRegionBehaviorFactory>().ImplementedBy<RegionBehaviorFactory>().LifestyleSingleton());
            container.Register(Component.For<IRegionNavigationJournal>().ImplementedBy<RegionNavigationJournal>().LifestyleSingleton());
            container.Register(Component.For<IRegionNavigationJournalEntry>().ImplementedBy<RegionNavigationJournalEntry>().LifestyleSingleton());
            container.Register(Component.For<IRegionNavigationService>().ImplementedBy<RegionNavigationService>().LifestyleSingleton());
            container.Register(Component.For<IRegionNavigationContentLoader>().ImplementedBy<RegionNavigationContentLoader>().LifestyleSingleton());

            container.Register(Component.For<SelectorRegionAdapter>().Instance(new SelectorRegionAdapter(regionBehaviorFactory)));
            container.Register(Component.For<ItemsControlRegionAdapter>().Instance(new ItemsControlRegionAdapter(regionBehaviorFactory)));
            container.Register(Component.For<ContentControlRegionAdapter>().Instance(new ContentControlRegionAdapter(regionBehaviorFactory)));

            var bootstrapper = new MockedContainerBootstrapper(container);
            bootstrapper.Run(false);

            mockedModuleManager.Verify(mm => mm.Run(), Times.Once());
        }

        [StaFact]
        public void RunShouldCallTheMethodsInOrder()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            Assert.Equal("CreateLogger", bootstrapper.MethodCalls[0]);
            Assert.Equal("CreateModuleCatalog", bootstrapper.MethodCalls[1]);
            Assert.Equal("ConfigureModuleCatalog", bootstrapper.MethodCalls[2]);
            Assert.Equal("CreateContainer", bootstrapper.MethodCalls[3]);
            Assert.Equal("ConfigureContainer", bootstrapper.MethodCalls[4]);
            Assert.Equal("ConfigureServiceLocator", bootstrapper.MethodCalls[5]);
            Assert.Equal("ConfigureViewModelLocator", bootstrapper.MethodCalls[6]);
            Assert.Equal("ConfigureRegionAdapterMappings", bootstrapper.MethodCalls[7]);
            Assert.Equal("ConfigureDefaultRegionBehaviors", bootstrapper.MethodCalls[8]);
            Assert.Equal("RegisterFrameworkExceptionTypes", bootstrapper.MethodCalls[9]);
            Assert.Equal("CreateShell", bootstrapper.MethodCalls[10]);
            Assert.Equal("InitializeShell", bootstrapper.MethodCalls[11]);
            Assert.Equal("InitializeModules", bootstrapper.MethodCalls[12]);
        }

        [StaFact]
        public void RunShouldLogBootstrapperSteps()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.Contains("Logger was created successfully.", messages[0]);
            Assert.Contains("Creating module catalog.", messages[1]);
            Assert.Contains("Configuring module catalog.", messages[2]);
            Assert.Contains("Creating Unity container.", messages[3]);
            Assert.Contains("Configuring the Unity container.", messages[4]);
            Assert.Contains("Adding UnityBootstrapperExtension to container.", messages[5]);
            Assert.Contains("Configuring ServiceLocator singleton.", messages[6]);
            Assert.Contains("Configuring the ViewModelLocator to use Unity.", messages[7]);
            Assert.Contains("Configuring region adapters.", messages[8]);
            Assert.Contains("Configuring default region behaviors.", messages[9]);
            Assert.Contains("Registering Framework Exception Types.", messages[10]);
            Assert.Contains("Creating the shell.", messages[11]);
            Assert.Contains("Setting the RegionManager.", messages[12]);
            Assert.Contains("Updating Regions.", messages[13]);
            Assert.Contains("Initializing the shell.", messages[14]);
            Assert.Contains("Initializing modules.", messages[15]);
            Assert.Contains("Bootstrapper sequence completed.", messages[16]);
        }

        [StaFact]
        public void RunShouldLogLoggerCreationSuccess()
        {
            const string ExpectedMessageText = "Logger was created successfully.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }
        [StaFact]
        public void RunShouldLogAboutModuleCatalogCreation()
        {
            const string ExpectedMessageText = "Creating module catalog.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldLogAboutConfiguringModuleCatalog()
        {
            const string ExpectedMessageText = "Configuring module catalog.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldLogAboutCreatingTheContainer()
        {
            const string ExpectedMessageText = "Creating Unity container.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldLogAboutConfiguringContainer()
        {
            const string ExpectedMessageText = "Configuring the Unity container.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldLogAboutConfiguringViewModelLocator()
        {
            const string ExpectedMessageText = "Configuring the ViewModelLocator to use Unity.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldLogAboutConfiguringRegionAdapters()
        {
            const string ExpectedMessageText = "Configuring region adapters.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldLogAboutConfiguringRegionBehaviors()
        {
            const string ExpectedMessageText = "Configuring default region behaviors.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldLogAboutRegisteringFrameworkExceptionTypes()
        {
            const string ExpectedMessageText = "Registering Framework Exception Types.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldLogAboutCreatingTheShell()
        {
            const string ExpectedMessageText = "Creating the shell.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldLogAboutInitializingTheShellIfShellCreated()
        {
            const string ExpectedMessageText = "Initializing the shell.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldNotLogAboutInitializingTheShellIfShellIsNotCreated()
        {
            const string ExpectedMessageText = "Initializing shell";
            var bootstrapper = new DefaultCastleWindsorBootstrapper { ShellObject = null };

            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.False(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldLogAboutInitializingModules()
        {
            const string ExpectedMessageText = "Initializing modules.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        [StaFact]
        public void RunShouldLogAboutRunCompleting()
        {
            const string ExpectedMessageText = "Bootstrapper sequence completed.";
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();
            System.Collections.Generic.IList<string> messages = bootstrapper.BaseLogger.Messages;

            Assert.True(messages.Contains(ExpectedMessageText));
        }

        private static void SetupMockedContainerForVerificationTests(Mock<IWindsorContainer> mockedContainer)
        {
            var mockedModuleInitializer = new Mock<IModuleInitializer>();
            var mockedModuleManager = new Mock<IModuleManager>();
            var regionAdapterMappings = new RegionAdapterMappings();
            var serviceLocatorAdapter = new CastleWinsorServiceLocatorAdapter(mockedContainer.Object);
            var regionBehaviorFactory = new RegionBehaviorFactory(serviceLocatorAdapter);

            mockedContainer.Setup(c => c.Resolve(null, typeof(IServiceLocator))).Returns(serviceLocatorAdapter);

            mockedContainer.Setup(c => c.Register(Component.For(It.IsAny<Type>()).Instance(It.IsAny<object>()).Named(It.IsAny<string>())));

            mockedContainer.Setup(c => c.Resolve(null, typeof(IModuleCatalog))).Returns(
                new ModuleCatalog());

            mockedContainer.Setup(c => c.Resolve(null, typeof(IModuleInitializer))).Returns(
                mockedModuleInitializer.Object);

            mockedContainer.Setup(c => c.Resolve(null, typeof(IModuleManager))).Returns(
                mockedModuleManager.Object);

            mockedContainer.Setup(c => c.Resolve(null, typeof(RegionAdapterMappings))).Returns(
                regionAdapterMappings);

            mockedContainer.Setup(c => c.Resolve(null, typeof(SelectorRegionAdapter))).Returns(
                new SelectorRegionAdapter(regionBehaviorFactory));

            mockedContainer.Setup(c => c.Resolve(null, typeof(ItemsControlRegionAdapter))).Returns(
                new ItemsControlRegionAdapter(regionBehaviorFactory));

            mockedContainer.Setup(c => c.Resolve(null, typeof(ContentControlRegionAdapter))).Returns(
                new ContentControlRegionAdapter(regionBehaviorFactory));
        }

        private class MockedContainerBootstrapper : CastleWindsorBootstrapper
        {
            private readonly IWindsorContainer _container;
            public ILoggerFacade BaseLogger => base.Logger;

            public void CallConfigureContainer()
            {
                base.ConfigureContainer();
            }

            public MockedContainerBootstrapper(IWindsorContainer container)
            {
                _container = container;
            }

            protected override IWindsorContainer CreateContainer()
            {
                return _container;
            }

            protected override DependencyObject CreateShell()
            {
                return new UserControl();
            }

            protected override void InitializeShell()
            {
                // no op
            }
        }
    }
}
