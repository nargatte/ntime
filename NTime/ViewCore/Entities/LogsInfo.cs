using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public class LogsInfo : ViewCore.Entities.ILogsInfo
    {
        public HashSet<int> LogsNumbers { get; set; } = new HashSet<int>();
        public HashSet<int> GatesNumbers { get; set; } = new HashSet<int>();
        public HashSet<string> DistancesNames { get; set; } = new HashSet<string>();
    }
}
