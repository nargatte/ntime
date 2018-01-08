using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore.Entities;
using ViewCore.ManagersInterfaces;

namespace ViewCore.ManagersDesktop
{
    public class CompetitionManagerDesktop : BindableBase, ICompetitionManager
    {
        private CompetitionRepository _competitionRepository;
        private ObservableCollection<EditableCompetition> _competitions = new ObservableCollection<EditableCompetition>();

        public CompetitionManagerDesktop()
        {
            _competitionRepository = new CompetitionRepository(new ContextProvider());
        }

        public ObservableCollection<EditableCompetition> GetCompetitionsToDisplay() => _competitions;

        public async void DownloadDataFromDatabase()
        {
            var dbCompetitions = new List<Competition>(await _competitionRepository.GetAllAsync());
            foreach (var dbCompetition in dbCompetitions)
            {
                _competitions.Add(new EditableCompetition() { DbEntity = dbCompetition });
            }
        }

        //private async void DownloadCompetitions(CompetitionRepository repository)
        //{
        //    var dbCompetitions = new List<Competition>(await repository.GetAllAsync());
        //    foreach (var dbCompetition in dbCompetitions)
        //    {
        //        _competitions.Add(new EditableCompetition() { DbEntity = dbCompetition });
        //    }
        //}

        public void ClearDatabase()
        {
            //await _competitionRepository.RemoveAllAsync();
            System.Windows.MessageBox.Show("Nie masz uprawnień by usunąć wszystkie zawody");
        }

        public async void AddSampleCompetitionsToDatabase()
        {
            await _competitionRepository.AddRangeAsync(new List<Competition>()
            {
            new Competition(
                "Zawody 1", new DateTime(2017, 11, 6), null, null, null, "Poznań"),
            new Competition(
                "Zawody 2", new DateTime(2017, 11, 6), null, null, null, "Łódź"),
            new Competition(
                "Zawody 3", new DateTime(2017, 11, 6), "Opis zawodów 3", null, null, "Warszawa"),
            new Competition(
                "Zawody 4", new DateTime(2017, 12, 1), null, null, null, "Gdynia")
            });
        }

        public async Task AddAsync(Competition dbEntity)
        {
            await _competitionRepository.AddAsync(dbEntity);
        }
    }
}
