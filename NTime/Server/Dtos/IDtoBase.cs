using System.Web.UI;

namespace Server.Dtos
{
    public interface IDtoBase<T>
    {
        T CopyDataFromDto(T entity);
        int Id { get; set; }
    }
}