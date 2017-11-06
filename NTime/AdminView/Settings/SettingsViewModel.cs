using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Settings
{
    class SettingsViewModel : BindableBase, IViewModel
    {
        public SettingsViewModel()
        {
            TabTitle = "Ustawienia";
        }
        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }


        private string _tabTitle;
        public string TabTitle
        {
            get { return _tabTitle; }
            set { SetProperty(ref _tabTitle, value); }
        }
    }
}
