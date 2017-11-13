using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Entities
{
    class EditableCategory : BindableBase
    {
        public EditableCategory()
        {
            DeleteCategoryCmd = new RelayCommand(OnDeleteCategory);
        }

        private BaseCore.DataBase.AgeCategory _category = new BaseCore.DataBase.AgeCategory();
        public BaseCore.DataBase.AgeCategory Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public string Name
        {
            get { return Category.Name; }
            set { Category.Name = SetProperty(Category.Name, value); }
        }


        private void OnDeleteCategory()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        public int YearFrom
        {
            get { return Category.YearFrom; }
            set { Category.YearFrom = SetProperty(Category.YearFrom, value); }
        }

        public int YearTo
        {
            get { return Category.YearTo; }
            set { Category.YearTo = SetProperty(Category.YearTo, value); }
        }

        public event EventHandler DeleteRequested = delegate { };
        public RelayCommand DeleteCategoryCmd { get; private set; }

    }
}
