using BaseCore.DataBase.Entities;
using BaseCore.DataBase.Enums;
using MvvmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public class EditableSeriesStandingsParameters : BindableBase
    {

        public SeriesStandingsParameters DbEntity { get; set; } = new SeriesStandingsParameters();

        public CompetitionStandingsType StandingsType
        {
            get { return DbEntity.StandingsType; }
            set { DbEntity.StandingsType = SetProperty(DbEntity.StandingsType, value); }
        }

        public bool MinStartsEnabled
        {
            get { return DbEntity.MinStartsEnabled; }
            set { DbEntity.MinStartsEnabled = SetProperty(DbEntity.MinStartsEnabled, value); }
        }


        public int MinStartsCount
        {
            get { return DbEntity.MinStartsCount; }
            set { DbEntity.MinStartsCount = SetProperty(DbEntity.MinStartsCount, value); }
        }

        public bool SortByStartsCountFirst
        {
            get { return DbEntity.SortByStartsCountFirst; }
            set { DbEntity.SortByStartsCountFirst = SetProperty(DbEntity.SortByStartsCountFirst, value); }
        }

        public bool BestScoresEnabled
        {
            get { return DbEntity.BestScoresEnabled; }
            set { DbEntity.BestScoresEnabled = SetProperty(DbEntity.BestScoresEnabled, value); }
        }

        public int BestCompetitionsCount
        {
            get { return DbEntity.BestCompetitionsCount; }
            set { DbEntity.BestCompetitionsCount = SetProperty(DbEntity.BestCompetitionsCount, value); }
        }
    }
}
