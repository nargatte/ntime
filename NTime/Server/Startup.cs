using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Server.Models;

[assembly: OwinStartup(typeof(Server.Startup))]

namespace Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private string[] rolesStrings =
        {
            "Player",
            "Organizer",
            "Moderator",
            "Administrator"
        }; 

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            foreach (string s in rolesStrings)
            {
                if (!roleManager.RoleExists(s))
                {
                    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role.Name = s;
                    roleManager.Create(role);

                }
            }
 
            if (!userManager.Users.Any(u => u.UserName == "admin"))
            {      

                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin";

                string userPWD = "admin1";

                var chkUser = userManager.Create(user, userPWD);
 
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Administrator");

                }
            }
        }
    }
}
