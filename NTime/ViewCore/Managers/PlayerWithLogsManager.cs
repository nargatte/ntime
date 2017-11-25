using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore.Entities;

namespace ViewCore.Managers
{
    public class PlayerWithLogsManager : CompetitionItemBase
    {
        private PlayerRepository _playerRepository;
        private ObservableCollection<EditablePlayerWithLogs> _playersWithLogs;

        public PlayerWithLogsManager(IEditableCompetition currentComptetition, PlayerRepository playerRepository) : base(currentComptetition)
        {
            _playerRepository = playerRepository;
        }

        public async Task<ObservableCollection<EditablePlayerWithLogs>> GetAllPlayers()
        {
            _playersWithLogs = new ObservableCollection<EditablePlayerWithLogs>();
            var dbPlayers = await _playerRepository.GetAllAsync();
            foreach (var dbPlayer in dbPlayers)
            {
                EditablePlayerWithLogs playerToAdd = new EditablePlayerWithLogs(_currentCompetition)
                {
                    DbEntity = dbPlayer
                };
                playerToAdd.DownloadTimeReads();
                _playersWithLogs.Add(playerToAdd);
            }
            return _playersWithLogs;
        }
    }
}
