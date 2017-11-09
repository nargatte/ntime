using System;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class PlayerRepository : RepositoryCompetitionId<Player>
    {
        public PlayerRepository(Competition competition) : base(competition)
        {
        }

        protected override IQueryable<Player> GetSortQuery(IQueryable<Player> items) =>
            items.OrderBy(i => i.LastName);

        public Task<Player[]> GetAllByFiltersAsync(Competition competition, PlayerFiltersOptions filtersOptions, int pageNumber, out int totalPageNumber, int numberItemsOnPage)
        {
            throw new NotImplementedException();
        }
    }
} 