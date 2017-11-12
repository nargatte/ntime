using System.Linq;

namespace BaseCore.DataBase
{
    public class DistanceRepository : RepositoryCompetitionId<Distance>
    {
        public DistanceRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {
        }

        protected override IQueryable<Distance> GetSortQuery(IQueryable<Distance> items) =>
            items.OrderBy(e => e.Name);
    }
}