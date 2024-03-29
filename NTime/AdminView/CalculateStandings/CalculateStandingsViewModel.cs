﻿using BaseCore.Csv;
using BaseCore.Csv.CompetitionSeries;
using MvvmHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using ViewCore.Entities;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.MessageBox;

namespace AdminView.CalculateStandings
{
    public class CalculateStandingsViewModel : BindableBase
    {
        private Window _view;
        private Dictionary<int, string> _competitionNamesDict = new Dictionary<int, string>();
        private List<string> _competitionsPaths = new List<string>();

        public CalculateStandingsViewModel()
        {
            CalculateStandingsCmd = new RelayCommand(OnCalculateStandings, CanCalculateStandings);
            CancelCmd = new RelayCommand(OnCancel);
            AddFilesCmd = new RelayCommand(OnAddFiles);
        }

        #region Properties

        public ObservableCollection<EditableCompetitionFile> CompetitionFiles { get; set; } = new ObservableCollection<EditableCompetitionFile>();


        private EditableSeriesStandingsParameters _seriesStandingsParameters = new EditableSeriesStandingsParameters();
        public EditableSeriesStandingsParameters SeriesStandingsParameters
        {
            get { return _seriesStandingsParameters; }
            set { SetProperty(ref _seriesStandingsParameters, value); }
        }

        #endregion

        public void ShowWindowDialog()
        {
            _view = new CalculateStandingsView() { DataContext = this };
            _view.ShowDialog();
        }


        private void OnAddFiles()
        {

            this._competitionsPaths = new List<string>();
            //competitionPaths.AddRange(resourceLoader.LoadFilesToTemp(filesDictionary.Skip(1).ToDictionary(x => x.Key, x => x.Value), Path.GetTempPath()));
            var dialog = new OpenFileDialog
            {
                Filter = "csv files (*.csv)|*.csv",
                RestoreDirectory = true,
                Title = "Wybierz w dobrej kolejności wyniki dla cyklu",
                Multiselect = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var fileName in dialog.FileNames)
                {
                    var competitionFile = new EditableCompetitionFile { FullPath = fileName };
                    competitionFile.Title = competitionFile.FileName.Split('.').First();
                    competitionFile.DeleteRequested += CompetitionFile_DeleteRequested;
                    competitionFile.MoveUpRequested += CompetitionFile_MoveUpRequested;
                    competitionFile.MoveDownRequested += CompetitionFile_MoveDownRequested;
                    CompetitionFiles.Add(competitionFile);
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano żadnych zawodów");
            }
        }


        private async void OnCalculateStandings()
        {
            var competitionNames = CompetitionFiles.Select(file => file.Title).ToList();
            this._competitionNamesDict = new Dictionary<int, string>();
            int iter = 0;
            competitionNames.ForEach(name => _competitionNamesDict.Add(iter++, name));

            this._competitionsPaths = CompetitionFiles.Select(file => file.FullPath).ToList();
            if (_competitionsPaths == null || _competitionsPaths.Count == 0)
            {
                MessageBox.Show("Nie podano żadnych ścieżek do plików z zawodami");
                return;
            }

            if (_competitionNamesDict == null || _competitionNamesDict.Count == 0)
            {
                MessageBox.Show("Nie podano nazw dla zawodów");
                return;
            }

            var resourceLoader = new ResourceLoader();
            var filesDictionary = new Dictionary<string, string>
            {
                { "klasyki_punkty.csv", Properties.Resources.klasyki_punkty }
            };

            //try
            //{
                var seriesStandings = new SeriesStandings(_competitionNamesDict, SeriesStandingsParameters.DbEntity, _competitionsPaths);

                var pointsPath = resourceLoader.LoadFilesToTemp(filesDictionary.Take(1).ToDictionary(x => x.Key, x => x.Value), Path.GetTempPath());

                bool calculatedAndExportedCorrectly = await seriesStandings.CalculateResults(pointsPath.First());
                if (calculatedAndExportedCorrectly)
                    MessageBox.Show("Wyniki rankingu zostały przeliczone");
                else
                    MessageBox.Show("Błąd. Nie udało się przeliczyć wyników rankingu");
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Nie udało sie przeliczyć wyników. Błąd: " + e.Message);
            //    Application.Restart();
            //}
        }


        private void CompetitionFile_DeleteRequested(object sender, EventArgs e)
        {
            var competitionFile = sender as EditableCompetitionFile;
            CompetitionFiles.Remove(competitionFile);
        }

        private void CompetitionFile_MoveUpRequested(object sender, EventArgs e)
        {
            var competitionFile = sender as EditableCompetitionFile;
            if (competitionFile == CompetitionFiles.First())
                return;
            int currentIndex = CompetitionFiles.IndexOf(competitionFile);
            CompetitionFiles.Move(currentIndex, currentIndex - 1);
        }

        private void CompetitionFile_MoveDownRequested(object sender, EventArgs e)
        {
            var competitionFile = sender as EditableCompetitionFile;
            if (competitionFile == CompetitionFiles.Last())
                return;
            int currentIndex = CompetitionFiles.IndexOf(competitionFile);
            CompetitionFiles.Move(currentIndex, currentIndex + 1);
        }

        private bool CanCalculateStandings()
        {
            return true;
        }

        private void OnCancel()
        {
            _view.Close();
        }

        public RelayCommand CalculateStandingsCmd { get; private set; }
        public RelayCommand AddFilesCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
    }

}
