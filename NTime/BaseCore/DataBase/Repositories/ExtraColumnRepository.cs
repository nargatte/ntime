using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.DataBase.Repositories
{
    public class ExtraColumnRepository : RepositoryCompetitionId<ExtraColumn>, IExtraColumnRepository
    {
        public ExtraColumnRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {

        }

        protected override IQueryable<ExtraColumn> GetSortQuery(IQueryable<ExtraColumn> items)
        {
            return items
                .OrderBy(item => item.CompetitionId)
                .ThenByDescending(item => item.SortIndex.HasValue)
                .ThenBy(item => item.SortIndex.Value);
        }

        public override Task RemoveAsync(ExtraColumn item)
        {
            CheckNull(item);
            CheckItem(item);
            return ContextProvider.DoAsync(async ctx =>
            {
                var extraColumnValues = await ctx.ExtraColumnValues.Where(value => value.ColumnId == item.Id).ToListAsync();
                ctx.ExtraColumnValues.RemoveRange(extraColumnValues);

                ctx.ExtraColumns.Attach(item);
                ctx.ExtraColumns.Remove(item);
                await ctx.SaveChangesAsync();
            });
        }


    }
}
