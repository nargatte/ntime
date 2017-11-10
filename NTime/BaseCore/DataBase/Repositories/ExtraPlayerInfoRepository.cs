using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class ExtraPlayerInfoRepository : RepositoryCompetitionId<ExtraPlayerInfo>
    {
        public ExtraPlayerInfoRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {
        }

        protected override IQueryable<ExtraPlayerInfo> GetSortQuery(IQueryable<ExtraPlayerInfo> items) =>
            items.OrderBy(e => e.Name);
    }
}