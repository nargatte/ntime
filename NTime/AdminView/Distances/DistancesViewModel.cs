using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore;

namespace AdminView.Distances
{
    class DistancesViewModel : TabItemViewModel
    {
        ViewCore.Entities.ILogsInfo logsInfo;
        public DistancesViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Dystanse";
            AddMeasurementPointCmd = new RelayCommand(OnAddMeasurementPoint);
            AddDistanceCmd = new RelayCommand(OnAddDistance);
            logsInfo = new ViewCore.Entities.LogsInfo();
        }

        private void OnAddMeasurementPoint()
        {
            if (CanAddMeasurementPoint())
            {
                var measurementPointNumber = int.Parse(NewMeasurementPointNumber);
                var measurementPointToAdd = new ViewCore.Entities.EditableGate(logsInfo)
                {
                    Number = measurementPointNumber,
                    Name = NewMeasurementPointName,
                    AssignedLogs = new ObservableCollection<ViewCore.Entities.EditableTimeReadsLog>()
                };
                measurementPointToAdd.DeleteRequested += MeasurementPointToAdd_DeleteRequested;
                DefinedGates.Add(measurementPointToAdd);
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
            var measurementPointToDelete = sender as ViewCore.Entities.EditableGate;
            foreach (var log in measurementPointToDelete.AssignedLogs)
            {
                logsInfo.LogsNumbers.Remove(log.LogNumber);
            }
            logsInfo.MeasurementPointsNumbers.Remove(measurementPointToDelete.Number);
            DefinedGates.Remove(measurementPointToDelete);
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
                var distance = new ViewCore.Entities.EditableDistance(logsInfo, _definedGates) { Name = NewDistanceName };
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
            var distanceToDelete = sender as ViewCore.Entities.EditableDistance;
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

        private ObservableCollection<ViewCore.Entities.IEditableGate> _definedGates = new ObservableCollection<ViewCore.Entities.IEditableGate>();
        public ObservableCollection<ViewCore.Entities.IEditableGate> DefinedGates
        {
            get { return _definedGates; }
            set { SetProperty(ref _definedGates, value); }
        }


        private ObservableCollection<ViewCore.Entities.EditableDistance> _distances = new ObservableCollection<ViewCore.Entities.EditableDistance>();
        public ObservableCollection<ViewCore.Entities.EditableDistance> Distances
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
}
