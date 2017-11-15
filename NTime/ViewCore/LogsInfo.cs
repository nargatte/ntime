using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView
{
    public class LogsInfo : ILogsInfo
    {
        public HashSet<int> LogsNumbers { get; set; } = new HashSet<int>();
        public HashSet<int> MeasurementPointsNumbers { get; set; } = new HashSet<int>();
        public HashSet<string> DistancesNames { get; set; } = new HashSet<string>();
    }
}
