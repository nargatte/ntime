using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class AgeCategoryTemplateRepository : Repository<AgeCategoryTemplate>
    {
        public AgeCategoryTemplateRepository(IContextProvider contextProvider, AgeCategoryCollection ageCategoryCollection) : base(contextProvider) => AgeCategoryCollection = ageCategoryCollection;

        public AgeCategoryCollection AgeCategoryCollection { get; set; }

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