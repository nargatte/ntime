using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class DistanceRepository : RepositoryCompetitionId<Distance>
    {
        public DistanceRepository(Competition competition) : base(competition)
        {
        }

        protected override IQueryable<Distance> GetSortQuery(IQueryable<Distance> items) =>
            items.OrderBy(e => e.Name);
    }
}