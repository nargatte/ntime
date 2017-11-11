using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminView.Distances;

namespace AdminView
{
    interface ILogsInfo
    {
        HashSet<int> LogsNumbers { get; set; }
        HashSet<int> MeasurementPointsNumbers { get; set; }
        //ObservableCollection<MeasurementPoint> TimeReadsLogs { get; set; }
    }
}
