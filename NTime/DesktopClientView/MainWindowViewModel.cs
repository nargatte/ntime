using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopClientView.CompetitionManager;
using MvvmHelper;
using ViewCore.Entities;

namespace DesktopClientView
{
    internal class MainWindowViewModel : BindableBase
    {
        private ISwitchableViewModel _currentViewModel;

        public MainWindowViewModel()
        {
            CurrentViewModel = new CompetitionManagerViewModel();
        }


        public ISwitchableViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }
    }
}
