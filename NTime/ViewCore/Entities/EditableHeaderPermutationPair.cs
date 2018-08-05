using BaseCore.Models;
using MvvmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public class EditableHeaderPermutationPair : EditableBaseClass<HeaderPermutationPair>
    {
        public EditableHeaderPermutationPair(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            DeleteExtraHeaderCmd = new RelayCommand(OnDeleteExtraHeader);
            MoveUpExtraHeaderCmd = new RelayCommand(OnMoveUpRequested);
            MoveDownExtraHeaderCmd = new RelayCommand(OnMoveDownRequested);
        }


        public int PermutationElement
        {
            get { return DbEntity.PermutationElement; }
            set
            {
                DbEntity.PermutationElement = SetProperty(DbEntity.PermutationElement, value);
                //OnUpdateRequested();
            }
        }

        public string HeaderName
        {
            get { return DbEntity.HeaderName; }
            set
            {
                DbEntity.HeaderName = SetProperty(DbEntity.HeaderName, value);
                //OnUpdateRequested();
            }
        }


        public RelayCommand DeleteExtraHeaderCmd { get; private set; }
        public RelayCommand MoveUpExtraHeaderCmd { get; private set; }
        public RelayCommand MoveDownExtraHeaderCmd { get; private set; }
        public event EventHandler DeleteRequested = delegate { };
        public event EventHandler MoveUpRequested = delegate { };
        public event EventHandler MoveDownRequested = delegate { };

        private void OnDeleteExtraHeader()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        protected void OnMoveUpRequested()
        {
            MoveUpRequested?.Invoke(this, EventArgs.Empty);
        }

        protected void OnMoveDownRequested()
        {
            MoveDownRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
