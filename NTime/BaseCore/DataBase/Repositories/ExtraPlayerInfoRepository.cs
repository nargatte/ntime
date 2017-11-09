using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class ExtraPlayerInfoRepository : RepositoryCompetitionId<ExtraPlayerInfo>
    {
        public ExtraPlayerInfoRepository(Competition competition) : base(competition)
        {
        }

        protected override IQueryable<ExtraPlayerInfo> GetSortQuery(IQueryable<ExtraPlayerInfo> items) =>
            items.OrderBy(e => e.Name);
    }
}