using BaseCore.DataBase.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.DataBase.Entities
{
    public class SeriesStandingsParameters
    {
        public CompetitionStandingsType StandingsType { get; set; }
        public bool StrictMinStartsEnabled { get; set; }
        public int StrictMinStartsCount { get; set; }
        public bool MinStartsEnabled { get; set; }
        public int MinStartsCount { get; set; }
        public bool SortByStartsCountFirst { get; set; }
        public bool BestScoresEnabled { get; set; }
        public int BestCompetitionsCount { get; set; }
    }
}
