using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Distances
{
    class DistancesViewModel : TabItemViewModel
    {
        ILogsInfo logsInfo;
        public DistancesViewModel(Entities.EditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Dystanse";
            AddMeasurementPointCmd = new RelayCommand(OnAddMeasurementPoint);
            AddDistanceCmd = new RelayCommand(OnAddDistance);
            logsInfo = new LogsInfo();
        }

        private void OnAddMeasurementPoint()
        {
            if (CanAddMeasurementPoint())
            {
                var measurementPointNumber = int.Parse(NewMeasurementPointNumber);
                var measurementPointToAdd = new MeasurementPoint(logsInfo)
                {
                    PointNumber = measurementPointNumber,
                    PointName = NewMeasurementPointName,
                    AssignedLogs = new ObservableCollection<TimeReadsLog>()
                };
                measurementPointToAdd.DeleteRequested += MeasurementPointToAdd_DeleteRequested;
                MeasurementPoints.Add(measurementPointToAdd);
                logsInfo.MeasurementPointsNumbers.Add(measurementPointNumber);
                NewMeasurementPointNumber = (measurementPointNumber + 1).ToString();
                NewMeasurementPointName = "";
            }
            else
            {
                System.Windows.MessageBox.Show("Punkt pomiarowy o takim numerze już istnieje. Wybierz inny numer");
            }

        }

        private void MeasurementPointToAdd_DeleteRequested(object sender, EventArgs e)
        {
            var measurementPointToDelete = sender as MeasurementPoint;
            foreach (var log in measurementPointToDelete.AssignedLogs)
            {
                logsInfo.LogsNumbers.Remove(log.LogNumber);
            }
            logsInfo.MeasurementPointsNumbers.Remove(measurementPointToDelete.PointNumber);
            MeasurementPoints.Remove(measurementPointToDelete);
        }


        private bool CanAddMeasurementPoint()
        {
            if (!int.TryParse(NewMeasurementPointNumber, out int number))
                return false;
            if (string.IsNullOrWhiteSpace(NewMeasurementPointName))
                return false;
            if (logsInfo.MeasurementPointsNumbers.Contains(number))
                return false;
            return true;
        }

        private void OnAddDistance()
        {
            if (CanAddDistance(out string errorMessage))
            {
                var distance = new Entities.EditableDistance(logsInfo) { Name = NewDistanceName };
                logsInfo.DistancesNames.Add(distance.Name);
                distance.DeleteRequested += Distance_DeleteRequested;
                Distances.Add(distance);
            }
            else
            {
                System.Windows.MessageBox.Show(errorMessage);
            }
            
        }

        private void Distance_DeleteRequested(object sender, EventArgs e)
        {
            var distanceToDelete = sender as Entities.EditableDistance;
            logsInfo.DistancesNames.Remove(distanceToDelete.Name);
            Distances.Remove(distanceToDelete);
        }

        private bool CanAddDistance(out string errorMessage)
        {
            errorMessage = "";
            if(string.IsNullOrWhiteSpace(NewDistanceName))
            {
                errorMessage = "Nazwa dystansu nie może być pusta";
                return false;
            }
            if(NewDistanceName != NewDistanceName.ToUpper())
            {
                errorMessage = "Nazwa dystansu może zawierać tylko wielkie litery";
                return false;
            }
            if (logsInfo.DistancesNames.Contains(NewDistanceName))
            {
                errorMessage = "Taka nazwa dystansu już istnieje";
                return false;
            }
            return true;
        }



        #region Properties

        private ObservableCollection<MeasurementPoint> _measurementPoints = new ObservableCollection<MeasurementPoint>();
        public ObservableCollection<MeasurementPoint> MeasurementPoints
        {
            get { return _measurementPoints; }
            set { SetProperty(ref _measurementPoints, value); }
        }


        private ObservableCollection<Entities.EditableDistance> _distances = new ObservableCollection<Entities.EditableDistance>();
        public ObservableCollection<Entities.EditableDistance> Distances
        {
            get { return _distances; }
            set { SetProperty(ref _distances, value); }
        }

        private string _newMeasurementPointNumber;
        public string NewMeasurementPointNumber
        {
            get { return _newMeasurementPointNumber; }
            set
            {
                SetProperty(ref _newMeasurementPointNumber, value);
            }
        }


        private string _newDistanceName = "";
        public string NewDistanceName
        {
            get { return _newDistanceName; }
            set { SetProperty(ref _newDistanceName, value); }
        }

        private string _newMeasurementPointName;
        public string NewMeasurementPointName
        {
            get { return _newMeasurementPointName; }
            set { SetProperty(ref _newMeasurementPointName, value); }
        }


        private int _logsCount;
        public int LogsCount
        {
            get { return _logsCount; }
            set { SetProperty(ref _logsCount, value); }
        }

        #endregion

        public event Action CompetitionManagerRequested = delegate { };

        public RelayCommand AddMeasurementPointCmd { get; private set; }
        public RelayCommand AddDistanceCmd { get; private set; }
    }

    class MeasurementPoint : BindableBase
    {
        ILogsInfo logsInfo;
        public MeasurementPoint(ILogsInfo logsInfo)
        {
            AddLogCmd = new RelayCommand(OnAddLog);
            DeleteMeasurementPointCmd = new RelayCommand(OnDeleteMeasurementPoint);
            this.logsInfo = logsInfo;
        }

        private void OnDeleteMeasurementPoint()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        private void OnAddLog()
        {
            if (CanAddLog())
            {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                if (dialog.ShowDialog().Value)
                {
                    NewLogDirectoryName = dialog.FileName;
                    var logToAdd = new TimeReadsLog(logsInfo)
                    {
                        LogNumber = NewLogNumber,
                        DirectoryName = NewLogDirectoryName
                    };
                    logToAdd.DeleteRequested += LogToAdd_DeleteRequested1;
                    AssignedLogs.Add(logToAdd);
                    logsInfo.LogsNumbers.Add(NewLogNumber);
                    NewLogNumber++;
                    NewLogDirectoryName = "";
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Log o takim numerze już istnieje. Wybierz inny numer");
            }
        }

        private void LogToAdd_DeleteRequested1(object sender, EventArgs e)
        {
            var logToDelete = sender as TimeReadsLog;
            logsInfo.LogsNumbers.Remove(logToDelete.LogNumber);
            AssignedLogs.Remove(logToDelete);
        }

        private bool CanAddLog()
        {
            if (NewLogNumber <= 0)
                return false;
            if (logsInfo.LogsNumbers.Contains(NewLogNumber))
                return false;
            return true;
        }


        private int _newLogNumber;
        public int NewLogNumber
        {
            get { return _newLogNumber; }
            set
            {
                SetProperty(ref _newLogNumber, value);
                //AddLogCmd.RaiseCanExecuteChanged();
            }
        }


        private string _newLogDirectoryName;
        public string NewLogDirectoryName
        {
            get { return _newLogDirectoryName; }
            set
            {
                SetProperty(ref _newLogDirectoryName, value);
            }
        }

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

        public RelayCommand AddLogCmd { get; }
        public RelayCommand DeleteMeasurementPointCmd { get; }
        public event EventHandler DeleteRequested = delegate { };

    }

    class TimeReadsLog : BindableBase
    {
        ILogsInfo logsInfo;
        public TimeReadsLog(ILogsInfo logsInfo)
        {
            this.logsInfo = logsInfo;
            DeleteLogCmd = new RelayCommand(OnDeleteLog);
        }

        private void OnDeleteLog()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

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

        public RelayCommand DeleteLogCmd { get; }

        public event EventHandler DeleteRequested = delegate { };
    }
}
