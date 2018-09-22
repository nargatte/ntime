using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.Interfaces
{
    public interface IPlayerScore
    {
        string ScoreString { get;  }
        double NumberValue { get; }
        void AddValue(IPlayerScore score);
        void SubtractValue(IPlayerScore score);
        void ResetValue();
        bool IsDnf { get; set; }
    }
}
