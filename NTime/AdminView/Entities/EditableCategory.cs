using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Entities
{
    class EditableCategory : BindableBase
    {

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private int _yearFrom;
        public int YearFrom
        {
            get { return _yearFrom; }
            set { SetProperty(ref _yearFrom, value); }
        }


        private int _yearTo;
        public int YearTo
        {
            get { return _yearTo; }
            set { SetProperty(ref _yearTo, value); }
        }
    }
}
