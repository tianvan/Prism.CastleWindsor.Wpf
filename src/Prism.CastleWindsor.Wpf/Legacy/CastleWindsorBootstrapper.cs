using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using CommonServiceLocator;

using Prism.CastleWindsor.Ioc;
using Prism.CastleWindsor.Properties;
using Prism.CastleWindsor.Regions;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Prism.CastleWindsor.Legacy
{
    [Obsolete("It is recommended to use the new PrismApplication as the app's base class. This will require updating the App.xaml and App.xaml.cs files.", false)]
    public abstract class CastleWindsorBootstrapper : Bootstrapper
    {
        private bool _useDefaultConfiguration = true;

        public IWindsorContainer Container { get; protected set; }

        public override void Run(bool runWithDefaultConfiguration)
        {
            _useDefaultConfiguration = runWithDefaultConfiguration;

            Logger = CreateLogger();
            if (Logger == null)
            {
                throw new InvalidOperationException(Resources.NullLoggerFacadeException);
            }

            Logger.Log(Resources.LoggerCreatedSuccessfully, Category.Debug, Priority.Low);

            Logger.Log(Resources.CreatingModuleCatalog, Category.Debug, Priority.Low);
            ModuleCatalog = CreateModuleCatalog();
            if (ModuleCatalog == null)
            {
                throw new InvalidOperationException(Resources.NullModuleCatalogException);
            }

            Logger.Log(Resources.ConfiguringModuleCatalog, Category.Debug, Priority.Low);
            ConfigureModuleCatalog();

            Logger.Log(Resources.CreatingUnityContainer, Category.Debug, Priority.Low);
            Container = CreateContainer();
            if (Container == null)
            {
                throw new InvalidOperationException(Resources.NullUnityContainerException);
            }

            ContainerExtension = CreateContainerExtension();

            Logger.Log(Resources.ConfiguringUnityContainer, Category.Debug, Priority.Low);
            ConfigureContainer();

            Logger.Log(Resources.ConfiguringServiceLocatorSingleton, Category.Debug, Priority.Low);
            ConfigureServiceLocator();

            Logger.Log(Resources.ConfiguringViewModelLocator, Category.Debug, Priority.Low);
            ConfigureViewModelLocator();

            Logger.Log(Resources.ConfiguringRegionAdapters, Category.Debug, Priority.Low);
            ConfigureRegionAdapterMappings();

            Logger.Log(Resources.ConfiguringDefaultRegionBehaviors, Category.Debug, Priority.Low);
            ConfigureDefaultRegionBehaviors();

            Logger.Log(Resources.RegisteringFrameworkExceptionTypes, Category.Debug, Priority.Low);
            RegisterFrameworkExceptionTypes();

            Logger.Log(Resources.CreatingShell, Category.Debug, Priority.Low);
            Shell = CreateShell();
            if (Shell != null)
            {
                Logger.Log(Resources.SettingTheRegionManager, Category.Debug, Priority.Low);
                RegionManager.SetRegionManager(Shell, Container.Resolve<IRegionManager>());

                Logger.Log(Resources.UpdatingRegions, Category.Debug, Priority.Low);
                RegionManager.UpdateRegions();

                Logger.Log(Resources.InitializingShell, Category.Debug, Priority.Low);
                InitializeShell();
            }

            if (ContainerExtension.IsRegistered<IModuleManager>())
            {
                Logger.Log(Resources.InitializingModules, Category.Debug, Priority.Low);
                InitializeModules();
            }

            Logger.Log(Resources.BootstrapperSequenceCompleted, Category.Debug, Priority.Low);
        }

        protected override void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => Container.Resolve<IServiceLocator>());
        }

        protected override void RegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();

            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentResolutionException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentNotFoundException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentRegistrationException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(CircularDependencyException));
        }

        protected virtual void ConfigureContainer()
        {
            Logger.Log(Resources.AddingUnityBootstrapperExtensionToContainer, Category.Debug, Priority.Low);

            ContainerExtension.RegisterInstance<IContainerExtension>(ContainerExtension);
            ContainerExtension.RegisterInstance<ILoggerFacade>(Logger);

            ContainerExtension.RegisterInstance(ModuleCatalog);

            if (_useDefaultConfiguration)
            {
                RegisterTypeIfMissing(typeof(IDialogService), typeof(DialogService), true);
                RegisterTypeIfMissing(typeof(IDialogWindow), typeof(DialogWindow), false);

                RegisterTypeIfMissing(typeof(IServiceLocator), typeof(CastleWinsorServiceLocatorAdapter), true);
                RegisterTypeIfMissing(typeof(IModuleInitializer), typeof(ModuleInitializer), true);
                RegisterTypeIfMissing(typeof(IModuleManager), typeof(ModuleManager), true);
                RegisterTypeIfMissing(typeof(RegionAdapterMappings), typeof(RegionAdapterMappings), true);
                RegisterTypeIfMissing(typeof(IRegionManager), typeof(RegionManager), true);
                RegisterTypeIfMissing(typeof(IEventAggregator), typeof(EventAggregator), true);
                RegisterTypeIfMissing(typeof(IRegionViewRegistry), typeof(RegionViewRegistry), true);
                RegisterTypeIfMissing(typeof(IRegionBehaviorFactory), typeof(RegionBehaviorFactory), true);
                RegisterTypeIfMissing(typeof(IRegionNavigationJournalEntry), typeof(RegionNavigationJournalEntry), false);
                RegisterTypeIfMissing(typeof(IRegionNavigationJournal), typeof(RegionNavigationJournal), false);
                RegisterTypeIfMissing(typeof(IRegionNavigationService), typeof(RegionNavigationService), false);
                RegisterTypeIfMissing(typeof(IRegionNavigationContentLoader), typeof(CastleWinsorRegionNavigationContentLoader), true);
            }
        }

        protected override void InitializeModules()
        {
            IModuleManager manager;

            try
            {
                manager = Container.Resolve<IModuleManager>();
            }
            catch (ComponentNotFoundException ex)
            {
                if (ex.Message.Contains("IModuleCatalog"))
                {
                    throw new InvalidOperationException(Resources.NullModuleCatalogException);
                }

                throw;
            }

            manager.Run();
        }

        protected virtual IWindsorContainer CreateContainer()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            return container;
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            return new CastleWindsorExtension(Container);
        }

        protected void RegisterTypeIfMissing(Type fromType, Type toType, bool registerAsSingleton)
        {
            if (fromType == null)
            {
                throw new ArgumentNullException(nameof(fromType));
            }
            if (toType == null)
            {
                throw new ArgumentNullException(nameof(toType));
            }
            if (IsRegistered(fromType))
            {
                Logger.Log(
                    string.Format(CultureInfo.CurrentCulture,
                                  Resources.TypeMappingAlreadyRegistered,
                                  fromType.Name), Category.Debug, Priority.Low);
            }
            else
            {
                if (registerAsSingleton)
                {
                    Container.Register(Component.For(fromType).ImplementedBy(toType).LifestyleSingleton());
                }
                else
                {
                    Container.Register(Component.For(fromType).ImplementedBy(toType));
                }
            }
        }

        protected virtual bool IsRegistered(Type fromType)
        {
            try
            {
                Container.Resolve(fromType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings instance = ServiceLocator.Current.GetInstance<RegionAdapterMappings>();
            if (instance != null)
            {
                instance.RegisterMapping(typeof(Selector), ServiceLocator.Current.GetInstance<SelectorRegionAdapter>());
                instance.RegisterMapping(typeof(ItemsControl), ServiceLocator.Current.GetInstance<ItemsControlRegionAdapter>());
                instance.RegisterMapping(typeof(ContentControl), ServiceLocator.Current.GetInstance<ContentControlRegionAdapter>());
            }
            return instance;
        }
    }
}
