using System;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public interface IContextProvider
    {
        Task DoAsync(Func<NTimeDBContext, Task> action);
    }
}