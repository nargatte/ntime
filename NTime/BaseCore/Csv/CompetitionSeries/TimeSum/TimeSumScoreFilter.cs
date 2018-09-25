using BaseCore.Csv.CompetitionSeries.Interfaces;
using BaseCore.Csv.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.TimeSum
{
    public class TimeSumScoreFilter : IScoreFilter
    {
        public (IEnumerable<PlayerScoreRecord> filteredScores, IEnumerable<PlayerScoreRecord> dnfs) FilterCorrectScores(
            IEnumerable<PlayerScoreRecord> scores)
        {
            var dnfs = scores.Where(x => x.IsDNF());
            foreach (var score in dnfs)
            {
                score.DistancePlaceNumber = 0;
                score.CategoryPlaceNumber = 0;
                score.RaceTime = TimeSpan.Zero;
            }
            var filteredScores = scores
                .Where(x => x.RaceTime!= null && x.RaceTime != TimeSpan.Zero)
                .Union(dnfs);
            foreach (var score in filteredScores)
                score.StartedInCompetition = true;
            return (filteredScores, dnfs);
        }
    }
}
