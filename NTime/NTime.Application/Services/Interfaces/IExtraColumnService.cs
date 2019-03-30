using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NTime.Application.Services
{
    public interface IExtraColumnService
    {
        Task AddExtraColumn(ExtraColumn extraColumn);
        Task<ExtraColumn[]> GetExtraColumnsForCompetition();
        Task UpdateExtraColumns(IEnumerable<ExtraColumn> extraColumn);
        Task RemoveExtraColumn(ExtraColumn extraColumn);
        bool ExtraColumnsChanged(IEnumerable<ExtraColumn> originalColumns, IEnumerable<ExtraColumn> updatedColumns);
        List<ExtraColumn> GetExtraColumnsWithSortIndices(IEnumerable<ExtraColumn> extraColumns);
    }
}
