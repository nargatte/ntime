using System;
using System.Linq;

namespace BaseCore.DataBase
{
    public class AgeCategoryTemplateItemRepository : Repository<AgeCategoryTemplate>
    {
        public AgeCategoryTemplateItemRepository(IContextProvider contextProvider, AgeCategoryCollection ageCategoryCollection) : base(contextProvider) => AgeCategoryCollection = ageCategoryCollection;

        protected AgeCategoryCollection AgeCategoryCollection { get; set; }

        protected override IQueryable<AgeCategoryTemplate> GetAllQuery(IQueryable<AgeCategoryTemplate> items) =>
            items.Where(i => i.AgeCategoryCollectionId == AgeCategoryCollection.Id);

        protected override IQueryable<AgeCategoryTemplate> GetSortQuery(IQueryable<AgeCategoryTemplate> items) =>
            items.OrderBy(i => i.YearFrom);

        protected override void CheckItem(AgeCategoryTemplate item)
        {
            if (item.AgeCategoryCollectionId != AgeCategoryCollection.Id)
                throw new ArgumentException("Wrong AgeCategoryCollectionId");
        }

        protected override void PrepareToAdd(AgeCategoryTemplate item)
        {
            item.AgeCategoryCollectionId = AgeCategoryCollection.Id;
            item.AgeCategoryCollection = null;
        }
    }
}