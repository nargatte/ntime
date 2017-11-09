using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class AgeCategoryCollectionRepository : Repository<AgeCategoryCollection>
    {
        protected override IQueryable<AgeCategoryCollection> GetSortQuery(IQueryable<AgeCategoryCollection> items) =>
            items.OrderBy(i => i.Name);
    }
}