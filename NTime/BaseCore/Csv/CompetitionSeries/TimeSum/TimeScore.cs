using BaseCore.Csv.CompetitionSeries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.TimeSum
{
    public class TimeScore : IPlayerScore
    {
        string timeSpanFormat = @"hh\:mm\:ss\.fff";
        private TimeSpan _time;
        public string ScoreString => IsDnf ? "DNF" : _time.ToString(timeSpanFormat);
        public bool IsDnf { get; set; }
        public double NumberValue => IsDnf ? -1 : _time.Milliseconds;

        public TimeScore(TimeSpan time, bool isDnf)
        {
            _time = time;
            IsDnf = isDnf;
        }

        public void AddValue(IPlayerScore score)
        {
            if (score is TimeScore)
                _time += TimeSpan.Parse(score.ScoreString);
            else
                throw new InvalidOperationException();
        }

        public void ResetValue()
        {
            _time = TimeSpan.FromSeconds(0);
        }

        public void SubtractValue(IPlayerScore score)
        {
            if (score is TimeScore)
                _time -= TimeSpan.Parse(score.ScoreString);
            else
                throw new InvalidOperationException();
        }
    }
}
