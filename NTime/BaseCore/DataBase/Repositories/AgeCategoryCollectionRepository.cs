using System.Linq;

namespace BaseCore.DataBase
{
    public class AgeCategoryCollectionRepository : Repository<AgeCategoryCollection>
    {
        public AgeCategoryCollectionRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        protected override IQueryable<AgeCategoryCollection> GetSortQuery(IQueryable<AgeCategoryCollection> items) =>
            items.OrderBy(i => i.Name);
    }
}