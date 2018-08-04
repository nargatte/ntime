using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;
using BaseCore.PlayerFilter;
using ViewCore.Entities;
using System.Windows;
using System.Collections;
using System.ComponentModel;
using BaseCore.TimesProcess;
using ViewCore.ManagersDesktop;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.Subcategories;
using ViewCore.Factories.Players;
using ViewCore.Factories;

namespace AdminView.Players
{
    class PlayersViewModel : PlayersViewModelBase
    {

        public PlayersViewModel(IEditableCompetition currentCompetition, DependencyContainer dependencyContainer) : base(currentCompetition, dependencyContainer)
        {
            TabTitle = "Zawodnicy";
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            AddPlayerCmd = new RelayCommand(OnAddPlayerAsync);
            DeleteSelectedPlayersCmd = new RelayCommand(OnDeleteSelectedPlayersRequestedAsync);
            DeleteAllPlayersCmd = new RelayCommand(OnDeleteAllPlayersRequestedAsync);
            ReadPlayersFromCsvCmd = new RelayCommand(OnReadPlayersFromCsvAsync);
        }

        #region Properties

        private IList _selectedPlayersList = new ArrayList();
        public IList SelectedPlayersList
        {
            get { return _selectedPlayersList; }
            set { SetProperty(ref _selectedPlayersList, value); }
        }


        private EditablePlayer _newPlayer;
        public EditablePlayer NewPlayer
        {
            get { return _newPlayer; }
            set { SetProperty(ref _newPlayer, value); }
        }

        #endregion

        #region Methods and Events

        public RelayCommand ViewLoadedCmd { get; private set; }
        public RelayCommand AddPlayerCmd { get; private set; }
        public RelayCommand DeleteSelectedPlayersCmd { get; set; }
        public RelayCommand DeleteAllPlayersCmd { get; set; }
        public RelayCommand ReadPlayersFromCsvCmd { get; set; }


        private async void OnViewLoadedAsync()
        {
            await DownLoadPlayersFromDatabaseAndDisplay();
            ClearNewPlayer();
        }


        private async void OnAddPlayerAsync()
        {
            if (await _playersManager.TryAddPlayerAsync(NewPlayer))
            {
                Players = _playersManager.GetPlayersToDisplay();
                UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
                ClearNewPlayer();
            }
        }

        private void ClearNewPlayer()
        {
            NewPlayer = new EditablePlayer(_currentCompetition, DefinedDistances, DefinedSubcategory, new Player()
            {
                Distance = new Distance(),
                Subcategory = new Subcategory(),
                StartTime = DateTime.Today,
                BirthDate = DateTime.Today
            });
        }

        private async void OnReadPlayersFromCsvAsync()
        {
            try
            {
                await _playersManager.AddPlayersFromCsvToDatabase();
                Players = _playersManager.GetPlayersToDisplay();
                UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie udało się załadować pliku {ex.Message}");
            }
        }

        private async void OnDeleteAllPlayersRequestedAsync()
        {
            MessageBoxResult result = MessageBox.Show(
                $"Czy na pewno chcesz usunąć wszystkich zawodników?",
                $"",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await _playersManager.DeleteAllPlayersFromDatabaseAsync();
                Players = _playersManager.GetPlayersToDisplay();
                UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
                MessageBox.Show("Wszyscy zawodnicy zostali usunięci");
            }
            else return;
        }

        private void OnDeleteSelectedPlayersRequestedAsync()
        {
            MessageBoxResult result = MessageBox.Show(
             $"Czy na pewno chcesz usunąć {SelectedPlayersList.Count} zawodników?",
                $"",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectedPlayersArray = SelectedPlayersList.Cast<EditablePlayer>().ToArray();
                _playersManager.DeleteSelectedPlayersFromDatabaseAsync(selectedPlayersArray);
                DeleteSelectedPlayersFromGUI(selectedPlayersArray);
                UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
                MessageBox.Show("Wybrani zawodnicy zostali usunięci");
            }
            else return;
        }

        private void DeleteSelectedPlayersFromGUI(ICollection<EditablePlayer> selectedPlayersList)
        {
            foreach (var player in selectedPlayersList)
            {
                Players.Remove(player);
            }
        }

        #endregion
    }
}
