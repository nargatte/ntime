﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;

namespace ViewCore.Entities
{
    public class EditableExtraPlayerInfo : EditableBaseClass<Subcategory>
    {
        public EditableExtraPlayerInfo(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            DeleteExtraPlayerInfoCmd = new RelayCommand(OnDeleteExtraPlayerInfo);
        }
        public string Name
        {
            get { return DbEntity.Name; }
            set
            {
                DbEntity.Name = SetProperty(DbEntity.Name, value);
                OnUpdateRequested();
            }
        }

        public string ShortName
        {
            get { return DbEntity.ShortName; }
            set
            {
                DbEntity.ShortName = SetProperty(DbEntity.ShortName, value);
                OnUpdateRequested();
            }
        }

        private void OnDeleteExtraPlayerInfo()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        public RelayCommand DeleteExtraPlayerInfoCmd { get; private set; }
        public event EventHandler DeleteRequested = delegate { };
        public event EventHandler UpdateRequested = delegate { };

        protected void OnUpdateRequested()
        {
            UpdateRequested?.Invoke(this, EventArgs.Empty);
        }

    }
}
