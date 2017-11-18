using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;

namespace AdminView.Distances
{
    class DistancesViewModel : TabItemViewModel
    {
        ViewCore.Entities.ILogsInfo logsInfo;
        public DistancesViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
            AddMeasurementPointCmd = new RelayCommand(OnAddMeasurementPoint);
            AddDistanceCmd = new RelayCommand(OnAddDistance);
            TabTitle = "Dystanse";
            logsInfo = new ViewCore.Entities.LogsInfo();
        }

        private void OnViewLoaded()
        {
            DownloadDistancesFromDatabase();
        }

        private async void DownloadDistancesFromDatabase()
        {
            var repository = new DistanceRepository(new ContextProvider(), _currentCompetition.DbEntity);
            var dbDistances = await repository.GetAllAsync();
            foreach (var dbDistance in dbDistances)
            {
                Distances.Add(new ViewCore.Entities.EditableDistance(logsInfo, _definedGates) { DbEntity = dbDistance });
            }
        }

        private void OnAddMeasurementPoint()
        {
            if (CanAddMeasurementPoint())
            {
                var GateNumber = int.Parse(NewGateNumber);
                var GateToAdd = new ViewCore.Entities.EditableGate(logsInfo)
                {
                    DbEntity = new Gate()
                    {
                        Number = GateNumber,
                        Name = NewGateName,
                        //AssignedLogs = new ObservableCollection<ViewCore.Entities.EditableTimeReadsLogInfo>();
                    }
                };
                GateToAdd.DeleteRequested += Gate_DeleteRequested;
                DefinedGates.Add(GateToAdd);
                logsInfo.GatesNumbers.Add(GateNumber);
                NewGateNumber = (GateNumber + 1).ToString();
                NewGateName = "";
            }
            else
            {
                System.Windows.MessageBox.Show("Punkt pomiarowy o takim numerze już istnieje. Wybierz inny numer");
            }

        }

        private void Gate_DeleteRequested(object sender, EventArgs e)
        {
            var GateToDelete = sender as ViewCore.Entities.EditableGate;
            foreach (var log in GateToDelete.AssignedLogs)
            {
                logsInfo.LogsNumbers.Remove(log.LogNumber);
            }
            logsInfo.GatesNumbers.Remove(GateToDelete.Number);
            DefinedGates.Remove(GateToDelete);
        }


        private bool CanAddMeasurementPoint()
        {
            if (!int.TryParse(NewGateNumber, out int number))
                return false;
            if (string.IsNullOrWhiteSpace(NewGateName))
                return false;
            if (logsInfo.GatesNumbers.Contains(number))
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

        private string _newGateNumber;
        public string NewGateNumber
        {
            get { return _newGateNumber; }
            set
            {
                SetProperty(ref _newGateNumber, value);
            }
        }


        private string _newGateName;
        public string NewGateName
        {
            get { return _newGateName; }
            set { SetProperty(ref _newGateName, value); }
        }

        private string _newDistanceName = "";
        public string NewDistanceName
        {
            get { return _newDistanceName; }
            set { SetProperty(ref _newDistanceName, value); }
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
        public RelayCommand ViewLoadedCmd { get; private set; }
    }
}
