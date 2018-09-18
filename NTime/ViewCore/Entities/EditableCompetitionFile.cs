using MvvmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries
{
    public class EditableCompetitionFile : BindableBase
    {
        private List<char> slashes = new List<char> { '/', '\\' };

        public EditableCompetitionFile()
        {
            DeleteExtraHeaderCmd = new RelayCommand(OnDeleteExtraHeader);
            MoveUpExtraHeaderCmd = new RelayCommand(OnMoveUpRequested);
            MoveDownExtraHeaderCmd = new RelayCommand(OnMoveDownRequested);
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string FileName => FullPath.Split(this.slashes.ToArray()).Last();

        private string _fullPath;
        public string FullPath
        {
            get { return _fullPath; }
            set { SetProperty(ref _fullPath, value); }
        }

        public RelayCommand DeleteExtraHeaderCmd { get; private set; }
        public RelayCommand MoveUpExtraHeaderCmd { get; private set; }
        public RelayCommand MoveDownExtraHeaderCmd { get; private set; }
        public event EventHandler DeleteRequested = delegate { };
        public event EventHandler MoveUpRequested = delegate { };
        public event EventHandler MoveDownRequested = delegate { };

        private void OnDeleteExtraHeader()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        protected void OnMoveUpRequested()
        {
            MoveUpRequested?.Invoke(this, EventArgs.Empty);
        }

        protected void OnMoveDownRequested()
        {
            MoveDownRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
