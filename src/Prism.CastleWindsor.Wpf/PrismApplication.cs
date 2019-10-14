using Prism.CastleWindsor.Wpf.Ioc;
using Prism.Ioc;

namespace Prism.CastleWindsor.Wpf
{
    public abstract class PrismApplication : PrismApplicationBase
    {
        protected override IContainerExtension CreateContainerExtension()
        {
            return new CastleWindsorExtension();
        }
    }
}
