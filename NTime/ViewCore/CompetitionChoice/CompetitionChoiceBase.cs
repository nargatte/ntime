using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore.Entities;
using ViewCore.Managers;

namespace ViewCore
{
    public class CompetitionChoiceBase : BindableBase
    {

        public ICompetitionChoiceManager CompetitionChoiceManager { get; set; }

        public CompetitionChoiceBase()
        {
            _selectedCompetition = new EditableCompetition();
        }
        #region Properties
        private ObservableCollection<EditableCompetition> _competitions = new ObservableCollection<EditableCompetition>();
        public ObservableCollection<EditableCompetition> Competitions
        {
            get { return _competitions; }
            set { SetProperty(ref _competitions, value); }
        }

        private EditableCompetition _selectedCompetition;
        public EditableCompetition SelectedCompetition
        {
            get { return _selectedCompetition; }
            set
            {
                SetProperty(ref _selectedCompetition, value);
                if (!string.IsNullOrWhiteSpace(SelectedCompetition.Name))
                {
                    IsCompetitionSelected = true;
                }
                CompetitionSelected();
            }
        }


        private bool _isCompetitionSelected;
        public bool IsCompetitionSelected
        {
            get { return _isCompetitionSelected; }
            set { _isCompetitionSelected = value; }
        }

        public event Action CompetitionSelected = delegate { };

        #endregion
    }
}
