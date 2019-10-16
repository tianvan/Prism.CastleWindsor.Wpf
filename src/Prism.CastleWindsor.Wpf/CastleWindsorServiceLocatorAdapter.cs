using System;
using System.Collections.Generic;
using System.Linq;

using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using CommonServiceLocator;

namespace Prism.CastleWindsor
{
    public class CastleWindsorServiceLocatorAdapter : ServiceLocatorImplBase
    {
        private readonly IWindsorContainer _container;

        public CastleWindsorServiceLocatorAdapter(IWindsorContainer container)
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
            catch (ComponentNotFoundException)
            {
                if (key == null)
                {
                    _container.Register(Component.For(serviceType).LifestyleTransient());
                    return _container.Resolve(serviceType);
                }
                else
                {
                    _container.Register(Component.For(serviceType).Named(key).LifestyleTransient());
                    return _container.Resolve(key, serviceType);
                }
            }
        }
    }
}