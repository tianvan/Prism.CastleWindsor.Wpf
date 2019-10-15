using System.Linq;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using CommonServiceLocator;

using Prism.CastleWindsor.Wpf.Tests.Mocks;
using Prism.IocContainer.Wpf.Tests.Support.Mocks.Views;
using Prism.Regions;

using Xunit;

namespace Prism.CastleWindsor.Wpf.Tests
{
    [Collection("ServiceLocator")]
    public class CastleWindsorRegionNavigationContentLoaderFixture
    {
        private readonly IWindsorContainer _container;

        public CastleWindsorRegionNavigationContentLoaderFixture()
        {
            _container = new WindsorContainer();
            var serviceLocator = new MockServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        [StaFact]
        public void ShouldFindCandidateViewInRegion()
        {
            _container.Register(Component.For<object>().ImplementedBy<MockView>().Named("MockView"));

            // We cannot access the UnityRegionNavigationContentLoader directly so we need to call its
            // GetCandidatesFromRegion method through a navigation request.
            IRegion testRegion = new Region();

            var view = new MockView();
            testRegion.Add(view);
            testRegion.Deactivate(view);

            testRegion.RequestNavigate("MockView");

            Assert.True(testRegion.Views.Contains(view));
            Assert.True(testRegion.Views.Count() == 1);
            Assert.True(testRegion.ActiveViews.Count() == 1);
            Assert.True(testRegion.ActiveViews.Contains(view));
        }

        [StaFact]
        public void ShouldFindCandidateViewWithFriendlyNameInRegion()
        {
            _container.Register(Component.For<object>().ImplementedBy<MockView>().Named("SomeView"));

            // We cannot access the UnityRegionNavigationContentLoader directly so we need to call its
            // GetCandidatesFromRegion method through a navigation request.
            IRegion testRegion = new Region();

            var view = new MockView();
            testRegion.Add(view);
            testRegion.Deactivate(view);

            testRegion.RequestNavigate("SomeView");

            Assert.True(testRegion.Views.Contains(view));
            Assert.True(testRegion.ActiveViews.Count() == 1);
            Assert.True(testRegion.ActiveViews.Contains(view));
        }
    }
}
