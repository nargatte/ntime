using BaseCore.Csv.CompetitionSeries.Interfaces;
using BaseCore.Csv.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.PlacesAndPoints
{
    public class PlacesAndPointsScoreFilter : IScoreFilter
    {
        public (IEnumerable<PlayerScoreRecord> filteredScores, IEnumerable<PlayerScoreRecord> dnfs) FilterCorrectScores(
            IEnumerable<PlayerScoreRecord> scores)
        {
            int limit = 20000;
            var dnfs = scores.Where(x => x.IsDNF());
            foreach (var score in dnfs)
            {
                score.DistancePlaceNumber = 0;
                score.CategoryPlaceNumber = 0;
            }
            var filteredScores = scores
                .Where(x => x.DistancePlaceNumber > 0 && x.CategoryPlaceNumber > 0
                    && x.DistancePlaceNumber < limit && x.CategoryPlaceNumber < limit)
                .Union(dnfs);
            filteredScores.ToList().ForEach(score => score.StartedInCompetition = true);
            return (filteredScores, dnfs);
        }
    }
}
