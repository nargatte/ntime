using System;
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
    public class SubcategoryManagerDesktop : CompetitionItemBase, ISubcategoryManager
    {
        public ObservableCollection<EditableSubcategory> DefinedSubcategory { get; set; } = new ObservableCollection<EditableSubcategory>();
        private SubcategoryRepository _subcategoryRepository;
        public SubcategoryManagerDesktop(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            //_playerRepository
            _subcategoryRepository = new SubcategoryRepository(new ContextProvider(), _currentCompetition.DbEntity);
        }


        public async Task<ObservableCollection<EditableSubcategory>> DownloadSubcategoryAsync()
        {
            var dbSubcategories = await _subcategoryRepository.GetAllAsync();
            DefinedSubcategory.Clear();
            foreach (var dbSubcategory in dbSubcategories)
            {
                DefinedSubcategory.Add(new EditableSubcategory(_currentCompetition)
                {
                    DbEntity = dbSubcategory
                });
            }
            DefinedSubcategory.Add(new EditableSubcategory(_currentCompetition)
            {
                DbEntity = new Subcategory()
            });
            return DefinedSubcategory;
        }
    }
}
