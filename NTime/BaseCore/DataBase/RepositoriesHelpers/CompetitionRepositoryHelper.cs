using BaseCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.DataBase.RepositoriesHelpers
{
    public static class CompetitionRepositoryHelper
    {
        public static async Task ModifyExtraDataHeaders(HeaderPermutationPair[] permutationPairs, Competition competition)
        {
            var playerRepository = new PlayerRepository(new ContextProvider(), competition);
            var competitionRepository = new CompetitionRepository(new ContextProvider());
            char delimiter = competitionRepository.DelimiterForExtraData;

            await playerRepository.ModifyExtraData(permutationPairs.Select(pp => pp.PermutationElement).ToArray(), delimiter);

            competition.ExtraDataHeaders = String.Join(delimiter.ToString(), permutationPairs.Select(pp => pp.HeaderName).ToArray());
            await competitionRepository.UpdateAsync(competition);
        }
    }
}
