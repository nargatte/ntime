using System.Security.Principal;
using System.Threading.Tasks;
using BaseCore.DataBase;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Server.Models;

namespace Server.Controllers
{
    public static class ControllerHelper
    {
        public static async Task<bool> ModeratorAcess(IPrincipal user, ContextProvider contextProvider, int competitionId)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            CompetitionRepository competitionRepository = new CompetitionRepository(contextProvider);
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = UserManager.GetRoles(user.Identity.GetUserId());
            if (s[0] == "Moderator" &&
                !await competitionRepository.CanModeratorEdit(user.Identity.GetUserId(), competitionId)) 
                return false;
            return true;
        }
    }
}