﻿using BaseCore.DataBase;
using BaseCore.TimesProcess;

namespace ViewCore.Entities
{
    public class EditableTimeRead : EditableCompetitionItemBase<TimeRead>
    {
        public EditableTimeRead(IEditableCompetition currentCompetition) : base(currentCompetition) { }


        public TimeReadTypeEnum TimeReadType
        {
            get { return DbEntity.TimeReadTypeEnum; }
            set { DbEntity.TimeReadTypeEnum = SetProperty(DbEntity.TimeReadTypeEnum, value); }
        }

        public string Time
        {
            get { return DbEntity.Time.ToDateTime().ConvertToString(); }
            //set
            //{
            //    if(value.TryConvertToDateTime(out ))
            //        else
            //    DbEntity.Time = SetProperty(DbEntity.Time, value.conv
            //  );
            //}
        }

        public int Reader
        {
            get { return DbEntity.Gate.Number; }
        }

    }
}
