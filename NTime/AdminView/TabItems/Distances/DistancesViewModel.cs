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
using ViewCore.Entities;
using System.Linq;
using System.Windows;

namespace AdminView.Distances
{
    class DistancesViewModel : TabItemViewModel
    {
        ILogsInfo _logsInfo;
        public DistancesViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
            AddMeasurementPointCmd = new RelayCommand(OnAddGateAsync);
            AddDistanceCmd = new RelayCommand(OnAddDistance);
            LoadLogsFromCSVsToDB = new RelayCommand(OnLoadLogsFromCSVsToDB);
            TabTitle = "Dystanse";
            _logsInfo = new LogsInfo();
        }

        private void OnViewLoaded()
        {
            DownloadGatesInfoFromDatabaseAsync();
            DownloadDistancesFromDatabase();
        }

        private async void DownloadGatesInfoFromDatabaseAsync()
        {
            DefinedGates = new ObservableCollection<IEditableGate>();
            var contextProvider = new ContextProvider();
            var dbGates = await _gateRepository.GetAllAsync();
            foreach (var dbGate in dbGates)
            {
                var gateToAdd = new EditableGate(_logsInfo, _currentCompetition)
                {
                    DbEntity = dbGate,
                    AssignedLogs = new ObservableCollection<EditableTimeReadsLogInfo>()
                };
                var logRepository = new TimeReadsLogInfoRepository(contextProvider, dbGate);
                var logs = await logRepository.GetAllAsync();
                AddGateToGUI(gateToAdd);
                foreach (var log in logs)
                {
                    var logToAdd = new EditableTimeReadsLogInfo(_logsInfo, _currentCompetition)
                    {
                        DbEntity = log
                    };
                    gateToAdd.AddLogToGUI(logToAdd);
                }
            }
        }

        private async void DownloadDistancesFromDatabase()
        {
            Distances = new ObservableCollection<EditableDistance>();
            ContextProvider contextProvider = new ContextProvider();
            var dbDistances = await _distanceRepository.GetAllAsync();
            foreach (var dbDistance in dbDistances)
            {
                var gateOrderRepository = new GateOrderItemRepository(contextProvider, dbDistance);
                var dbGateOrderItems = await gateOrderRepository.GetAllAsync();
                var distanceToAdd = new EditableDistance(_logsInfo, _definedGates, _currentCompetition, gatesCount: dbGateOrderItems.Length)
                {
                    DbEntity = dbDistance,
                    GatesOrderItems = new ObservableCollection<EditableGatesOrderItem>(),
                };
                AddDistanceToGUI(distanceToAdd);
                foreach (var dbGateOrderItem in dbGateOrderItems)
                {
                    distanceToAdd.GatesOrderItems.Add(new EditableGatesOrderItem(_definedGates, _currentCompetition)
                    {
                        DbEntity = dbGateOrderItem
                    });
                }
                distanceToAdd.HideFirstMinTime();
            }
        }

        private async void OnLoadLogsFromCSVsToDB()
        {
            try
            {
                await _playerRepository.ImportTimeReadsFromSourcesAsync();
                MessageBox.Show("Wczytano pomiary z plików");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie udało się załadować pliku {ex.Message}");
            }
        }



        private async void OnAddGateAsync()
        {
            if (CanAddGate())
            {
                var gateNumber = int.Parse(NewGateNumber);
                var gateToAdd = new EditableGate(_logsInfo, _currentCompetition)
                {
                    DbEntity = new Gate()
                    {
                        Number = gateNumber,
                        Name = NewGateName,
                    },
                    AssignedLogs = new ObservableCollection<EditableTimeReadsLogInfo>()
                };
                AddGateToGUI(gateToAdd);
                await _gateRepository.AddAsync(gateToAdd.DbEntity);

            }
            else
            {
                System.Windows.MessageBox.Show("Punkt pomiarowy o takim numerze już istnieje. Wybierz inny numer");
            }

        }

        private void AddGateToGUI(EditableGate gateToAdd)
        {
            gateToAdd.DeleteRequested += Gate_DeleteRequestedAsync;
            DefinedGates.Add(gateToAdd);
            _logsInfo.GatesNumbers.Add(gateToAdd.Number);
            NewGateNumber = (gateToAdd.Number + 1).ToString();
            NewGateName = "";
        }

        private async void Gate_DeleteRequestedAsync(object sender, EventArgs e)
        {
            var gateToDelete = sender as EditableGate;
            foreach (var log in gateToDelete.AssignedLogs)
            {
                _logsInfo.LogsNumbers.Remove(log.LogNumber);
            }
            _logsInfo.GatesNumbers.Remove(gateToDelete.Number);
            DefinedGates.Remove(gateToDelete);
            await _gateRepository.RemoveAsync(gateToDelete.DbEntity);
        }

        private bool CanAddGate()
        {
            if (!int.TryParse(NewGateNumber, out int number))
                return false;
            if (string.IsNullOrWhiteSpace(NewGateName))
                return false;
            if (_logsInfo.GatesNumbers.Contains(number))
                return false;
            return true;
        }

        private async void OnAddDistance()
        {
            (bool canAdd, string errorMessage) = CanAddDistance();
            if (canAdd)
            {
                var distanceToAdd = new EditableDistance(_logsInfo, _definedGates, _currentCompetition, gatesCount: 0)
                {
                    DbEntity = new Distance()
                    {
                        Name = NewDistanceName,
                        //StartTime = DateTime.Today
                    }
                };
                AddDistanceToGUI(distanceToAdd);
                await _distanceRepository.AddAsync(distanceToAdd.DbEntity);
            }
            else
            {
                System.Windows.MessageBox.Show(errorMessage);
            }

        }

        private void AddDistanceToGUI(EditableDistance distanceToAdd)
        {
            //distanceToAdd.GatesCount = gateOrderItemsCount;
            _logsInfo.DistancesNames.Add(distanceToAdd.Name);
            distanceToAdd.DeleteRequested += Distance_DeleteRequested;
            Distances.Add(distanceToAdd);
        }

        private async void Distance_DeleteRequested(object sender, EventArgs e)
        {
            var distanceToDelete = sender as EditableDistance;
            _logsInfo.DistancesNames.Remove(distanceToDelete.Name);
            try
            {
                await _distanceRepository.RemoveAsync(distanceToDelete.DbEntity);
                Distances.Remove(distanceToDelete);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udało się usunąć dystansu." + Environment.NewLine +
                                $"Błąd: {ex.Message}" + Environment.NewLine +
                                $"Inner: {ex.InnerException.Message}");

            }
        }

        private (bool result, string message) CanAddDistance()
        {
            if (string.IsNullOrWhiteSpace(NewDistanceName))
                return (false, "Nazwa dystansu nie może być pusta");
            if (NewDistanceName != NewDistanceName.ToUpper())
                return (false, "Nazwa dystansu może zawierać tylko wielkie litery");
            if (_logsInfo.DistancesNames.Contains(NewDistanceName))
                return (false, "Taka nazwa dystansu już istnieje");
            return (true, "Brak błędów");
        }

        #region Properties

        private ObservableCollection<IEditableGate> _definedGates = new ObservableCollection<IEditableGate>();
        public ObservableCollection<IEditableGate> DefinedGates
        {
            get { return _definedGates; }
            set { SetProperty(ref _definedGates, value); }
        }


        private ObservableCollection<EditableDistance> _distances = new ObservableCollection<EditableDistance>();
        public ObservableCollection<EditableDistance> Distances
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
        public RelayCommand LoadLogsFromCSVsToDB { get; private set; }
    }
}
