using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public abstract class RepositoryCompetitionId<T> : Repository<T>
        where T: class, IEntityId, ICompetitionId
    {

        protected Competition Competition { get; }


        protected RepositoryCompetitionId(IContextProvider contextProvider, Competition competition) :
            base(contextProvider) =>
            Competition = competition;

        public async Task<IEnumerable<T>>  GetAllForCompetitionAsync()
        {
            var allItems = await base.GetAllAsync();
            return allItems.Where(item => item.CompetitionId == Competition.Id);
        }

        protected override IQueryable<T> GetAllQuery(IQueryable<T> items) => 
            items.Where(e => e.CompetitionId == Competition.Id);

        protected override void CheckItem(T item)
        {
            if (item.CompetitionId != Competition.Id) throw new ArgumentException("Wrong CompetitionId");
        }

        protected override void PrepareToAdd(T item)
        {
            item.CompetitionId = Competition.Id;
            item.Competition = null;
        }
    }
}