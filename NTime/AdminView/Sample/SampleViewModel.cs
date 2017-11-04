using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupWPF.Sample
{
    class SampleViewModel : BindableBase
    {
        public RelayCommand SampleCommand { get; private set; }

        public event Action SampleEvent = delegate { };
    }
}
