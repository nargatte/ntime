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

        private BaseCore.DataBase.GatesOrder _gatesOrder = new BaseCore.DataBase.GatesOrder();
        public BaseCore.DataBase.GatesOrder GatesOrder
        {
            get { return _gatesOrder; }
            set { _gatesOrder = value; }
        }


        //public int GateNumber
        //{
        //    get { return GatesOrder.GateNumber; }
        //    set { GatesOrder.GateNumber = SetProperty(GatesOrder.GateNumber, value); }
        //}


        private EditableGate _gate = new EditableGate(new ViewCore.Entities.LogsInfo());
        public EditableGate Gate
        {
            get { return _gate; }
            set { SetProperty(ref _gate, value); }
        }


        public decimal MinTimeBefore
        {
            get { return GatesOrder.MinTimeBefore; }
            set { GatesOrder.MinTimeBefore = SetProperty(GatesOrder.MinTimeBefore, value); }
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
