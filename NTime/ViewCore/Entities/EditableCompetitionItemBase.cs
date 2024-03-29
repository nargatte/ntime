﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using BaseCore.DataBase;
using ViewCore.Entities;

namespace ViewCore
{
    public abstract class EditableCompetitionItemBase<T> : CompetitionItemBase where T : class
    {
        public EditableCompetitionItemBase(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            DbEntity = Activator.CreateInstance<T>();
        }

        private T _dbEntity;
        public T DbEntity
        {
            get { return _dbEntity; }
            set { SetProperty(ref _dbEntity, value); }
        }
    }
}
