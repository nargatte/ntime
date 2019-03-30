using BaseCore.DataBase;
using BaseCore.DataBase.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<ExtraColumn> AddExtraColumn(int competitionId, ExtraColumn extraColumn)
        {
            throw new NotImplementedException();
        }

        public async Task<ExtraColumn[]> GetExtraColumnsForCompetition()
        {
            return await _extraColumnRepository.GetAllAsync();
        }

        public async Task<ExtraColumn> UpdateExtraColumn(int competitionId, ExtraColumn extraColumn)
        {
            throw new NotImplementedException();
        }
    }
}
