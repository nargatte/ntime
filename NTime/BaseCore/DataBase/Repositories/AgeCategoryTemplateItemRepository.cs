using System;
using System.Linq;

namespace BaseCore.DataBase
{
    public class AgeCategoryTemplateItemRepository : Repository<AgeCategoryTemplateItem>
    {
        public AgeCategoryTemplateItemRepository(IContextProvider contextProvider, AgeCategoryTemplate ageCategoryTemplate) : base(contextProvider) => AgeCategoryTemplate = ageCategoryTemplate;

        public AgeCategoryTemplate AgeCategoryTemplate { get; protected set; }

        protected override IQueryable<AgeCategoryTemplateItem> GetAllQuery(IQueryable<AgeCategoryTemplateItem> items) =>
            items.Where(i => i.AgeCategoryTemplateId == AgeCategoryTemplate.Id);

        protected override IQueryable<AgeCategoryTemplateItem> GetSortQuery(IQueryable<AgeCategoryTemplateItem> items) =>
            items.OrderBy(i => i.YearFrom);

        protected override void CheckItem(AgeCategoryTemplateItem item)
        {
            if (item.AgeCategoryTemplateId != AgeCategoryTemplate.Id)
                throw new ArgumentException("Wrong AgeCategoryCollectionId");
        }

        protected override void PrepareToAdd(AgeCategoryTemplateItem item)
        {
            item.AgeCategoryTemplateId = AgeCategoryTemplate.Id;
            item.AgeCategoryTemplate = null;
        }


    }
}