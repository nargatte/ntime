using BaseCore.Csv.CompetitionSeries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.PlacesAndPoints
{
    public class PointsScore : IPlayerScore
    {
        private double _points;
        public string ScoreString => IsDnf ? "DNF" : _points.ToString();
        public bool IsDnf { get; set; }

        public double NumberValue => IsDnf ? -1 : _points;

        public bool StartedInCompetition { get; set; }
        public bool CompetitionCompleted => StartedInCompetition && !IsDnf;

        public PointsScore(double points, bool isDnf, bool startedInCompetition)
        {
            _points = points;
            IsDnf = isDnf;
            StartedInCompetition = startedInCompetition;
        }

        public void AddValue(IPlayerScore score)
        {
            if (score is PointsScore)
            {
                if (!score.IsDnf)
                    _points += score.NumberValue;
            }
            else
                throw new InvalidOperationException();
        }

        public void ResetValue()
        {
            _points = 0;
        }

        public void SubtractValue(IPlayerScore score)
        {
            if (score is PointsScore)
            {
                if (!score.IsDnf)
                    _points -= score.NumberValue;
            }
            else
                throw new InvalidOperationException();
        }
    }
}
