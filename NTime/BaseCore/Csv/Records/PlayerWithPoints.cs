using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Records
{
    class PlayerWithPoints
    {
        public PlayerWithPoints(PlayerScoreRecord player, Dictionary<int, string> allCompetitions)
        {
            var firstNameSplit = player.FirstName.Split(' ');
            FirstName = firstNameSplit[0];
            var lastNameSplit = player.LastName.Split(' ');
            LastName = lastNameSplit[0];
            City = player.City;
            BirthDate = player.BirthDate;
            AgeCategory = player.AgeCategory;
            AllCompetitions = allCompetitions;
        }

        public PlayerWithPoints(PlayerScoreRecord player, Dictionary<int, string> allCompetitions, int points, int dnfsCount, int seriesCategoryPlace)
            : this(player, allCompetitions)
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
        public int CompetitionsStarted { get; set; }
        public Dictionary<int, double> CompetitionsPoints { get; set; } = new Dictionary<int, double>();
        public Dictionary<int, string> AllCompetitions { get; private set; }

        public override string ToString()
        {
            var str = $"{FirstName,-10} {LastName,-12} {BirthDate.Year,-5} {AgeCategory,-4} {CompetitionsStarted,-2} {Points,-2}   ";
            foreach (var competition in AllCompetitions)
            {
                bool competitionFound = CompetitionsPoints.TryGetValue(competition.Key, out double points);
                var extraString = string.Empty;
                if (competitionFound)
                {
                    if (points == -1)
                        extraString = "DNF";
                    else
                        extraString = points.ToString();
                }
                str += $"{extraString,-3} ";
            }
            return str;
        }
    }
}
