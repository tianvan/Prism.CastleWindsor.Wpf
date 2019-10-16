using System;
using System.Collections.Generic;

using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using CommonServiceLocator;

using Xunit;

namespace Prism.CastleWindsor.Wpf.Tests
{
    public class CastleWindsorServiceLocatorAdapterFixture
    {
        [Fact]
        public void ShouldForwardResolveToInnerContainer()
        {
            var myInstance = new object();

            IWindsorContainer container = new MockWindsorContainer()
            {
                ResolveMethod = delegate
                                    {
                                        return myInstance;
                                    }
            };

            IServiceLocator containerAdapter = new CastleWinsorServiceLocatorAdapter(container);

            Assert.Same(myInstance, containerAdapter.GetInstance(typeof(object)));
        }

        [Fact]
        public void ShouldForwardResolveAllToInnerContainer()
        {
            IEnumerable<object> list = new List<object> { new object(), new object() };

            IWindsorContainer container = new MockWindsorContainer()
            {
                ResolveMethod = delegate
                {
                    return list;
                }
            };

            IServiceLocator containerAdapter = new CastleWinsorServiceLocatorAdapter(container);

            Assert.Same(list, containerAdapter.GetAllInstances(typeof(object)));
        }

        private class MockWindsorContainer : IWindsorContainer
        {
            public Func<object> ResolveMethod { get; set; }
            public IKernel Kernel { get; }
            public string Name { get; }
            public IWindsorContainer Parent { get; set; }

            public void AddChildContainer(IWindsorContainer childContainer)
            {
                throw new NotImplementedException();
            }

            public IWindsorContainer AddFacility(IFacility facility)
            {
                throw new NotImplementedException();
            }

            public IWindsorContainer AddFacility<TFacility>() where TFacility : IFacility, new()
            {
                throw new NotImplementedException();
            }

            public IWindsorContainer AddFacility<TFacility>(Action<TFacility> onCreate) where TFacility : IFacility, new()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public IWindsorContainer GetChildContainer(string name)
            {
                throw new NotImplementedException();
            }

            public IWindsorContainer Install(params IWindsorInstaller[] installers)
            {
                throw new NotImplementedException();
            }

            public IWindsorContainer Register(params IRegistration[] registrations)
            {
                throw new NotImplementedException();
            }

            public void Release(object instance)
            {
                throw new NotImplementedException();
            }

            public void RemoveChildContainer(IWindsorContainer childContainer)
            {
                throw new NotImplementedException();
            }

            public object Resolve(string key, Type service)
            {
                return ResolveMethod();
            }

            public object Resolve(Type service)
            {
                throw new NotImplementedException();
            }

            public object Resolve(Type service, Arguments arguments)
            {
                throw new NotImplementedException();
            }

            public T Resolve<T>()
            {
                throw new NotImplementedException();
            }

            public T Resolve<T>(Arguments arguments)
            {
                throw new NotImplementedException();
            }

            public T Resolve<T>(string key)
            {
                throw new NotImplementedException();
            }

            public T Resolve<T>(string key, Arguments arguments)
            {
                throw new NotImplementedException();
            }

            public object Resolve(string key, Type service, Arguments arguments)
            {
                throw new NotImplementedException();
            }

            public T[] ResolveAll<T>()
            {
                throw new NotImplementedException();
            }

            public Array ResolveAll(Type service)
            {
                throw new NotImplementedException();
            }

            public Array ResolveAll(Type service, Arguments arguments)
            {
                throw new NotImplementedException();
            }

            public T[] ResolveAll<T>(Arguments arguments)
            {
                throw new NotImplementedException();
            }
        }
    }
}