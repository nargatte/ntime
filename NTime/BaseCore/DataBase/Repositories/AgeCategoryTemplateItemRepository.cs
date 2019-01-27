using System;
using System.Linq;

namespace BaseCore.DataBase
{
    public class AgeCategoryTemplateItemRepository : Repository<AgeCategoryTemplateItem>
    {
        public AgeCategoryTemplateItemRepository(IContextProvider contextProvider, AgeCategoryTemplate ageCategoryCollection) : base(contextProvider) => AgeCategoryCollection = ageCategoryCollection;

        protected AgeCategoryTemplate AgeCategoryCollection { get; set; }

        protected override IQueryable<AgeCategoryTemplateItem> GetAllQuery(IQueryable<AgeCategoryTemplateItem> items) =>
            items.Where(i => i.AgeCategoryCollectionId == AgeCategoryCollection.Id);

        protected override IQueryable<AgeCategoryTemplateItem> GetSortQuery(IQueryable<AgeCategoryTemplateItem> items) =>
            items.OrderBy(i => i.YearFrom);

        protected override void CheckItem(AgeCategoryTemplateItem item)
        {
            if (item.AgeCategoryCollectionId != AgeCategoryCollection.Id)
                throw new ArgumentException("Wrong AgeCategoryCollectionId");
        }

        protected override void PrepareToAdd(AgeCategoryTemplateItem item)
        {
            item.AgeCategoryCollectionId = AgeCategoryCollection.Id;
            item.AgeCategoryCollection = null;
        }


    }
}