using BaseCore.Csv.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.Interfaces
{
    public interface IScoreFilter
    {
        (IEnumerable<PlayerScoreRecord> filteredScores, IEnumerable<PlayerScoreRecord> dnfs) FilterCorrectScores(
            IEnumerable<PlayerScoreRecord> scores);
    }
}
