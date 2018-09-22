using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Records
{
    public class PlayerWithScores
    {

        public int CategoryStandingPlace { get; set; }
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
        public List<string> CompetitionsPointsExport { get; set; } = new List<string>();

        public PlayerWithScores(PlayerScoreRecord player, Dictionary<int, string> allCompetitions)
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

        public PlayerWithScores(PlayerScoreRecord player, Dictionary<int, string> allCompetitions, int points, int dnfsCount, int seriesCategoryPlace)
            : this(player, allCompetitions)
        {
            Points = points;
            DNFsCount = dnfsCount;
            SeriesCategoryPlace = seriesCategoryPlace;
        }


        public override string ToString()
        {
            SetPointsForCompetitions();
            var str = $"{LastName,-12} {FirstName,-12} {BirthDate.Year,-5} {AgeCategory,-4} {CompetitionsStarted,-2} {Points,-2}   ";
            CompetitionsPointsExport.ForEach(points => str += $"{points,-4} ");
            return str;

            //foreach (var competition in AllCompetitions)
            //{
            //    string extraString = GetPointsForCompetition(competition);
            //    str += $"{extraString,-3} ";
            //}
        }

        public void SetPointsForCompetitions()
        {
            CompetitionsPointsExport.Clear();
            foreach (var competition in AllCompetitions)
            {
                string extraString = GetPointsForCompetition(competition);
                CompetitionsPointsExport.Add(extraString);
            }
        }

        private string GetPointsForCompetition(KeyValuePair<int, string> competition)
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

            return extraString;
        }
    }
}
