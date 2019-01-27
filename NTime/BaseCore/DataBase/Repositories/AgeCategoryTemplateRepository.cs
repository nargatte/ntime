using System.Linq;

namespace BaseCore.DataBase
{
    public class AgeCategoryTemplateRepository : Repository<AgeCategoryCollection>
    {
        public AgeCategoryTemplateRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        protected override IQueryable<AgeCategoryCollection> GetSortQuery(IQueryable<AgeCategoryCollection> items) =>
            items.OrderBy(i => i.Name);
    }
}