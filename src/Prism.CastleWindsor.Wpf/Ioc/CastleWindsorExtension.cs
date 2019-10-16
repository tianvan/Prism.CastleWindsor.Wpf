using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Prism.Ioc;

namespace Prism.CastleWindsor.Ioc
{
    public class CastleWindsorExtension : IContainerExtension<IWindsorContainer>
    {
        public IWindsorContainer Instance { get; }

        public CastleWindsorExtension() : this(new WindsorContainer())
        {
        }

        public CastleWindsorExtension(IWindsorContainer container)
        {
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            //container.Register(Component.For<IContainerExtension>().Instance(this));

            Instance = container;
        }

        public void FinalizeExtension()
        {
        }

        public bool IsRegistered(Type type)
        {
            try
            {
                var instance = Instance.Resolve(type);
                return true;
            }
            catch (ComponentNotFoundException)
            {
                return false;
            }
        }

        public bool IsRegistered(Type type, string name)
        {
            try
            {
                var instance = Instance.Resolve(name, type);
                return true;
            }
            catch (ComponentNotFoundException)
            {
                return false;
            }
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).LifestyleTransient());
            return this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).Named(name).LifestyleTransient());
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Instance.Register(Component.For(type).Instance(instance));
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Instance.Register(Component.For(type).Instance(instance).Named(name));
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).LifestyleSingleton());
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).Named(name).LifestyleSingleton());
            return this;
        }

        public object Resolve(Type type)
        {
            try
            {
                return Instance.Resolve(type);
            }
            catch (ComponentNotFoundException)
            {
                Instance.Register(Component.For(type).LifestyleTransient());
                return Instance.Resolve(type);
            }
        }

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            Arguments arguments = ConvertToArguments(parameters);

            try
            {

                return Instance.Resolve(type, arguments);
            }
            catch (ComponentNotFoundException)
            {
                Instance.Register(Component.For(type).LifestyleTransient().DependsOn(arguments));

                return Instance.Resolve(type, arguments);
            }
        }

        private static Arguments ConvertToArguments((Type Type, object Instance)[] parameters)
        {
            var pairs = new Collection<KeyValuePair<Type, object>>();
            foreach ((Type Type, object Instance) item in parameters)
            {
                pairs.Add(new KeyValuePair<Type, object>(item.Type, item.Instance));
            }

            return Arguments.FromTyped(pairs);
        }

        public object Resolve(Type type, string name)
        {
            try
            {

                return Instance.Resolve(name, type);
            }
            catch (ComponentNotFoundException)
            {
                Instance.Register(Component.For(type).Named(name).LifestyleTransient());
                return Instance.Resolve(name, type);
            }
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            Arguments arguments = ConvertToArguments(parameters);

            try
            {

                return Instance.Resolve(name, type, arguments);
            }
            catch (ComponentNotFoundException)
            {
                Instance.Register(Component.For(type).Named(name).LifestyleTransient().DependsOn(arguments));

                return Instance.Resolve(name, type, arguments);
            }
        }
    }
}
