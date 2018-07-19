using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Records
{
    class PlayerWithPoints
    {
        public PlayerWithPoints(PlayerScoreRecord player)
        {
            FirstName = player.FirstName;
            LastName = player.LastName;
            City = player.City;
            BirthDate = player.BirthDate;
            AgeCategory = player.AgeCategory;
        }

        public PlayerWithPoints(PlayerScoreRecord player, int points, int dnfsCount, int seriesCategoryPlace) : this(player)
        {
            Points = points;
            DNFsCount = dnfsCount;
            SeriesCategoryPlace = seriesCategoryPlace;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
        public string AgeCategory { get; set; }
        public double Points { get; set; }
        public int DNFsCount { get; set; }
        public int SeriesCategoryPlace { get; set; }
        public int CompetitionsCompleted { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} {BirthDate.Year} {AgeCategory} {CompetitionsCompleted} {Points}";
        }
    }
}
