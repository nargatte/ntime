using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using BaseCore.Models;
using BaseCore.PlayerFilter;
using BaseCore.TimesProcess;
using Server.Dtos;
using ViewCore.Entities;
using ViewCore.HttpClients;
using ViewCore.ManagersInterfaces;

namespace ViewCore.ManagersHttp
{
    public class PlayerManagerHttp : ManagerHttp, IPlayerManager
    {
        private ObservableCollection<EditableDistance> _definedDistances;
        private ObservableCollection<EditableSubcategory> _definedSubcategories;
        private PlayerFilterOptions _playerFilter = new PlayerFilterOptions();
        private RangeInfo _recordsRangeInfo;
        private ObservableCollection<EditablePlayer> _players = new ObservableCollection<EditablePlayer>();
        private IEditableCompetition _currentCompetition;
        private HttpPlayerClient _client;

        public PlayerManagerHttp(IEditableCompetition currentCompetition, ObservableCollection<EditableDistance> definedDistances,
                                    ObservableCollection<EditableSubcategory> definedSubcategories, RangeInfo recordsRangeInfo,
                                    AccountInfo accountInfo, ConnectionInfo connectionInfo)
                                    : base(accountInfo, connectionInfo)
        {
            _currentCompetition = currentCompetition;
            _definedDistances = definedDistances;
            _definedSubcategories = definedSubcategories;
            _recordsRangeInfo = recordsRangeInfo;
            _client = new HttpPlayerClient(accountInfo, connectionInfo, "Player");
        }

        public ObservableCollection<EditablePlayer> GetPlayersToDisplay() => _players;
        public RangeInfo GetRecordsRangeInfo() => _recordsRangeInfo;

        public async Task UpdateFilterInfo(int pageNumber, string query, SortOrderEnum? sortOrder, PlayerSort? sortCriteria,
            EditableDistance distanceSortCriteria, EditableSubcategory subcategorySortCriteria, EditableAgeCategory ageCategorySortCriteria)
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

        public async void DeleteSelectedPlayersFromDatabaseAsync(EditablePlayer[] selectedPlayersArray)
        {
            foreach (var player in selectedPlayersArray)
            {
                await _client.Delete(player.DbEntity);
                _players.Remove(player);
            }
            _recordsRangeInfo.TotalItemsCount -= selectedPlayersArray.Length;
        }

        public async Task<EditablePlayer> GetFullRegisteredPlayerFromCompetition(Competition competition, PlayerAccount playerAccount)
        {
            EditablePlayer player = new EditablePlayer(new EditableCompetition() { DbEntity = competition});
            await TryCallApi(async () =>
            {
                player = new EditablePlayer(new EditableCompetition() { DbEntity = competition })
                {
                    DbEntity = (await _client.GetFullRegisteredPlayerFromCompetition(competition, playerAccount)).CopyDataFromDto(new Player())
                };
                player.Distance = _definedDistances.FirstOrDefault(defined => defined.DbEntity.Id == player.DbEntity.DistanceId);
                player.Subcategory = _definedSubcategories.FirstOrDefault(defined => defined.DbEntity.Id == player.DbEntity.SubcategoryId);
            });
            if (player.DbEntity != null)
            {
                return player;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> TryAddPlayerAsync(EditablePlayer newPlayer)
        {
            if (CanAddPlayer(newPlayer, out string message))
            {
                newPlayer.IsMale = GetSexForPlayer(newPlayer);
                var playerToAdd = newPlayer.Clone() as EditablePlayer;
                var tempDistance = playerToAdd.DbEntity.Distance;
                var tempSubcategory = playerToAdd.DbEntity.Subcategory;
                await TryCallApi(async () =>
                    {
                        playerToAdd.CompetitionId = _currentCompetition.DbEntity.Id;
                        await _client.AddRegisteredPlayer(_currentCompetition.DbEntity.Id, playerToAdd.DbEntity);
                    }, "Zapis na zawody przebiegł poprawnie");
                playerToAdd.DbEntity.Distance = tempDistance;
                playerToAdd.DbEntity.Subcategory = tempSubcategory;
                playerToAdd.UpdateRequested += Player_UpdateRequested;
                _players.Add(playerToAdd);
                //_recordsRangeInfo.TotalItemsCount++;
                return true;
            }
            else
            {
                System.Windows.MessageBox.Show(message);
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
            //await _playerRepository.UpdateAsync(playerToUpdate.DbEntity, playerToUpdate.DbEntity.Distance,
            //    playerToUpdate.DbEntity.Subcategory);
            await _client.UpdatePlayerFullInfo(playerToUpdate.DbEntity);
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

        public async Task AddPlayersFromDatabase(bool removeAllDisplayedBefore)
        {
            if (removeAllDisplayedBefore)
                _players = new ObservableCollection<EditablePlayer>();
            var dbPlayers = await DownloadPlayersFromDataBase(_playerFilter, _recordsRangeInfo.ItemsPerPage, _recordsRangeInfo.PageNumber - 1);
            foreach (var dbPlayer in dbPlayers)
            {
                EditablePlayer playerToAdd = new EditablePlayer(_currentCompetition, _definedDistances, _definedSubcategories, dbPlayer);
                playerToAdd.UpdateRequested += Player_UpdateRequested;
                _players.Add(playerToAdd);
            }
        }

        private async Task<Player[]> DownloadPlayersFromDataBase(PlayerFilterOptions playerFilter, int itemsOnPage, int pageNumber)
        {
            //PageViewModel<PlayerWithScoresDto> playerPageModel;
            IEnumerable<Player> downloadedPlayers = new List<Player>();
            await TryCallApi(async () =>
            {
                var playerPageModel = await _client.GetPlayersListView(_currentCompetition.DbEntity, itemsOnPage, pageNumber, playerFilter);
                _recordsRangeInfo.TotalItemsCount = playerPageModel.TotalCount;
                //    await _playerRepository.GetAllByFilterAsync(playerFilter, pageNumber, numberOfItemsOnPage);
                //_recordsRangeInfo.TotalItemsCount = dbPlayersTuple.Item2;
                downloadedPlayers = playerPageModel.Items.Select(dto => dto.CopyDataFromDto(new Player())).ToArray();
            });
            if (IsSuccess)
            {
                return downloadedPlayers.ToArray();
            }
            else
            {
                return new Player[0];
            }
        }

        public async Task AddPlayersFromCsvToDatabase()
        {
            await TryCallApi(async () =>
            {
                System.Windows.MessageBox.Show("Nie masz uprawnień do dodawania plików Csv");
            });
        }

        public async Task DeleteAllPlayersFromDatabaseAsync()
        {
            await TryCallApi(async () =>
            {
                System.Windows.MessageBox.Show("Nie masz uprawnień do usunięcia wszystkich zawodników");
            });
        }

        public async Task UpdatePlayerRegisterInfo(Player player)
        {
            await TryCallApi(async () =>
            {
                player.CompetitionId = _currentCompetition.DbEntity.Id;
                await _client.UpdatePlayerRegisterInfo(player);
            }, "Twoje dane zostały poprawnie zapisane");
        }
    }
}
