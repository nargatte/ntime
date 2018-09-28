using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Records
{
    public class DistanceRecord
    {
        public DistanceRecord()
        {

        }
        public string Name { get; set; }
        public int Length { get; set; }
        public int GatesCount { get; set; }
        public IEnumerable<int> GatesOrder { get; set; }
        public decimal StartTime { get; set; }
        public decimal Difference { get; set; }
    }
}
