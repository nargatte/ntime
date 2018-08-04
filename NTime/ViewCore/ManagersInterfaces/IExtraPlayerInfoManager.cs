using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ViewCore.Entities;

namespace ViewCore.ManagersInterfaces
{
    public interface ISubcategoryManager
    {
        ObservableCollection<EditableSubcategory> DefinedSubcategory { get; set; }

        Task<ObservableCollection<EditableSubcategory>> DownloadSubcategoryAsync();
    }
}