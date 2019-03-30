using BaseCore.DataBase;
using MvvmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public class EditableExtraColumn : EditableCompetitionItemBase<ExtraColumn>
    {
        public EditableExtraColumn(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            DeleteExtraHeaderCmd = new RelayCommand(OnDeleteExtraHeader);
            MoveUpExtraHeaderCmd = new RelayCommand(OnMoveUpRequested);
            MoveDownExtraHeaderCmd = new RelayCommand(OnMoveDownRequested);
        }

        #region Properties

        public int Id
        {
            get { return DbEntity.Id; }
            set { DbEntity.Id = SetProperty(DbEntity.Id, value); }
        }

        public int CompetitionId
        {
            get { return DbEntity.CompetitionId; }
            set { DbEntity.CompetitionId = SetProperty(DbEntity.CompetitionId, value); }
        }

        public string Title
        {
            get { return DbEntity.Title; }
            set { DbEntity.Title = SetProperty(DbEntity.Title, value); }
        }

        public bool IsRequired
        {
            get { return DbEntity.IsRequired; }
            set { DbEntity.IsRequired = SetProperty(DbEntity.IsRequired, value); }
        }

        public bool IsDisplayedInPublicList
        {
            get { return DbEntity.IsDisplayedInPublicList; }
            set { DbEntity.IsDisplayedInPublicList = SetProperty(DbEntity.IsDisplayedInPublicList, value); }
        }

        public bool IsDisplayedInPublicDetails
        {
            get { return DbEntity.IsDisplayedInPublicDetails; }
            set { DbEntity.IsDisplayedInPublicDetails = SetProperty(DbEntity.IsDisplayedInPublicDetails, value); }
        }

        public string Type
        {
            get { return DbEntity.Type; }
            set { DbEntity.Type = SetProperty(DbEntity.Type, value); }
        }

        public int? SortIndex
        {
            get { return DbEntity.SortIndex; }
            set { DbEntity.SortIndex = SetProperty(DbEntity.SortIndex, value); }
        }

        public int? MultiValueCount
        {
            get { return DbEntity.MultiValueCount; }
            set { DbEntity.MultiValueCount = SetProperty(DbEntity.MultiValueCount, value); }
        }

        public int? MinCharactersValidation
        {
            get { return DbEntity.MinCharactersValidation; }
            set { DbEntity.MinCharactersValidation = SetProperty(DbEntity.MinCharactersValidation, value); }
        }

        public int? MaxCharactersValidation
        {
            get { return DbEntity.MaxCharactersValidation; }
            set { DbEntity.MaxCharactersValidation = SetProperty(DbEntity.MaxCharactersValidation, value); }
        }
        #endregion

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
