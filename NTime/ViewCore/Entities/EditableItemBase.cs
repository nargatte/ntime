using MvvmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public abstract class EditableItemBase<T> : BindableBase
    {
        public EditableItemBase()
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
