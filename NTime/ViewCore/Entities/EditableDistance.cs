using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BaseCore.TimesProcess;
using MvvmHelper;
using BaseCore.DataBase;

namespace ViewCore.Entities
{

    public class EditableDistance : EditableBaseClass<Distance>
    {
        GateOrderItemRepository _gateOrderItemRepository;
        ObservableCollection<IEditableGate> _definedGates = new ObservableCollection<IEditableGate>();
        DistanceRepository _distanceRepository;
        public enum CompetitionTypeEnumerator
        {
            DeterminedDistanceLaps, DeterminedDistanceUnusual, LimitedTime
        }

        public EditableDistance(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            SaveDistanceCmd = new RelayCommand(OnSaveDistanceAsync);
            DeleteDistanceCmd = new RelayCommand(OnDeleteDistance);
            ContextProvider contextProvider = new ContextProvider();
            _distanceRepository = new DistanceRepository(contextProvider, _currentCompetition.DbEntity);
        }

        private ILogsInfo logsInfo;
        public EditableDistance(ILogsInfo logsInfo, ObservableCollection<IEditableGate> definedGates, IEditableCompetition currentCompetition,
                                int gatesCount) : this(currentCompetition)
        {
            _definedGates = definedGates;
            this.logsInfo = logsInfo;
            _gatesCount = gatesCount;
        }
        #region Properties


        public string Name
        {
            get { return DbEntity.Name; }
            set { DbEntity.Name = SetProperty(DbEntity.Name, value); }
        }

        public decimal Length
        {
            get { return DbEntity.Length; }
            set { DbEntity.Length = SetProperty(DbEntity.Length, value); }
        }


        public DistanceTypeEnum DistanceType
        {
            get { return DbEntity.DistanceTypeEnum; }
            set
            {
                DbEntity.DistanceTypeEnum = SetProperty(DbEntity.DistanceTypeEnum, value);
                ResolveLapsCountCollapsed();
            }
        }


        private int _gatesCount;
        public int GatesCount
        {
            get { return _gatesCount; }
            set
            {
                SetProperty(ref _gatesCount, value);
                UpdateGatesOrderCountAsync();
            }
        }


        private bool _isLapsCountCollapsed;
        public bool IsLapsCountCollapsed
        {
            get { return _isLapsCountCollapsed; }
            set { SetProperty(ref _isLapsCountCollapsed, value); }
        }


        private ObservableCollection<EditableGatesOrderItem> _gatesOrderItems = new ObservableCollection<EditableGatesOrderItem>();
        public ObservableCollection<EditableGatesOrderItem> GatesOrderItems
        {
            get { return _gatesOrderItems; }
            set { SetProperty(ref _gatesOrderItems, value); }
        }

        public int LapsCount
        {
            get { return DbEntity.LapsCount; }
            set { DbEntity.LapsCount = SetProperty(DbEntity.LapsCount, value); }
        }



        public string TimeLimit
        {
            get { return DbEntity.TimeLimit.ToDateTime().ConvertToString(); }
            set
            {
                if (value.TryConvertToDateTime(out DateTime dateTime))
                    DbEntity.TimeLimit = SetProperty(DbEntity.TimeLimit, dateTime.ToDecimal());
            }
        }

        private bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
        }

        #endregion

        #region Methods and events

        public RelayCommand DeleteDistanceCmd { get; }
        public RelayCommand SaveDistanceCmd { get; }

        public event EventHandler DeleteRequested = delegate { };

        private async void OnSaveDistanceAsync()
        {
            bool result = ValidateDistance();
            if (result)
            {
                await _distanceRepository.UpdateAsync(this.DbEntity);
                _gateOrderItemRepository = new GateOrderItemRepository(new ContextProvider(), this.DbEntity);
                List<Task> tasks = new List<Task>();
                var item = GatesOrderItems
                    .Select(goi => new Tuple<GatesOrderItem, Gate>(goi.DbEntity, goi.Gate?.DbEntity)).ToArray();
                await _gateOrderItemRepository.ReplaceByAsync(item);
                //foreach (var gateOrderItem in GatesOrderItems)
                //{
                //    tasks.Add(_gateOrderItemRepository.ReplaceByAsync(Gat));
                //}
                //await Task.WhenAll(tasks);
                MessageBox.Show("Dystans został poprawnie zapisany");
            }
        }

