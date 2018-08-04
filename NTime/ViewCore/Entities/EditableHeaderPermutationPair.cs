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

        private void OnDeleteExtraHeader()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        public RelayCommand DeleteExtraHeaderCmd { get; private set; }
        public event EventHandler DeleteRequested = delegate { };
        public event EventHandler UpdateRequested = delegate { };

        protected void OnUpdateRequested()
        {
            UpdateRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
