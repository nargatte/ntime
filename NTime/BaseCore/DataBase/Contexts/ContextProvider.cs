using System;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class ContextProvider : IContextProvider
    {
        public ContextProvider(string nameOrConnectionString)
        {
            NameOrConnectionString = nameOrConnectionString;
        }

        protected string NameOrConnectionString;

        public async Task DoAsync(Func<NTimeDBContext, Task> action)
        {
            using (NTimeDBContext context = new NTimeDBContext(NameOrConnectionString))
            {
                await action(context);
            }
        }
    }
}