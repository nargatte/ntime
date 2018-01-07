using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using ViewCore.Entities;
using ViewCore.ManagersInterfaces;

namespace ViewCore.ManagersDesktop
{
    public class AgeCategoryManagerDesktop : CompetitionItemBase, IAgeCategoryManager
    {
        private AgeCategoryRepository _ageCategoryRepository;
        public ObservableCollection<EditableAgeCategory> DefinedAgeCategories { get; set; } = new ObservableCollection<EditableAgeCategory>();

        public AgeCategoryManagerDesktop(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            _ageCategoryRepository = new AgeCategoryRepository(new ContextProvider(), _currentCompetition.DbEntity);
        }


        public async Task<ObservableCollection<EditableAgeCategory>> DownloadAgeCategoriesAsync()
        {
            var dbAgeCategories = await _ageCategoryRepository.GetAllAsync();
            DefinedAgeCategories.Clear();
            foreach (var dbAgeCategory in dbAgeCategories)
            {
                DefinedAgeCategories.Add(new EditableAgeCategory(_currentCompetition)
                {
                    DbEntity = dbAgeCategory
                });
            }
            DefinedAgeCategories.Add(new EditableAgeCategory(_currentCompetition)
            {
                DbEntity = new AgeCategory()
            });
            return DefinedAgeCategories;
        }
    }
}
