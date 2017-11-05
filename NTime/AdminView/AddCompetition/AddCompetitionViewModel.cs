using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;

namespace AdminView.AddCompetition
{
    class AddCompetitionViewModel : BindableBase
    {

        public AddCompetitionViewModel()
        {
            _competition = new ObservableCollection<BaseCore.DataBase.Competition>();
            Competition.Add(new Competition
            {
                Id = 78,
                Name = "Comp",
                City = "Poznań",
                Description = "W poznaniu",
                EventDate = new DateTime(2017, 12, 3),
                CompetitionTypeId = (int)CompetitionTypeEnum.Fastest
            });
        }

        private ObservableCollection<Competition> _competition;
        public ObservableCollection<Competition> Competition
        {
            get { return _competition; }
            set { SetProperty(ref _competition, value); }
        }
    }
}
