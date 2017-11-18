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
        public EditableGate(ILogsInfo logsInfo)
        {
            AddLogCmd = new RelayCommand(OnAddLog);
            DeleteMeasurementPointCmd = new RelayCommand(OnDeleteGate);
            this.logsInfo = logsInfo;
        }

        #region Methods
        private void OnDeleteGate()
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
                    var logToAdd = new EditableTimeReadsLogInfo(logsInfo)
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

        private int _number;
        public int Number
        {
            get { return _number; }
            set { SetProperty(ref _number, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
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
