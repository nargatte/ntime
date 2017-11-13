using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BaseCore.TimesProcess;

namespace AdminView.Entities
{
    public enum CompetitionTypeEnumerator
    {
        DeterminedDistanceLaps, DeterminedDistanceUnusual, LimitedTime
    }

    class EditableDistance : BindableBase
    {
        private BaseCore.DataBase.Distance _dstance = new BaseCore.DataBase.Distance();

        public BaseCore.DataBase.Distance Distance
        {
            get { return _dstance; }
            set { _dstance = value; }
        }


        private ILogsInfo logsInfo;
        public EditableDistance(ILogsInfo logsInfo)
        {
            this.logsInfo = logsInfo;
            SaveDistanceCmd = new RelayCommand(OnSaveDistance);
            DeleteDistanceCmd = new RelayCommand(OnDeleteDistance);
        }

        private void OnSaveDistance()
        {
            bool result = ValidateDistance();
            if (result)
                MessageBox.Show("Dystans został poprawnie zapisany");
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

        #region Properties


        public string Name
        {
            get { return Distance.Name; }
            set { Distance.Name = SetProperty(Distance.Name, value); }
        }


        public decimal Length
        {
            get { return Distance.Length; }
            set { Distance.Length = SetProperty(Distance.Length, value); }
        }


        public BaseCore.DataBase.DistanceTypeEnum DistanceType
        {
            get { return Distance.DistanceTypeEnum; }
            set { Distance.DistanceTypeEnum = SetProperty(Distance.DistanceTypeEnum, value); }
        }


        public ICollection<BaseCore.DataBase.ReaderOrder> ReadersOrder
        {
            get { return Distance.ReaderOrders; }
            set { Distance.ReaderOrders = SetProperty(Distance.ReaderOrders, value); }
        }


        public int LapsCount
        {
            get { return Distance.LapsCount; }
            set { Distance.LapsCount = SetProperty(Distance.LapsCount, value); }
        }



        public string TimeLimit
        {
            get { return Distance.TimeLimit.ToDateTime().ConvertToString(); }
            set {
                if (value.TryConvertToDateTime(out DateTime dateTime))
                    Distance.TimeLimit = SetProperty(Distance.TimeLimit, dateTime.ToDecimal());}
        }

        public string StartTime
        {
            get { return Distance.StartTime.ConvertToString(); }
            set
            {
                if (value.TryConvertToDateTime(out DateTime dateTime))
                    Distance.StartTime = SetProperty(Distance.StartTime, dateTime);
            }
        }

        private bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
        }

        #endregion
        /// <summary>
        /// Sets IsValid property and returns its value
        /// </summary>
        public bool ValidateDistance()
        {
            _isValid = IsDistanceValid();
            return IsValid;
        }

        private bool IsDistanceValid()
        {
            if (Length <= 0)
                return false;
            if (StartTime == null)
                return false;
            switch (DistanceType)
            {
                case BaseCore.DataBase.DistanceTypeEnum.DeterminedDistance:
                    if (LapsCount <= 0)
                        return false;
                    break;
                case BaseCore.DataBase.DistanceTypeEnum.DeterminedLaps:
                    break;
                case BaseCore.DataBase.DistanceTypeEnum.LimitedTime:
                    if (LapsCount <= 0)
                        return false;
                    if (TimeLimit == null)
                        return false;
                    break;
                default:
                    break;
            }
            //if (ReadersOrder == null)
            //    return false;
            //if (ReadersOrder.Length == 0)
            //    return false;


            return true;
        }

        public RelayCommand DeleteDistanceCmd { get; }
        public RelayCommand SaveDistanceCmd { get; }

        public event EventHandler DeleteRequested = delegate { };
    }

    public static class ReaderConverter
    {
        /// <summary>
        /// Converts a string with reader numbers seperated with comas, dots or semi-colons to an array of int
        /// </summary>
        /// <param name="readersOrderInput"></param>
        /// <param name="readersOrderOutput"></param>
        /// <returns> Returns true if conversion was successful, if there were any expection returns false </returns>
        public static bool ConvertToReadersOrder(this string readersOrderInput, out int[] readersOrderOutput)
        {
            List<int> readersOrderList = new List<int>();
            try
            {
                string[] measurementPoints = readersOrderInput.Split(new char[] { ',', '.', ';' });
                foreach (var reader in measurementPoints)
                {
                    readersOrderList.Add(int.Parse(reader));
                }
            }
            catch (FormatException)
            {
                readersOrderOutput = new int[0];
                return false;
            }
            readersOrderOutput = readersOrderList.ToArray();
            return true;
        }
    }


}
