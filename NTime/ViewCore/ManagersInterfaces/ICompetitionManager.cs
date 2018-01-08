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
        void DownloadDataFromDatabase();
        ObservableCollection<EditableCompetition> GetCompetitionsToDisplay();
    }
}