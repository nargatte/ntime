using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BaseCore.TimesProcess;
using MvvmHelper;
using BaseCore.DataBase;

namespace ViewCore.Entities
{

    public class EditableDistance : EditableBaseClass<Distance>
    {
        GateOrderItemRepository _gateOrderItemRepository;
        ObservableCollection<IEditableGate> _definedGates;
        DistanceRepository _distanceRepository;
        public enum CompetitionTypeEnumerator
        {
            DeterminedDistanceLaps, DeterminedDistanceUnusual, LimitedTime
        }

        private ILogsInfo logsInfo;
        public EditableDistance(ILogsInfo logsInfo, ObservableCollection<IEditableGate> definedGates)
        {
            _definedGates = definedGates;
            this.logsInfo = logsInfo;
            SaveDistanceCmd = new RelayCommand(OnSaveDistanceAsync);
            DeleteDistanceCmd = new RelayCommand(OnDeleteDistance);
            ContextProvider contextProvider = new ContextProvider();
            _gateOrderItemRepository = new GateOrderItemRepository(contextProvider, this.DbEntity);
            _distanceRepository = new DistanceRepository(contextProvider, _currentCompetition.DbEntity);
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

        public BaseCore.DataBase.DistanceTypeEnum DistanceType
        {
            get { return DbEntity.DistanceTypeEnum; }
            set { DbEntity.DistanceTypeEnum = SetProperty(DbEntity.DistanceTypeEnum, value); }
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

        private async void UpdateGatesOrderCountAsync()
        {

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
                        var gateOrderItem = new EditableGatesOrderItem(_definedGates)
                        {
                            DbEntity = new GatesOrderItem(),
                        };
                        gateOrderItem.UpdateGatesOrderItemRequested += GateOrderItem_UpdateGatesOrderItemRequestedAsync;
                        GatesOrderItems.Add(gateOrderItem);
                        await _gateOrderItemRepository.AddAsync(gateOrderItem.DbEntity);
                    }
                }
                else if (diff < 0)
                {
                    for (int i = 0; i < Math.Abs(diff); i++)
                    {
                        var gateOrderItem = GatesOrderItems.Last();
                        GatesOrderItems.Remove(gateOrderItem);
                        await _gateOrderItemRepository.RemoveAsync(gateOrderItem.DbEntity);
                    }
                }
            }
            else
            {
                GatesCount = currentGatesCount;
            }
            if (GatesOrderItems.Count > 0)
                GatesOrderItems.First().IsTimeCollapsed = true;
        }

        private async void GateOrderItem_UpdateGatesOrderItemRequestedAsync(object sender, EventArgs e)
        {
            var gateOrderInfoItem = sender as EditableGatesOrderItem;
            var repository = new GateOrderItemRepository(new ContextProvider(), this.DbEntity);
            await repository.UpdateAsync(gateOrderInfoItem.DbEntity);
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

        public string StartTime
        {
            get { return DbEntity.StartTime.ConvertToString(); }
            set
            {
                if (value.TryConvertToDateTime(out DateTime dateTime))
                    DbEntity.StartTime = SetProperty(DbEntity.StartTime, dateTime);
            }
        }

        private bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
        }

        #endregion

        #region Commands and events
        private async void OnSaveDistanceAsync()
        {
            bool result = ValidateDistance();
            if (result)
            {
                await _distanceRepository.UpdateAsync(this.DbEntity);
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

        public RelayCommand DeleteDistanceCmd { get; }
        public RelayCommand SaveDistanceCmd { get; }


        public event EventHandler DeleteRequested = delegate { };

        #endregion

        /// <summary>
        /// Sets IsValid property and returns its value
        /// </summary>
        public bool ValidateDistance()
        {
            _isValid = IsDistanceValid(out string message);
            if (!_isValid)
                MessageBox.Show(message);
            return IsValid;
        }

        private bool IsDistanceValid(out string message)
        {
            message = "";
            if (Length <= 0)
            {
                message = "Długość dystansu nie może być zerowa ani ujemna";
                return false;
            }
            if (StartTime == null)
            {
                message = "Ustaw poprawnie czas startu";
                return false;
            }

            switch (DistanceType)
            {
                case BaseCore.DataBase.DistanceTypeEnum.DeterminedDistance:
                    if (LapsCount <= 0)
                    {
                        message = "Liczba okrążeń musi być większa od 0";
                        return false;
                    }

                    break;
                case BaseCore.DataBase.DistanceTypeEnum.DeterminedLaps:
                    break;
                case BaseCore.DataBase.DistanceTypeEnum.LimitedTime:
                    if (LapsCount <= 0)
                    {
                        message = "Liczba okrążeń musi być większa od zera";
                        return false;
                    }

                    if (TimeLimit == null)
                    {
                        message = "Ustaw poprawnie limit czasu";
                        return false;
                    }

                    break;
                default:
                    break;
            }


            return true;
        }

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
            List<int> gatesOrderList = new List<int>();
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
