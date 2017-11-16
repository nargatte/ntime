using System;
using MvvmHelper;
using ViewCore;

namespace ViewCore.Entities
{
    public class EditableTimeReadsLog : BindableBase
    {
        ILogsInfo logsInfo;
        public EditableTimeReadsLog(ILogsInfo logsInfo)
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
