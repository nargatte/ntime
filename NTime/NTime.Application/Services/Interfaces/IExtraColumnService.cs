using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NTime.Application.Services
{
    public interface IExtraColumnService
    {
        Task<ExtraColumn> AddExtraColumn(int competitionId, ExtraColumn extraColumn);
        Task<ExtraColumn[]> GetExtraColumnsForCompetition();
        Task<ExtraColumn> UpdateExtraColumn(int competitionId, ExtraColumn extraColumn);
    }
}
