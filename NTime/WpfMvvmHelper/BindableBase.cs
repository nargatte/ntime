using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MvvmHelper
{
    public class BindableBase : INotifyPropertyChanged 
    {
        protected virtual void SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual T SetProperty<T>(T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return member;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            return member;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

    }

    public class BindableBase<T> : BindableBase
    {
        protected virtual void SetProperty(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            OnPropertyChanged(propertyName);
        }
    }
}
