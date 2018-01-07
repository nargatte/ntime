using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ViewCore.Entities;

namespace ViewCore.ManagersInterfaces
{
    public interface IExtraPlayerInfosManager
    {
        ObservableCollection<EditableExtraPlayerInfo> DefinedExtraPlayerInfo { get; set; }

        Task<ObservableCollection<EditableExtraPlayerInfo>> DownloadExtraPlayerInfoAsync();
    }
}