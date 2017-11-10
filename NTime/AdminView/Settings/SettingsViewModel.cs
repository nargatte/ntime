using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Settings
{
    class SettingsViewModel : TabItemViewModel, IViewModel
    {
        public SettingsViewModel()
        {
            TabTitle = "Ustawienia";
        }
        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }
    }
}
