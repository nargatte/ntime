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
    public class PlayersWithLogsManager : CompetitionItemBase
    {
        private PlayerRepository _playerRepository;
        private ObservableCollection<EditablePlayerWithLogs> _playersWithLogs;

        public PlayersWithLogsManager(IEditableCompetition currentComptetition, PlayerRepository playerRepository) : base(currentComptetition)
        {
            _playerRepository = playerRepository;
        }

        public Task<ObservableCollection<EditablePlayerWithLogs>> GetAllPlayers()
        {
            throw new NotImplementedException();
        }
    }
}
