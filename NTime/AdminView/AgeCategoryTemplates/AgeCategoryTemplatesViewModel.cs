using MvvmHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore.Entities;

namespace AdminView.AgeCategoryTemplates 
{
    public class AgeCategoryTemplatesViewModel : BindableBase
    {




        private ObservableCollection<EditableAgeCategoryCollection> _ageCategoryCollections = new ObservableCollection<EditableAgeCategoryCollection>();
        public ObservableCollection<EditableAgeCategoryCollection> AgeCategoryCollections
        {
            get { return _ageCategoryCollections; }
            set { SetProperty(ref _ageCategoryCollections, value); }
        }


        private EditableAgeCategoryCollection _selectedAgeCategoryCollection;
        public EditableAgeCategoryCollection SelectedAgeCategoryCollection
        {
            get { return _selectedAgeCategoryCollection; }
            set { SetProperty(ref _selectedAgeCategoryCollection, value); }
        }
    }
}
