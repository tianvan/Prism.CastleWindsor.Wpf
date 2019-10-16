using System;
using System.Collections.Generic;
using System.Linq;

using Castle.Windsor;

using CommonServiceLocator;

namespace Prism.CastleWindsor
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
            try
            {
                return key == null ? _container.Resolve(serviceType) : _container.Resolve(key, serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}