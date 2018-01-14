using System.Threading.Tasks;
using BaseCore.DataBase;

namespace ViewCore.ManagersInterfaces

{
    public interface IPlayerAccountManager
    {
        PlayerAccount LoggedInPlayer { get; }

        Task<PlayerAccount> DownloadPlayerTemplateData();
        Task UpdatePlayerTemplateData(PlayerAccount playerAccount);
    }
}