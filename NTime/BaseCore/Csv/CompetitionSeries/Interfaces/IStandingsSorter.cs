using BaseCore.Csv.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.Interfaces
{
    public interface IStandingsSorter
    {
        List<PlayerWithScores> SortStandings(Dictionary<string, List<PlayerWithScores>> categorieStandings, bool verbose);   
    }
}
