using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;

namespace ViewCore.Entities
{
    public class EditableGatesOrder : BindableBase
    {
        private BaseCore.DataBase.GatesOrder _gatesOrder = new BaseCore.DataBase.GatesOrder();
        public BaseCore.DataBase.GatesOrder GatesOrder
        {
            get { return _gatesOrder; }
            set { _gatesOrder = value; }
        }


        public int GateNumber
        {
            get { return GatesOrder.GateNumber; }
            set { GatesOrder.GateNumber = SetProperty(GatesOrder.GateNumber, value); }
        }


        public decimal MinTimeBefore
        {
            get { return GatesOrder.MinTimeBefore; }
            set { GatesOrder.MinTimeBefore = SetProperty(GatesOrder.MinTimeBefore, value); }
        }


        private bool _isTimeCollapsed;
        public bool IsTimeCollapsed
        {
            get { return _isTimeCollapsed; }
            set { SetProperty(ref _isTimeCollapsed, value); }
        }
    }
}
