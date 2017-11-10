using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;

namespace AdminView.Distances
{
    class DistancesViewModel : TabItemViewModel
    {
        public DistancesViewModel()
        {
            TabTitle = "Dystanse";
            AddMeasurementPointCmd = new RelayCommand(OnAddMeasurementPoint);
        }

        private void OnAddMeasurementPoint()
        {
            MeasurementPoints.Add(new MeasurementPoint
            {
                PointNumber = NewMeasurementPointNumber.Value,
                PointName = NewMeasurementPointName,
                AssignedLogs = new ObservableCollection<TimeReadsLog>()
            });
        }


        private ObservableCollection<MeasurementPoint> _measurementPoints = new ObservableCollection<MeasurementPoint>();
        public ObservableCollection<MeasurementPoint> MeasurementPoints
        {
            get { return _measurementPoints; }
            set { SetProperty(ref _measurementPoints, value); }
        }


        private int? _newMeasurementPointNumber;
        public int? NewMeasurementPointNumber
        {
            get { return _newMeasurementPointNumber; }
            set { SetProperty(ref _newMeasurementPointNumber, value); }
        }


        private string _newMeasurementPointName;
        public string NewMeasurementPointName
        {
            get { return _newMeasurementPointName; }
            set { SetProperty(ref _newMeasurementPointName, value); }
        }

        public event Action CompetitionManagerRequested = delegate { };

        public RelayCommand AddMeasurementPointCmd { get; private set; }
    }

    class MeasurementPoint : BindableBase
    {

        private int _pointNumber;
        public int PointNumber
        {
            get { return _pointNumber; }
            set { SetProperty(ref _pointNumber, value); }
        }

        private string _pointName;
        public string PointName
        {
            get { return _pointName; }
            set { SetProperty(ref _pointName, value); }
        }

        private ObservableCollection<TimeReadsLog> _assignedLogs = new ObservableCollection<TimeReadsLog>();
        public ObservableCollection<TimeReadsLog> AssignedLogs
        {
            get { return _assignedLogs; }
            set { SetProperty(ref _assignedLogs, value); }
        }


    }

    class TimeReadsLog : BindableBase
    {

        private int _logNumber;
        public int LogNumber
        {
            get { return _logNumber; }
            set { SetProperty(ref _logNumber, value); }
        }


        private string _directoryName;
        public string DirectoryName
        {
            get { return _directoryName; }
            set { SetProperty(ref _directoryName, value); }
        }
    }
}
