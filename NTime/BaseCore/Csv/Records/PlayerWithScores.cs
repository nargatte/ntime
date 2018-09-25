using BaseCore.Csv.CompetitionSeries.Interfaces;
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
        public IPlayerScore TotalScore { get; set; }
        public int DNFsCount { get; set; }
        public int SeriesCategoryPlace { get; set; }
        public int ExactCompetitionsStarted { get; set; }
        public int ApproximateCompetitionsStarted { get; set; }
        public int CompetitionsCompleted => CompetitionsScores.Values.Count(score => score.CompetitionCompleted);
        public Dictionary<int, IPlayerScore> CompetitionsScores { get; set; } = new Dictionary<int, IPlayerScore>();
        public Dictionary<int, string> AllCompetitions { get; private set; }
        public List<string> CompetitionsScoreExport { get; set; } = new List<string>();

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

        public PlayerWithScores(PlayerScoreRecord player, Dictionary<int, string> allCompetitions, IPlayerScore points, int dnfsCount, int seriesCategoryPlace)
            : this(player, allCompetitions)
        {
            TotalScore = points;
            DNFsCount = dnfsCount;
            SeriesCategoryPlace = seriesCategoryPlace;
        }


        public override string ToString()
        {
            SetPointsForCompetitions();
            var str = $"{LastName,-12} {FirstName,-12} {BirthDate.Year,-5} {AgeCategory,-4} {ExactCompetitionsStarted,-2} {TotalScore,-2}   ";
            CompetitionsScoreExport.ForEach(points => str += $"{points,-4} ");
            return str;

            //foreach (var competition in AllCompetitions)
            //{
            //    string extraString = GetPointsForCompetition(competition);
            //    str += $"{extraString,-3} ";
            //}
        }

        public void SetPointsForCompetitions()
        {
            CompetitionsScoreExport.Clear();
            foreach (var competition in AllCompetitions)
            {
                string extraString = GetPointsForCompetition(competition.Key);
                CompetitionsScoreExport.Add(extraString);
            }
        }

        private string GetPointsForCompetition(int competitionKey)
        {
            bool competitionFound = CompetitionsScores.TryGetValue(competitionKey, out IPlayerScore score);
            var extraString = string.Empty;
            if (competitionFound)
            {
                if (score.IsDnf)
                    extraString = "DNF";
                else
                    extraString = score.ScoreString;
            }

            return extraString;
        }
    }
}
