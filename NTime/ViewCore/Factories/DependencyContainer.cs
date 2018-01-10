using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Competitions;
using ViewCore.Factories.Distances;
using ViewCore.Factories.ExtraPlayerInfos;
using ViewCore.Factories.Players;

namespace ViewCore.Factories
{
    public class DependencyContainer
    {
        public DependencyContainer(IAgeCategoryManagerFactory ageCategoryManagerFactory, ICompetitionManagerFactory competitionManagerFactory,
            IDistanceManagerFactory distanceManagerFactory, IExtraPlayerInfoManagerFactory extraPlayerInfoManagerFactory,
            IPlayerManagerFactory playerManagerFactory)
        {
            AgeCategoryManagerFactory = ageCategoryManagerFactory;
            CompetitionManagerFactory = competitionManagerFactory;
            DistanceManagerFactory = distanceManagerFactory;
            ExtraPlayerInfoManagerFactory = extraPlayerInfoManagerFactory;
            PlayerManagerFactory = playerManagerFactory;
        }

        public DependencyContainer(IAgeCategoryManagerFactory ageCategoryManagerFactory, ICompetitionManagerFactory competitionManagerFactory,
            IDistanceManagerFactory distanceManagerFactory, IExtraPlayerInfoManagerFactory extraPlayerInfoManagerFactory,
            IPlayerManagerFactory playerManagerFactory, AccountInfo user, ConnectionInfo connectionInfo) :
            this(ageCategoryManagerFactory, competitionManagerFactory, distanceManagerFactory, extraPlayerInfoManagerFactory, playerManagerFactory)
        {
            User = user;
            ConnectionInfo = connectionInfo;
        }

        public IAgeCategoryManagerFactory AgeCategoryManagerFactory { get; private set; }
        public ICompetitionManagerFactory CompetitionManagerFactory { get; private set; }
        public IDistanceManagerFactory DistanceManagerFactory { get; private set; }
        public IExtraPlayerInfoManagerFactory ExtraPlayerInfoManagerFactory { get; private set; }
        public IPlayerManagerFactory PlayerManagerFactory { get; private set; }
        public AccountInfo User { get; set; }
        public ConnectionInfo ConnectionInfo { get; set; }
    }
}
