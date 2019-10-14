using System;
using System.Collections.Generic;
using System.Linq;

using Castle.Windsor;

using CommonServiceLocator;

namespace Prism.CastleWindsor.Wpf
{
    public class CastleWinsorServiceLocatorAdapter : ServiceLocatorImplBase
    {
        private readonly IWindsorContainer _container;

        public CastleWinsorServiceLocatorAdapter(IWindsorContainer container)
        {
            _container = container;
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _container.ResolveAll(serviceType).Cast<object>();
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return _container.Resolve(key, serviceType);
        }
    }
}