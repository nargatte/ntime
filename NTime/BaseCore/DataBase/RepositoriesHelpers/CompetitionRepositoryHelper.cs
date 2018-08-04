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

            await playerRepository.ModifyExtraData(permutationPairs.Select(pp => pp.PermutationElement).ToArray());

            competition.ExtraDataHeaders = String.Join(";", permutationPairs.Select(pp => pp.HeaderName).ToArray());
            await competitionRepository.UpdateAsync(competition);
        }
    }
}
