using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using BaseCore.DataBase;
using ViewCore.Entities;

namespace ViewCore
{
    public abstract class EditableBaseClass<T> : BindableBase
    {

        protected IEditableCompetition _currentCompetition;
        private EditableBaseClass()
        {
            DbEntity = Activator.CreateInstance<T>();
        }
        public EditableBaseClass(IEditableCompetition currentComptetition) : this()
        {
            _currentCompetition = currentComptetition;
        }
        private T _dbEntity;
        public T DbEntity
        {
            get { return _dbEntity; }
            set { SetProperty(ref _dbEntity, value); }
        }

        //TODO thinks if that is necessary and possiple
        //public async Task  UpdateAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
