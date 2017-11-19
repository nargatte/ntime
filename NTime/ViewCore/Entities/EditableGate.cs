using System;
using System.Collections.ObjectModel;
using System.Linq;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;

namespace ViewCore.Entities
{
    public class EditableGate : EditableBaseClass<Gate>, IEditableGate
    {
        ILogsInfo logsInfo;
        public EditableGate(ILogsInfo logsInfo) : base()
        {
            AddLogCmd = new RelayCommand(OnAddLog);
            DeleteMeasurementPointCmd = new RelayCommand(OnDeleteGate);
            this.logsInfo = logsInfo;
            UpdateNewLogNumber();
        }

        #region Methods
        private void OnDeleteGate()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        private async void OnAddLog()
        {
            if (CanAddLog())
            {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = "CSV files (*.csv)|*.csv";
                if (dialog.ShowDialog().Value)
                {
                    NewLogDirectoryName = dialog.FileName;
                    var logToAdd = new EditableTimeReadsLogInfo(logsInfo)
                    {
                        DbEntity = new TimeReadsLogInfo()
                        {
                            LogNumber = NewLogNumber,
                            Path = NewLogDirectoryName
                        }
                    };
                    var repository = new TimeReadsLogInfoRepository(new ContextProvider(), this.DbEntity);
                    await repository.AddAsync(logToAdd.DbEntity);
                    AddLogToGUI(logToAdd);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Log o takim numerze już istnieje. Wybierz inny numer");
            }
        }

        public void AddLogToGUI(EditableTimeReadsLogInfo logToAdd)
        {
            logToAdd.DeleteRequested += LogToAdd_DeleteRequested1;
            AssignedLogs.Add(logToAdd);
            logsInfo.LogsNumbers.Add(logToAdd.LogNumber);
            UpdateNewLogNumber();
            NewLogDirectoryName = "";
        }

        private void UpdateNewLogNumber()
        {
            if (logsInfo.LogsNumbers.Count > 0)
                NewLogNumber = logsInfo.LogsNumbers.Max() + 1;
            else
                NewLogNumber = 0;
        }

        private void LogToAdd_DeleteRequested1(object sender, EventArgs e)
        {
            var logToDelete = sender as EditableTimeReadsLogInfo;
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
        #endregion

        #region UI input properties
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
        #endregion



        public int Number
        {
            get { return DbEntity.Number; }
            set { DbEntity.Number = SetProperty(DbEntity.Number, value); }
        }


        public string Name
        {
            get { return DbEntity.Name; }
            set { DbEntity.Name = SetProperty(DbEntity.Name, value); }
        }

        private ObservableCollection<EditableTimeReadsLogInfo> _assignedLogs = new ObservableCollection<EditableTimeReadsLogInfo>();
        public ObservableCollection<EditableTimeReadsLogInfo> AssignedLogs
        {
            get { return _assignedLogs; }
            set { SetProperty(ref _assignedLogs, value); }
        }

        public RelayCommand AddLogCmd { get; }
        public RelayCommand DeleteMeasurementPointCmd { get; }
        public event EventHandler DeleteRequested = delegate { };

    }
}
