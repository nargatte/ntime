using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ViewCore.Entities;

namespace ViewCore.ManagersInterfaces
{
    public interface IAgeCategoryManager
    {
        ObservableCollection<EditableAgeCategory> DefinedAgeCategories { get; set; }

        Task<ObservableCollection<EditableAgeCategory>> DownloadAgeCategoriesAsync();
    }
}