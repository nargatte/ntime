using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BaseCore.DataBase;
using BaseCore.PlayerFilter;
using BaseCore.TimesProcess;
using ViewCore.Entities;
using ViewCore.ManagersInterfaces;

namespace ViewCore.ManagersDesktop
{
    public class PlayerManagerDesktop : CompetitionItemBase, IPlayerManager
    {
        private PlayerRepository _playerRepository;
        private ObservableCollection<EditableDistance> _definedDistances;
        private ObservableCollection<EditableSubcategory> _definedSubcategories;
        private PlayerFilterOptions _playerFilter = new PlayerFilterOptions();
        private RangeInfo _recordsRangeInfo;
        private ObservableCollection<EditablePlayer> _players = new ObservableCollection<EditablePlayer>();

        public PlayerManagerDesktop(IEditableCompetition currentCompetition, ObservableCollection<EditableDistance> definedDistances,
            ObservableCollection<EditableSubcategory> definedSubcategories, RangeInfo recordsRangeInfo) : base(currentCompetition)
        {
            _playerRepository = new PlayerRepository(new ContextProvider(), _currentCompetition.DbEntity);
            _definedDistances = definedDistances;
            _definedSubcategories = definedSubcategories;
            _recordsRangeInfo = recordsRangeInfo;
        }

        public ObservableCollection<EditablePlayer> GetPlayersToDisplay() => _players;
        public RangeInfo GetRecordsRangeInfo() => _recordsRangeInfo;


        public async Task UpdateFilterInfo(int pageNumber, string query, SortOrderEnum? sortOrder, PlayerSort? sortCriteria,
            EditableDistance distanceSortCriteria, EditableSubcategory subcategorySortCriteria,
            EditableAgeCategory ageCategorySortCriteria)
        {
            _recordsRangeInfo.PageNumber = pageNumber;
            _playerFilter.Query = query;
            if (!String.IsNullOrWhiteSpace(distanceSortCriteria?.DbEntity.Name))
                _playerFilter.Distance = distanceSortCriteria.DbEntity;
            else
                _playerFilter.Distance = null;
            if (!String.IsNullOrWhiteSpace(subcategorySortCriteria?.DbEntity.Name))
                _playerFilter.Subcategory = subcategorySortCriteria.DbEntity;
            else
                _playerFilter.Subcategory = null;
            if (!String.IsNullOrWhiteSpace(ageCategorySortCriteria?.DbEntity.Name))
                _playerFilter.AgeCategory = ageCategorySortCriteria.DbEntity;
            else
                _playerFilter.AgeCategory = null;
            if (sortOrder.HasValue && sortOrder.Value == SortOrderEnum.Descending)
                _playerFilter.DescendingSort = true;
            else
                _playerFilter.DescendingSort = false;
            if (sortCriteria.HasValue)
                _playerFilter.PlayerSort = sortCriteria.Value;
            await AddPlayersFromDatabase(removeAllDisplayedBefore: true);
        }

        public async Task DeleteAllPlayersFromDatabaseAsync()
        {
            await _playerRepository.RemoveAllAsync();
            _players.Clear();
        }


        public void DeleteSelectedPlayersFromDatabaseAsync(EditablePlayer[] selectedPlayersArray)
        {
            foreach (var player in selectedPlayersArray)
            {
                _playerRepository.RemoveAsync(player.DbEntity);
                _players.Remove(player);
            }
            _recordsRangeInfo.TotalItemsCount -= selectedPlayersArray.Length;
        }

        //private async Task DeleteSelectedPlayersFromDatabaseAsync(ICollection<EditablePlayer> selectedPlayersList)
        //{
        //    List<Task> tasks = new List<Task>();
        //    foreach (var player in selectedPlayersList)
        //    {
        //        tasks.Add(_playerRepository.RemoveAsync(player.DbEntity));
        //        _players.Remove(player);
        //    }
        //    await Task.WhenAll(tasks);
        //}

