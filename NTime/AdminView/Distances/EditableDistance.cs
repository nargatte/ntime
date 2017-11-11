using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdminView.Distances
{
    public enum CompetitionTypeEnumerator
    {
        DeterminedDistanceLaps, DeterminedDistanceUnusual, LimitedTime
    }

    class EditableDistance : BindableBase
    {
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

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


        private decimal _lenght;
        public decimal Length
        {
            get { return _lenght; }
            set { SetProperty(ref _lenght, value); }
        }

        private CompetitionTypeEnumerator? competitionType;
        public CompetitionTypeEnumerator? CompetitionType
        {
            get { return competitionType; }
            set { SetProperty(ref competitionType, value); }
        }


        private string _readersOrder;
        public string ReadersOrder
        {
            get { return _readersOrder; }
            set { SetProperty(ref _readersOrder, value); }
        }


        private int? _lapsCount;
        public int? LapsCount
        {
            get { return _lapsCount; }
            set { SetProperty(ref _lapsCount, value); }
        }


        private DateTime _timeLimit;
        public DateTime TimeLimit
        {
            get { return _timeLimit; }
            set { SetProperty(ref _timeLimit, value); }
        }


        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
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
            if (!CompetitionType.HasValue)
                return false;
            if (Length <= 0)
                return false;
            if (StartTime == null)
                return false;
            switch (CompetitionType)
            {
                case CompetitionTypeEnumerator.DeterminedDistanceLaps:
                    if (LapsCount <= 0)
                        return false;
                    break;
                case CompetitionTypeEnumerator.DeterminedDistanceUnusual:
                    break;
                case CompetitionTypeEnumerator.LimitedTime:
                    if (LapsCount <= 0)
                        return false;
                    if (TimeLimit == null)
                        return false;
                    break;
                default:
                    break;
            }
            if (ReadersOrder == null)
                return false;
            if (ReadersOrder.Length == 0)
                return false;


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
