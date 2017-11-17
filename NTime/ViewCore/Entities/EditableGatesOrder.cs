using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;

namespace ViewCore.Entities
{
    public class EditableGatesOrder : BindableBase
    {
        public EditableGatesOrder(ObservableCollection<IEditableGate> definedGates)
        {
            _definedGates = definedGates;
        }

        private BaseCore.DataBase.GateOrderItem _gateOrderItem = new BaseCore.DataBase.GateOrderItem();
        public BaseCore.DataBase.GateOrderItem GateOrderItem
        {
            get { return _gateOrderItem; }
            set { _gateOrderItem = value; }
        }


        //public int Number
        //{
        //    get { return GateOrderItem.Number; }
        //    set { GateOrderItem.Number = SetProperty(GateOrderItem.Number, value); }
        //}


        private EditableGate _gate = new EditableGate(new ViewCore.Entities.LogsInfo());
        public EditableGate Gate
        {
            get { return _gate; }
            set { SetProperty(ref _gate, value); }
        }


        public decimal MinTimeBefore
        {
            get { return GateOrderItem.MinTimeBefore; }
            set { GateOrderItem.MinTimeBefore = SetProperty(GateOrderItem.MinTimeBefore, value); }
        }


        private bool _isTimeCollapsed;

        private ObservableCollection<ViewCore.Entities.IEditableGate> _definedGates;
        public ObservableCollection<ViewCore.Entities.IEditableGate> DefinedGates
        {
            get { return _definedGates; }
            set { SetProperty(ref _definedGates, value); }
        }

        public bool IsTimeCollapsed
        {
            get { return _isTimeCollapsed; }
            set { SetProperty(ref _isTimeCollapsed, value); }
        }
    }
}
