using System;
using System.Collections.Generic;

using Castle.Windsor;

using CommonServiceLocator;

using Prism.CastleWindsor.Wpf.Regions;
using Prism.Regions;

namespace Prism.CastleWindsor.Wpf.Tests.Mocks
{
    public class MockServiceLocator : ServiceLocatorImplBase
    {
        private readonly IWindsorContainer _container;

        public MockServiceLocator(IWindsorContainer container)
        {
            _container = container;
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (serviceType == typeof(IRegionNavigationService))
            {
                var loader = new CastleWinsorRegionNavigationContentLoader(this, _container);
                return new RegionNavigationService(this, loader, new RegionNavigationJournal());
            }

            return null;
        }
    }
}
