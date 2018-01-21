using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BaseCore.DataBase;
using ViewCore.Entities;

namespace ViewCore.ManagersInterfaces
{
    public interface ICompetitionManager
    {
        Task AddAsync(Competition dbEntity);
        void ClearDatabase();
        Task DownloadDataFromDatabase();
        ObservableCollection<EditableCompetition> GetCompetitionsToDisplay();
        Task GetCompetitionsForPlayerAccount();
    }
}