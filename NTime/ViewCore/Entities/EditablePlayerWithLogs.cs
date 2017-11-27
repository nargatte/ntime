using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using BaseCore.TimesProcess;

namespace ViewCore.Entities
{
    public class EditablePlayerWithLogs : EditableBaseClass<Player>
    {
        public EditablePlayerWithLogs(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            
        }


        #region Properties
        public int StartNumber
        {
            get { return DbEntity.StartNumber; }
            set { DbEntity.StartNumber = SetProperty(DbEntity.StartNumber, value); }
        }


        public string FirstName
        {
            get { return DbEntity.FirstName; }
            set { DbEntity.FirstName = SetProperty(DbEntity.FirstName, value); }
        }


        public string LastName
        {
            get { return DbEntity.LastName; }
            set { DbEntity.LastName = SetProperty(DbEntity.LastName, value); }
        }


        public string StartTime
        {
            get { return DbEntity.StartTime.GetValueOrDefault().ConvertToString(); }
            set
            {
                if (value.TryConvertToDateTime(out DateTime dateTime))
                    DbEntity.StartTime = SetProperty(DbEntity.StartTime, dateTime);
            }
        }

        public string Distance
        {
            get { return DbEntity?.Distance.Name; }
            set { }
        }


        private ObservableCollection<EditableTimeRead> _timeReads = new ObservableCollection<EditableTimeRead>();
        public ObservableCollection<EditableTimeRead> TimeReads
        {
            get { return _timeReads; }
            set { SetProperty(ref _timeReads, value); }
        }
        #endregion



        public async void DownloadTimeReads(bool onlySignificant = false)
        {
            var repository = new TimeReadRepository(new ContextProvider(), DbEntity);
            var dbTimeReads = await repository.GetAllAsync();
            foreach (var dbTimeRead in dbTimeReads)
            {
                if (onlySignificant)
                {
                    if(dbTimeRead.TimeReadTypeEnum == TimeReadTypeEnum.NonsignificantAfter) continue;
                    if(dbTimeRead.TimeReadTypeEnum == TimeReadTypeEnum.NonsignificantBefore) continue;
                    if(dbTimeRead.TimeReadTypeEnum == TimeReadTypeEnum.Repeated) continue;
                    if(dbTimeRead.TimeReadTypeEnum == TimeReadTypeEnum.Unprocessed) continue;
                }
                TimeReads.Add(new EditableTimeRead(_currentCompetition)
                {
                    DbEntity = dbTimeRead
                });
            }

        }

    }
}
