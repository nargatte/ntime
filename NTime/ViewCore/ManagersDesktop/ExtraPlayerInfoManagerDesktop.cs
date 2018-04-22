﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore.Entities;
using BaseCore.DataBase;
using ViewCore.ManagersInterfaces;

namespace ViewCore.ManagersDesktop
{
    public class ExtraPlayerInfoManagerDesktop : CompetitionItemBase, IExtraPlayerInfoManager
    {
        public ObservableCollection<EditableExtraPlayerInfo> DefinedExtraPlayerInfo { get; set; } = new ObservableCollection<EditableExtraPlayerInfo>();
        private SubcategoryRepository _subcategoryRepository;
        public ExtraPlayerInfoManagerDesktop(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            //_playerRepository
            _subcategoryRepository = new SubcategoryRepository(new ContextProvider(), _currentCompetition.DbEntity);
        }


        public async Task<ObservableCollection<EditableExtraPlayerInfo>> DownloadExtraPlayerInfoAsync()
        {
            var dbExtraPlayerInfos = await _subcategoryRepository.GetAllAsync();
            DefinedExtraPlayerInfo.Clear();
            foreach (var dbExtraPlayerInfo in dbExtraPlayerInfos)
            {
                DefinedExtraPlayerInfo.Add(new EditableExtraPlayerInfo(_currentCompetition)
                {
                    DbEntity = dbExtraPlayerInfo
                });
            }
            DefinedExtraPlayerInfo.Add(new EditableExtraPlayerInfo(_currentCompetition)
            {
                DbEntity = new Subcategory()
            });
            return DefinedExtraPlayerInfo;
        }
    }
}
