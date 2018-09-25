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

        public bool StrictMinStartsEnabled
        {
            get { return DbEntity.StrictMinStartsEnabled; }
            set { DbEntity.StrictMinStartsEnabled = SetProperty(DbEntity.StrictMinStartsEnabled, value); }
        }


        public int StrictMinStartsCount
        {
            get { return DbEntity.StrictMinStartsCount; }
            set { DbEntity.StrictMinStartsCount = SetProperty(DbEntity.StrictMinStartsCount, value); }
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


        public bool PrintBestOnly
        {
            get { return DbEntity.PrintBestOnly; }
            set { DbEntity.PrintBestOnly = SetProperty(DbEntity.PrintBestOnly, value); }
        }


        public int PrintBestCount
        {
            get { return DbEntity.PrintBestCount; }
            set { DbEntity.PrintBestCount = SetProperty(DbEntity.PrintBestCount, value); }
        }
    }
}
