using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public abstract class RepositoryCompetitionId<T> : Repository<T>
        where T: class, IEntityId, ICompetitionId
    {
        private Competition _competition;

        public Competition Competition
        {
            get
            {
                if (_competition == null)
                    throw new NullReferenceException("Competition is unset");
                return _competition;
            }
            set => _competition = value;
        }

        protected RepositoryCompetitionId(IContextProvider contextProvider) : base(contextProvider)
        {
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