using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BaseCore.DataBase;
using BaseCore.PlayerFilter;
using ViewCore.Entities;

namespace ViewCore.ManagersInterfaces
{
    public interface IPlayersManager
    {
        Task AddPlayersFromDatabase(bool removeAllDisplayedBefore);
        Task DeleteAllPlayersFromDatabaseAsync();
        void DeleteSelectedPlayersFromDatabaseAsync(EditablePlayer[] selectedPlayersArray);
        ObservableCollection<EditablePlayer> GetPlayersToDisplay();
        RangeInfo GetRecordsRangeInfo();
        Task NavToNextPageAsync();
        Task NavToPreviousPageAsync();
        Task<bool> TryAddPlayerAsync(EditablePlayer newPlayer);
        Task UpdateFilterInfo(int pageNumber, string query, SortOrderEnum? sortOrder, PlayerSort? sortCriteria, EditableDistance distanceSortCriteria, EditableExtraPlayerInfo extraPlayerInfoSortCriteria, EditableAgeCategory ageCategorySortCriteria);
    }
}