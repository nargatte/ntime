using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore.ManagersInterfaces;

namespace ViewCore.Factories.PlayerAccounts
{
    public interface IPlayerAccountManagerFactory
    {
        IPlayerAccountManager CreateInstance(DependencyContainer dependencyContainer);
    }
}
