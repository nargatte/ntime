using BaseCore.DataBase;
using BaseCore.DataBase.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace NTime.Application.Services
{
    public class ExtraColumnService : IExtraColumnService
    {
        private readonly IExtraColumnRepository _extraColumnRepository;
        private readonly Competition _competition;

        public ExtraColumnService(Competition competition)
        {
            _extraColumnRepository = new ExtraColumnRepository(new ContextProvider(), competition);
            _competition = competition;
        }

        public async Task AddExtraColumn(ExtraColumn extraColumn)
        {
            await _extraColumnRepository.AddAsync(extraColumn);
        }

        public async Task<ExtraColumn[]> GetExtraColumnsForCompetition()
        {
            return await _extraColumnRepository.GetAllAsync();
        }

        public async Task UpdateExtraColumns(IEnumerable<ExtraColumn> extraColumns)
        {
            await _extraColumnRepository.UpdateRangeAsync(extraColumns);
        }

        public async Task RemoveExtraColumn(ExtraColumn extraColumn)
        {
            await _extraColumnRepository.RemoveAsync(extraColumn);
        }

        // TODO: Write tests for this method
        public bool ExtraColumnsChanged(IEnumerable<ExtraColumn> originalColumns, IEnumerable<ExtraColumn> updatedColumns)
        {
            return !originalColumns.SequenceEqual(updatedColumns, new ExtraColumnEqualityComparer());
        }

        public List<ExtraColumn> GetExtraColumnsWithSortIndices(IEnumerable<ExtraColumn> extraColumns)
        {
            var extraColumnsCopy = new List<ExtraColumn>(extraColumns);
            for (int i = 0; i < extraColumnsCopy.Count; i++)
            {
                extraColumnsCopy[i].SortIndex = i;
            }
            return extraColumnsCopy;
        }
    }
}
