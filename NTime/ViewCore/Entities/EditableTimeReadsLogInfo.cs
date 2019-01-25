using System;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;

namespace ViewCore.Entities
{
    public class EditableTimeReadsLogInfo : EditableCompetitionItemBase<TimeReadsLogInfo>
    {
        ILogsInfo logsInfo;
        public EditableTimeReadsLogInfo(ILogsInfo logsInfo, IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            this.logsInfo = logsInfo;
            DeleteLogCmd = new RelayCommand(OnDeleteLog);
        }

        #region Properties
        public int LogNumber
        {
            get { return DbEntity.LogNumber; }
            set { DbEntity.LogNumber = SetProperty(DbEntity.LogNumber, value); }
        }

        public string Path
        {
            get { return DbEntity.Path; }
            set { DbEntity.Path = SetProperty(DbEntity.Path, value); }
        }

        #endregion

        private void OnDeleteLog()
        {
            DeleteRequested(this, EventArgs.Empty);
        }


        public RelayCommand DeleteLogCmd { get; }

        public event EventHandler DeleteRequested = delegate { };
    }
}
