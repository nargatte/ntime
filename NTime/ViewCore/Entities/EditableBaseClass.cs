using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using BaseCore.DataBase;

namespace ViewCore
{
    public abstract class EditableBaseClass<T> : BindableBase
    {
        public EditableBaseClass()
        {
            DbEntity = Activator.CreateInstance<T>();
        }
        private T _dbEntity;
        public T DbEntity
        {
            get { return _dbEntity; }
            set { SetProperty(ref _dbEntity, value); }
        }

        //TODO thinks if that is necessary and possiple
        public async Task  UpdateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
