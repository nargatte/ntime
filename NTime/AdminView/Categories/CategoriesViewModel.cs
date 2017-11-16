using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MvvmHelper;
using ViewCore;

namespace AdminView.Categories
{
    class CategoriesViewModel : TabItemViewModel
    {
        public CategoriesViewModel(ViewCore.Entities.EditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Kategorie";
            AddCategoryCmd = new RelayCommand(OnAddCategory);
        }


        private void OnAddCategory()
        {
            NewCategory.DeleteRequested += NewCategory_DeleteRequested;
            Categories.Add(NewCategory);
            NewCategory = new ViewCore.Entities.EditableCategory();

        }

        //Same must be done after import !!!!
        private void NewCategory_DeleteRequested(object sender, EventArgs e)
        {
            Categories.Remove(sender as ViewCore.Entities.EditableCategory);
        }

        private ViewCore.Entities.EditableCategory _newCategory = new ViewCore.Entities.EditableCategory();
        public ViewCore.Entities.EditableCategory NewCategory
        {
            get { return _newCategory; }
            set { SetProperty(ref _newCategory, value); }
        }

        private ObservableCollection<ViewCore.Entities.EditableCategory> _categories = new ObservableCollection<ViewCore.Entities.EditableCategory>();
        public ObservableCollection<ViewCore.Entities.EditableCategory> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        public event Action CompetitionManagerRequested = delegate { };
        public event EventHandler DeleteRequested = delegate { };
        public RelayCommand AddCategoryCmd { get; private set; }
        public RelayCommand DeleteCategoryCmd { get; private set; }
    }
}
