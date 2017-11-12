using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AdminView.Scores
{
    class ScoresViewModel : TabItemViewModel, IViewModel
    {
        public ScoresViewModel()
        {
            TabTitle = "Wyniki";
            FillScores();
        }

        private void FillScores()
        {
            for (int i = 0; i < 20; i++)
            {
                Scores.Add(new Entities.EditableScore()
                {
                    CategoryPlaceNumber = 3,
                    DistancePlaceNumber = 1,
                    LapsCount = 5,
                    Time = "04:20:37.255",
                    Player = new Entities.EditablePlayer()
                    {
                        LastName = "Kierzkowski",
                        FirstName = "Jan",
                        Team = "Niezniszczalni Zgierz",
                        BirthDate = new DateTime(1994, 10, 09),
                        IsMale = true,
                        StartNumber = 18
                    }
                });
            }
        }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }

        private Entities.EditableScore _scoreFilter;
        public Entities.EditableScore ScoreFilter
        {
            get { return _scoreFilter; }
            set { SetProperty(ref _scoreFilter, value); }
        }

        private ObservableCollection<Entities.EditableScore> _scores = new ObservableCollection<Entities.EditableScore>();
        public ObservableCollection<Entities.EditableScore> Scores
        {
            get { return _scores; }
            set { SetProperty(ref _scores, value); }
        }
    }
}
