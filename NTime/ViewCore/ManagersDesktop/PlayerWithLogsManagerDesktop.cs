using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore.Entities;

namespace ViewCore.ManagersDesktop
{
    public class PlayerWithLogsManagerDesktop : CompetitionItemBase
    {
        private PlayerRepository _playerRepository;
        private ObservableCollection<EditablePlayerWithLogs> _playersWithLogs;

        public PlayerWithLogsManagerDesktop(IEditableCompetition currentComptetition, PlayerRepository playerRepository) : base(currentComptetition)
        {
            _playerRepository = playerRepository;
        }

        public async Task<ObservableCollection<EditablePlayerWithLogs>> GetAllPlayers(bool onlySignificant = false)
        {
            _playersWithLogs = new ObservableCollection<EditablePlayerWithLogs>();
            var dbPlayers = await _playerRepository.GetAllAsync();
            foreach (var dbPlayer in dbPlayers)
            {
                EditablePlayerWithLogs playerToAdd = new EditablePlayerWithLogs(_currentCompetition)
                {
                    DbEntity = dbPlayer
                };
                playerToAdd.DownloadTimeReads(onlySignificant);
                _playersWithLogs.Add(playerToAdd);
            }
            return _playersWithLogs;
        }
    }
}