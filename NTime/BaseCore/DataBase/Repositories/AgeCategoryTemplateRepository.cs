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
        private AgeCategoryCollection _ageCategoryCollection;

        public AgeCategoryTemplateRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        public AgeCategoryCollection AgeCategoryCollection
        {
            get
            {
                if (_ageCategoryCollection == null)
                    throw new NullReferenceException("AgeCategoryCollection is unset");
                return _ageCategoryCollection;
            }
            set => _ageCategoryCollection = value;
        }

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