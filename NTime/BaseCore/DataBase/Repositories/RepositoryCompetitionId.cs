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

        public override Task UpdateAsync(T item)
        {
            return base.UpdateAsync(item);
        }

        public override Task UpdateRangeAsync(IEnumerable<T> items)
        {
            return base.UpdateRangeAsync(items);
        }

        public override Task RemoveAsync(T item)
        {
            return base.RemoveAsync(item);
        }

        public override Task RemoveAllAsync()
        {
            return base.RemoveAllAsync();
        }

        public override Task RemoveRangeAsync(IEnumerable<T> items)
        {
            return base.RemoveRangeAsync(items);
        }
    }
}