
using System.Collections.Generic;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Prism.CastleWindsor.Ioc;
using Prism.IocContainer.Wpf.Tests.Support.Mocks;

using Xunit;

namespace Prism.CastleWindsor.Wpf.Tests
{
    public class CastleWindsorContainerExtensionFixture
    {
        [Fact]
        public void ExtensionReturnsTrueIfThereIsAPolicyForType()
        {
            var container = new WindsorContainer();
            var containerExtension = new CastleWindsorExtension(container);

            container.Register(Component.For<object>().ImplementedBy<string>());

            Assert.True(containerExtension.IsRegistered(typeof(object)));
            Assert.False(containerExtension.IsRegistered(typeof(int)));

            container.Register(Component.For<IList<int>>().ImplementedBy<List<int>>());

            Assert.True(containerExtension.IsRegistered(typeof(IList<int>)));
            Assert.False(containerExtension.IsRegistered(typeof(IList<string>)));

            container.Register(Component.For(typeof(IDictionary<,>)).ImplementedBy(typeof(Dictionary<,>)));
            Assert.True(containerExtension.IsRegistered(typeof(IDictionary<,>)));
        }

        [Fact]
        public void TryResolveShouldResolveTheElementIfElementExist()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IService>().ImplementedBy<MockService>());

            object dependantA = container.Resolve<IService>();
            Assert.NotNull(dependantA);
        }

        [Fact]
        public void TryResolveShouldReturnNullIfElementNotExist()
        {
            var container = new WindsorContainer();

            object dependantA = container.Resolve<IDependantA>();
            Assert.Null(dependantA);
        }

        [Fact]
        public void TryResolveWorksWithValueTypes()
        {
            var container = new WindsorContainer();

            var valueType = container.Resolve<int>();
            Assert.Equal(default, valueType);
        }
    }
}