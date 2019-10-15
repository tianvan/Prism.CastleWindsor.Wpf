
using System;
using System.Collections.Generic;
using System.Linq;

using Castle.Windsor;

using CommonServiceLocator;

using Prism.Regions;

namespace Prism.CastleWindsor.Regions
{
    public class CastleWinsorRegionNavigationContentLoader : RegionNavigationContentLoader
    {
        private readonly IWindsorContainer _container;

        public CastleWinsorRegionNavigationContentLoader(IServiceLocator serviceLocator, IWindsorContainer container) : base(serviceLocator)
        {
            _container = container;
        }

        protected override IEnumerable<object> GetCandidatesFromRegion(IRegion region, string candidateNavigationContract)
        {
            if (candidateNavigationContract == null || candidateNavigationContract.Equals(string.Empty))
            {
                throw new ArgumentNullException(nameof(candidateNavigationContract));
            }

            IEnumerable<object> contractCandidates = base.GetCandidatesFromRegion(region, candidateNavigationContract);

            if (!contractCandidates.Any())
            {
                var matchingRegistration = _container.Resolve(candidateNavigationContract, typeof(object));
                if (matchingRegistration == null)
                {
                    return contractCandidates;
                }

                var typeCandidateName = matchingRegistration.GetType().FullName;

                contractCandidates = base.GetCandidatesFromRegion(region, typeCandidateName);
            }

            return contractCandidates;
        }
    }
}