        private void OnDeleteDistance()
        {
            MessageBoxResult result = MessageBox.Show("",
                $"Czy na pewno chcesz usunąć dystans {Name} ?",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                DeleteDistance();
            else return;
        }

        private void DeleteDistance()
        {
            DeleteRequested(this, EventArgs.Empty);
        }


        private void ResolveLapsCountCollapsed()
        {
            switch (DistanceType)
            {
                case DistanceTypeEnum.DeterminedDistance:
                    IsLapsCountCollapsed = true;
                    break;
                case DistanceTypeEnum.DeterminedLaps:
                    IsLapsCountCollapsed = false;
                    break;
                case DistanceTypeEnum.LimitedTime:
                    IsLapsCountCollapsed = false;
                    break;
                default:
                    break;
            }
        }

        private void UpdateGatesOrderCountAsync()
        {
            _gateOrderItemRepository = new GateOrderItemRepository(new ContextProvider(), this.DbEntity);
            int currentGatesCount = GatesOrderItems.Count;
            int updatedGatesCount = GatesCount;
            int diff = updatedGatesCount - currentGatesCount;
            if (diff == 0)
                return;
            MessageBoxResult result = MessageBox.Show(
                $"Czy na pewno chcesz zmienić liczbę bramek pomiarowych z {currentGatesCount} na {updatedGatesCount}?",
                "",
                 MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (diff > 0)
                {
                    for (int i = 0; i < diff; i++)
                    {
                        var gateOrderItem = new EditableGatesOrderItem(_definedGates, _currentCompetition)
                        {
                            DbEntity = new GatesOrderItem(),
                        };
                        gateOrderItem.UpdateGatesOrderItemRequested += GateOrderItem_UpdateGatesOrderItemRequestedAsync;
                        GatesOrderItems.Add(gateOrderItem);
                    }
                }
                else if (diff < 0)
                {
                    for (int i = 0; i < Math.Abs(diff); i++)
                    {
                        var gateOrderItem = GatesOrderItems.Last();
                        GatesOrderItems.Remove(gateOrderItem);
                    }
                }
            }
            else
            {
                GatesCount = currentGatesCount;
            }
            HideFirstMinTime();
        }

        public void HideFirstMinTime()
        {
            if (GatesOrderItems.Count > 0)
                GatesOrderItems.First().IsTimeCollapsed = true;
        }

        private async void GateOrderItem_UpdateGatesOrderItemRequestedAsync(object sender, EventArgs e)
        {
            var gateOrderInfoItem = sender as EditableGatesOrderItem;
            var repository = new GateOrderItemRepository(new ContextProvider(), this.DbEntity);
            await repository.UpdateAsync(gateOrderInfoItem.DbEntity, gateOrderInfoItem.DbEntity.Gate);
        }
        public bool ValidateDistance()
        {
            var message = string.Empty;
            (_isValid,  message) = IsDistanceValid();
            if (!_isValid)
                MessageBox.Show(message);
            return IsValid;
        }

        private (bool result, string message) IsDistanceValid()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return (false, "Nazwa dystansu nie może być pusta");

            //if (Length <= 0)
            //    return (false, "Długość dystansu nie może być zerowa ani ujemna");

            //if (GatesOrderItems == null || GatesOrderItems.Count == 0)
            //    return (false, "Liczba bramek pomiarowych musi być większa od 0");

            switch (DistanceType)
            {
                case DistanceTypeEnum.DeterminedDistance:
                    if (LapsCount != 0)
                        return (false, "Liczba okrążeń musi być równa 0");
                    break;
                case DistanceTypeEnum.DeterminedLaps:
                    if (LapsCount <= 0)
                        return (false, "Liczba okrążeń musi być większa od zera");
                    if (!IsLapCorrect())
                        return (false, "Dla wyścigu z okrążeniami pierwsza i ostatnia bramka muszą być takie same");
                    break;
                case DistanceTypeEnum.LimitedTime:
                    if (LapsCount <= 0)
                        return (false, "Liczba okrążeń musi być większa od zera");
                    if (!IsLapCorrect())
                        return (false, "Dla wyścigu z okrążeniami pierwsza i ostatnia bramka muszą być takie same");
                    if (TimeLimit == null)
                        return (false, "Ustaw poprawnie limit czasu");
                    break;
                default:
                    break;
            }
            return (true, "Brak błędów");
        }

        private bool IsLapCorrect()
        {
            if (GatesOrderItems == null || GatesOrderItems.Count == 0)
                return false;
            return GatesOrderItems.FirstOrDefault().Gate?.Number == GatesOrderItems.LastOrDefault().Gate?.Number;
        }
        #endregion

    }

    public static class GateConverter
    {
        /// <summary>
        /// Converts a string with gates' numbers seperated with comas, dots or semi-colons to an array of int
        /// </summary>
        /// <param name="gatesOrderInput"></param>
        /// <param name="gatesOrderOutput"></param>
        /// <returns> Returns true if conversion was successful, if there were any expection returns false </returns>
        public static bool ConvertToGatesOrder(this string gatesOrderInput, out int[] gatesOrderOutput)
        {
            var gatesOrderList = new List<int>();
            try
            {
                string[] measurementPoints = gatesOrderInput.Split(new char[] { ',', '.', ';' });
                foreach (var gate in measurementPoints)
                {
                    gatesOrderList.Add(int.Parse(gate));
                }
            }
            catch (FormatException)
            {
                gatesOrderOutput = new int[0];
                return false;
            }
            gatesOrderOutput = gatesOrderList.ToArray();
            return true;
        }
    }


}
