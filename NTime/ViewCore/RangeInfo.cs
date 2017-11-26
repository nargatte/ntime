using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;

namespace ViewCore
{
    public class RangeInfo : BindableBase
    {

        public int FirstItem => 1 + (PageNumber - 1) * ItemsPerPage;


        public int LastItem
        {
            get
            {
                if (PageNumber * ItemsPerPage < TotalItemsCount)
                    return PageNumber * ItemsPerPage;
                else
                    return TotalItemsCount;
            }
        }

        private int _pageNumber;
        /// <summary>
        /// Numer strony począwszy od 1
        /// </summary>
        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                SetProperty(ref _pageNumber, value);
                NotifyDisplayedProperties();
            }
        }


        private int _itemsPerPage;
        public int ItemsPerPage
        {
            get { return _itemsPerPage; }
            set
            {
                SetProperty(ref _itemsPerPage, value);
                NotifyDisplayedProperties();
            }
        }


        private int _totalItemsCount;
        public int TotalItemsCount
        {
            get { return _totalItemsCount; }
            set
            {
                SetProperty(ref _totalItemsCount, value);
                NotifyDisplayedProperties();
            }
        }

        private void NotifyDisplayedProperties()
        {
            OnPropertyChanged(nameof(LastItem));
            OnPropertyChanged(nameof(FirstItem));
            OnPropertyChanged(nameof(TotalItemsCount));
            ChildUpdated();
        }

        public event Action ChildUpdated = delegate { };

    //    protected override void SetProperty<T>(ref T member, T val,
    //[CallerMemberName] string propertyName = null)
    //    {
    //        if (object.Equals(member, val)) return;

    //        member = val;
    //        NotifyDisplayedProperties();
    //    }
    }
}
