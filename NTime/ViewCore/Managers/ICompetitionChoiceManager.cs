using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BaseCore.DataBase;
using ViewCore.Entities;

namespace ViewCore.Managers
{
    public interface ICompetitionChoiceManager
    {
        Task AddAsync(Competition dbEntity);
        void ClearDatabase();
        void DownloadDataFromDatabase();
        ObservableCollection<EditableCompetition> GetCompetitionsToDisplay();
    }
}