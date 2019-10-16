
using System;
using System.Windows;

using Castle.Windsor;

using Prism.CastleWindsor.Legacy;
using Prism.IocContainer.Wpf.Tests.Support;

using Xunit;

namespace Prism.CastleWindsor.Wpf.Tests
{
    public class CastleWindsorBootstrapperNullContainerFixture : BootstrapperFixtureBase
    {
        [Fact]
        public void RunThrowsWhenNullContainerCreated()
        {
            var bootstrapper = new NullContainerBootstrapper();

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), nameof(IWindsorContainer));
        }

        private class NullContainerBootstrapper : CastleWindsorBootstrapper
        {
            protected override IWindsorContainer CreateContainer()
            {
                return null;
            }
            protected override DependencyObject CreateShell()
            {
                throw new NotImplementedException();
            }

            protected override void InitializeShell()
            {
                throw new NotImplementedException();
            }
        }
    }
}