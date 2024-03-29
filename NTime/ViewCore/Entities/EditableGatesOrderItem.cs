﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using BaseCore.DataBase;

namespace ViewCore.Entities
{
    public class EditableGatesOrderItem : EditableCompetitionItemBase<GatesOrderItem>
    {
        public EditableGatesOrderItem(ObservableCollection<IEditableGate> definedGates, IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            //This has been changed
            DefinedGates = definedGates;
        }

        public IEditableGate Gate
        {
            get
            {
                var temp = DefinedGates.FirstOrDefault(g => Equals(g.DbEntity, DbEntity.Gate));
                return temp;
            }
            set
            {
                if (value != null)
                {
                    DbEntity.Gate = SetProperty(DbEntity.Gate, value.DbEntity);
                    DbEntity.Gate.Number = SetProperty(DbEntity.Gate.Number, value.DbEntity.Number, "Number");
                }
            }
        }

        private void UpdateGatesOrderItem()
        {
            UpdateGatesOrderItemRequested(this, EventArgs.Empty);
        }

        public decimal MinTimeBefore
        {
            get { return DbEntity.MinTimeBefore; }
            set { DbEntity.MinTimeBefore = SetProperty(DbEntity.MinTimeBefore, value); }
        }


        //public ObservableCollection<EditableGate> DbDefinedGates
        //{
        //    get { return ; }
        //    set { SetProperty(ref _dbDefinedGates, value); }
        //}

        private ObservableCollection<IEditableGate> _definedGates = new ObservableCollection<IEditableGate>();
        public ObservableCollection<IEditableGate> DefinedGates
        {
            get { return _definedGates; }
            set { SetProperty(ref _definedGates, value); }
        }

        private bool _isTimeCollapsed;
        public bool IsTimeCollapsed
        {
            get { return _isTimeCollapsed; }
            set { SetProperty(ref _isTimeCollapsed, value); }
        }

        public event EventHandler UpdateGatesOrderItemRequested = delegate { };
    }
}
