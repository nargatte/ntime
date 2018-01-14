using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore.ManagersHttp;
using ViewCore.ManagersInterfaces;

namespace ViewCore.Factories.PlayerAccounts
{
    public class PlayerAccountManagerFactoryHttp : IPlayerAccountManagerFactory
    {
        public IPlayerAccountManager CreateInstance(DependencyContainer dependencyContainer)
        {
            return new PlayerAccountManagerHttp(dependencyContainer);
        }
    }
}
