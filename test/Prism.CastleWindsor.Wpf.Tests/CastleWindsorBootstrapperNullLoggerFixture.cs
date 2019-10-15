
using System;
using System.Windows;

using Prism.CastleWindsor.Legacy;
using Prism.IocContainer.Wpf.Tests.Support;
using Prism.Logging;

using Xunit;

namespace Prism.CastleWindsor.Wpf.Tests
{
    public class CastleWindsorBootstrapperNullLoggerFixture : BootstrapperFixtureBase
    {
        [Fact]
        public void NullLoggerThrows()
        {
            var bootstrapper = new NullLoggerBootstrapper();

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "ILoggerFacade");
        }

        internal class NullLoggerBootstrapper : CastleWindsorBootstrapper
        {
            protected override ILoggerFacade CreateLogger()
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