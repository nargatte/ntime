using System.Web.UI;

namespace Server.Dtos
{
    public interface IDtoBase<T>
    {
        T CopyDataFromDto(T entiti);
        int Id { get; set; }
    }
}