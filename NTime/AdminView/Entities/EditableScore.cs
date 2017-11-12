using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Entities
{
    class EditableScore : BindableBase
    {

        private Entities.EditablePlayer _player;
        public Entities.EditablePlayer Player
        {
            get { return _player; }
            set { SetProperty(ref _player, value); }
        }


        private int _lapsCount;
        public int LapsCount
        {
            get { return _lapsCount; }
            set { SetProperty(ref _lapsCount, value); }
        }


        private int _distancePlaceNumber;
        public int DistancePlaceNumber
        {
            get { return _distancePlaceNumber; }
            set { SetProperty(ref _distancePlaceNumber, value); }
        }


        private int _categoryPlaceNumber;
        public int CategoryPlaceNumber
        {
            get { return _categoryPlaceNumber; }
            set { SetProperty(ref _categoryPlaceNumber, value); }
        }


        private string _time;
        public string Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }
    }
}
