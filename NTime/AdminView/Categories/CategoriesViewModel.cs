using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AdminView.Categories
{
    class CategoriesViewModel : TabItemViewModel
    {
        public CategoriesViewModel()
        {
            TabTitle = "Kategorie";
            AddCategoryCmd = new RelayCommand(OnAddCategory);
        }

        private void OnAddCategory()
        {
            Categories.Add(NewCategory);
            NewCategory = new Entities.EditableCategory();
        }

        private Entities.EditableCategory _newCategory = new Entities.EditableCategory();
        public Entities.EditableCategory NewCategory
        {
            get { return _newCategory; }
            set { SetProperty(ref _newCategory, value); }
        }

        private ObservableCollection<Entities.EditableCategory> _categories = new ObservableCollection<Entities.EditableCategory>();
        public ObservableCollection<Entities.EditableCategory> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        public event Action CompetitionManagerRequested = delegate { };
        public RelayCommand AddCategoryCmd { get; private set; }
    }
}
