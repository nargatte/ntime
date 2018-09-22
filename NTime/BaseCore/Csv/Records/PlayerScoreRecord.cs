using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Records
{
    public class PlayerScoreRecord
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
        public string AgeCategory { get; set; }
        public Subcategory Subcategory { get; set; }
        public string Distance { get; set; }
        public TimeSpan RaceTime { get; set; }
        public int DistancePlaceNumber { get; set; }
        public int CategoryPlaceNumber { get; set; }
        public int LapsCount { get; set; }
        public decimal MeasurementTime { get; set; }
        public int CompetitionId { get; set; }
    }

    public static class ExtensionsForPlayerScore
    {
        public static bool IsDNF(this PlayerScoreRecord x)
        {
            return x.FirstName.ToLower().Contains("dnf") || x.LastName.ToLower().Contains("dnf");
        }
    }
}
