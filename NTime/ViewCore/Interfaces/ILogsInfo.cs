﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public interface ILogsInfo
    {
        HashSet<int> LogsNumbers { get; set; }
        HashSet<int> GatesNumbers { get; set; }
        HashSet<string> DistancesNames { get; set; }
        //ObservableCollection<MeasurementPoint> TimeReadsLogs { get; set; }
    }
}
