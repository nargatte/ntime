using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore.Entities;

namespace ViewCore
{
    public class CompetitionSelectionBase : TabItemViewModel
    {
        public CompetitionSelectionBase(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
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
                    CompetitionSelected = true;
            }
        }


        private bool _competitionSelected;
        public bool CompetitionSelected
        {
            get { return _competitionSelected; }
            set
            {
                _competitionSelected = value;
                GoToCompetitionCmd.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Methods and Events

        public RelayCommand GoToCompetitionCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; private set; }


        protected void OnViewLoaded()
        {
            DownloadDataFromDatabase();
        }

        protected void DownloadDataFromDatabase()
        {
            var repository = new CompetitionRepository(new ContextProvider());
            DownloadCompetitions(repository);
        }

        protected void OnGoToCompetition()
        {

        }


        protected bool CanGoToCompetition()
        {
            return CompetitionSelected;
        }

        #endregion

        #region Database

        private async void DownloadCompetitions(CompetitionRepository repository)
        {
            var dbCompetitions = new List<Competition>(await repository.GetAllAsync());
            foreach (var dbCompetition in dbCompetitions)
            {
                Competitions.Add(new EditableCompetition() { DbEntity = dbCompetition });
            }
        }
        #endregion
    }
}
