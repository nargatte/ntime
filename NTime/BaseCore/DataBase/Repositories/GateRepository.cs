using System.Data.Entity;
using System.Linq;

namespace BaseCore.DataBase
{
    public class GateRepository : RepositoryCompetitionId<Gate>
    {
        public GateRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {
        }

        protected override IQueryable<Gate> GetSortQuery(IQueryable<Gate> items) =>
            items.OrderBy(i => i.Number);
    }
}