        public async Task<bool> TryAddPlayerAsync(EditablePlayer newPlayer)
        {
            if (CanAddPlayer(newPlayer, out string message))
            {
                newPlayer.IsMale = GetSexForPlayer(newPlayer);
                var playerToAdd = newPlayer.Clone() as EditablePlayer;
                var tempDistance = playerToAdd.DbEntity.Distance;
                var tempSubcategory = playerToAdd.DbEntity.Subcategory;
                await _playerRepository.AddAsync(playerToAdd.DbEntity, playerToAdd.DbEntity.AgeCategory, playerToAdd.DbEntity.Distance, playerToAdd.DbEntity.Subcategory);
                playerToAdd.DbEntity.Distance = tempDistance;
                playerToAdd.DbEntity.Subcategory = tempSubcategory;
                playerToAdd.UpdateRequested += Player_UpdateRequested;
                _players.Add(playerToAdd);
                _recordsRangeInfo.TotalItemsCount++;
                return true;
            }
            else
            {
                MessageBox.Show(message);
                return false;
            }

        }
        private bool CanAddPlayer(EditablePlayer newPlayer, out string message)
        {
            message = "";
            if (String.IsNullOrWhiteSpace(newPlayer.LastName))
            {
                message = "Nazwisko nie może być puste";
                return false;
            }
            newPlayer.StartTime.TryConvertToDateTime(out DateTime startTimeDateTime);
            if (startTimeDateTime < new DateTime(2000, 1, 1))
            {
                message = "Nieprawidłowy czas startu zawodnika";
                return false;
            }
            if (newPlayer.Distance == null || String.IsNullOrWhiteSpace(newPlayer.Distance.Name))
            {
                message = "Nie przypisano żadnego dystansu";
                return false;
            }
            if (newPlayer.Subcategory == null || String.IsNullOrWhiteSpace(newPlayer.Subcategory.Name))
            {
                message = "Nie przypisano Dodatkowych informacji";
                return false;
            }

            return true;
        }

        private bool GetSexForPlayer(EditablePlayer newPlayer)
        {
            char[] firstName = newPlayer.FirstName.ToCharArray();
            if (firstName.Last() == 'a' && newPlayer.FirstName.ToLower() != "kuba")
                return false;
            else
                return true;
        }

        private async void Player_UpdateRequested(object sender, EventArgs e)
        {
            var playerToUpdate = sender as EditablePlayer;
            await _playerRepository.UpdateAsync(playerToUpdate.DbEntity, playerToUpdate.DbEntity.AgeCategory, playerToUpdate.DbEntity.Distance,
                playerToUpdate.DbEntity.Subcategory);
            playerToUpdate.UpdateFullCategoryDisplay();
        }

        

        public async Task NavToNextPageAsync()
        {
            if (_recordsRangeInfo.LastItem < _recordsRangeInfo.TotalItemsCount)
            {
                _recordsRangeInfo.PageNumber++;
                await AddPlayersFromDatabase(removeAllDisplayedBefore: true);
            }
        }

        public async Task NavToPreviousPageAsync()
        {
            if (_recordsRangeInfo.PageNumber > 1)
            {
                _recordsRangeInfo.PageNumber--;
                await AddPlayersFromDatabase(removeAllDisplayedBefore: true);
            }
        }





        public async Task AddPlayersFromCsvToDatabase()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "CSV files (*.csv)|*.csv";
            if (dialog.ShowDialog().Value)
            {
                var path = dialog.FileName;
                var temp = await _playerRepository.ImportPlayersAsync(path);
                await AddPlayersFromDatabase(removeAllDisplayedBefore: true);
                MessageBox.Show($"Odczytano {temp.Item2} zawodników, z czego {temp.Item1} zostało dodanych do bazy");
            }
        }

        public async Task AddPlayersFromDatabase(bool removeAllDisplayedBefore)
        {
            if (removeAllDisplayedBefore)
                _players = new ObservableCollection<EditablePlayer>();
            var dbPlayers = await DownloadPlayersFromDataBase(_playerFilter, _recordsRangeInfo.PageNumber - 1, _recordsRangeInfo.ItemsPerPage);
            foreach (var dbPlayer in dbPlayers)
            {
                EditablePlayer playerToAdd = new EditablePlayer(_currentCompetition, _definedDistances, _definedSubcategories, dbPlayer);
                playerToAdd.UpdateRequested += Player_UpdateRequested;
                _players.Add(playerToAdd);
            }
        }

        private async Task<Player[]> DownloadPlayersFromDataBase(PlayerFilterOptions playerFilter, int pageNumber, int numberOfItemsOnPage)
        {
            var dbPlayersTuple = await _playerRepository.GetAllByFilterAsync(playerFilter, pageNumber, numberOfItemsOnPage);
            _recordsRangeInfo.TotalItemsCount = dbPlayersTuple.Item2;
            return dbPlayersTuple.Item1;
        }

        public Task<EditablePlayer> GetFullRegisteredPlayerFromCompetition(Competition competition, PlayerAccount playerAccount)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePlayerRegisterInfo(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
