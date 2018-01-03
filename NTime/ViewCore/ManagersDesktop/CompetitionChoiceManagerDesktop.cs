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
    public class CompetitionChoiceManagerDesktop : BindableBase, ICompetitionChoiceManager
    {
        private CompetitionRepository _competitionRepository;
        private ObservableCollection<EditableCompetition> _competitions = new ObservableCollection<EditableCompetition>();

        public CompetitionChoiceManagerDesktop()
        {
            _competitionRepository = new CompetitionRepository(new ContextProvider());
        }

        public ObservableCollection<EditableCompetition> GetCompetitionsToDisplay() => _competitions;

        public void DownloadDataFromDatabase()
        {
            var repository = new CompetitionRepository(new ContextProvider());
            DownloadCompetitions(repository);
        }

        private async void DownloadCompetitions(CompetitionRepository repository)
        {
            var dbCompetitions = new List<Competition>(await repository.GetAllAsync());
            foreach (var dbCompetition in dbCompetitions)
            {
                _competitions.Add(new EditableCompetition() { DbEntity = dbCompetition });
            }
        }

        public async void ClearDatabase()
        {
            await _competitionRepository.RemoveAllAsync();
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
