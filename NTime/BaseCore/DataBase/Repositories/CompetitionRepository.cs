using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class CompetitionRepository : Repository<Competition>
    {
        public CompetitionRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        protected override IQueryable<Competition> GetSortQuery(IQueryable<Competition> items) =>
            items.OrderByDescending(e => e.EventDate);

    }
}