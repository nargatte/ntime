using System;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class ContextProvider : IContextProvider
    {
        public ContextProvider()
        {
        }

        public ContextProvider(string nameOrConnectionString)
        {
            NameOrConnectionString = nameOrConnectionString;
        }

        protected string NameOrConnectionString = "NTime";

        public async Task DoAsync(Func<NTimeDBContext, Task> action)
        {
            using (NTimeDBContext context = new NTimeDBContext(NameOrConnectionString))
            {
                await action(context);
            }
        }
    }
}