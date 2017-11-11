using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView
{
    class LogsCounts : ILogsInfo
    {
        public HashSet<int> LogsNumbers { get; set; } = new HashSet<int>();
        public HashSet<int> MeasurementPointsNumbers { get; set; } = new HashSet<int>();
    }
}
