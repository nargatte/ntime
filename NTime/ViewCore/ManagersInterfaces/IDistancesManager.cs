using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ViewCore.Entities;

namespace ViewCore.ManagersInterfaces
{
    public interface IDistancesManager
    {
        ObservableCollection<EditableDistance> DefinedDistances { get; set; }

        Task<ObservableCollection<EditableDistance>> DownloadDistancesAsync();
    }
}