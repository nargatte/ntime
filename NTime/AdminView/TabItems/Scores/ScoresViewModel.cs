using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;

namespace AdminView.Scores
{
    class ScoresViewModel : TabItemViewModel<Competition>, ViewCore.Entities.ISwitchableViewModel
    {
        public ScoresViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Wyniki";
            FillScores();
        }

        private void FillScores()
        {
            for (int i = 0; i < 20; i++)
            {
                Scores.Add(new ViewCore.Entities.EditablePlayer(_currentCompetition)
                {
                    DbEntity = new BaseCore.DataBase.Player()
                    {
                        LastName = "Kierzkowski",
                        FirstName = "Jan",
                        Team = "Niezniszczalni Zgierz",
                        BirthDate = new DateTime(1994, 10, 09),
                        IsMale = true,
                        StartNumber = 18,
                        CategoryPlaceNumber = 3,
                        DistancePlaceNumber = 1,
                        LapsCount = 5,
                        Time = 12345,
                    }
                });
            }
        }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }

        private ViewCore.Entities.EditablePlayer _scoreFilter;
        public ViewCore.Entities.EditablePlayer ScoreFilter
        {
            get { return _scoreFilter; }
            set { SetProperty(ref _scoreFilter, value); }
        }

        private ObservableCollection<ViewCore.Entities.EditablePlayer> _scores = new ObservableCollection<ViewCore.Entities.EditablePlayer>();
        public ObservableCollection<ViewCore.Entities.EditablePlayer> Scores
        {
            get { return _scores; }
            set { SetProperty(ref _scores, value); }
        }
    }
}
