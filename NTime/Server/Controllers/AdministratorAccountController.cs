using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Server.Models;

namespace Server.Controllers
{
    [RoutePrefix("api/AdministratorAccount")]
    public class AdministratorAccountController : ControllerNTimeBase
    {
        private ApplicationDbContext _applicationDBContext = new ApplicationDbContext();
        private UserManager<ApplicationUser> _userManager;
        private new RoleManager<IdentityRole> _roleManager;

        protected AdministratorAccountController()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_applicationDBContext));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_applicationDBContext));
        }

        // GET api/AdministratorAccount
        [Route]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult Get()
        {
            List<string> roles = _roleManager.Roles.Where(r => r.Name == "Administrator").Select(r => r.Id).ToList();
            ApplicationUser[] applicationUsers =
                _userManager.Users.Where(u => u.Roles.Any(r => roles.Contains(r.RoleId))).ToArray();

            UserInfoViewModel[] infoViewModels = applicationUsers.Select(u => new UserInfoViewModel
            {
                Id = u.Id,
                Email = u.Email,
                Status = _userManager.GetRoles(u.Id)[0]
            }).ToArray();
            return Ok(infoViewModels);
        }

        // POST api/AdministratorAccount
        [Route]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Post(RegisterBindingModel accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = accountDto.Email, Email = accountDto.Email };

            IdentityResult result = await _userManager.CreateAsync(user, accountDto.Password);
            await _userManager.AddToRoleAsync(user.Id, "Administrator");

            if (!result.Succeeded)
            {
                return InternalServerError();
            }

            return Ok();
        }

        // DELETE api/AdministratorAccount/aaa
        [Route("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            if (user.Id == User.Identity.GetUserId())
                return BadRequest();

            await _userManager.DeleteAsync(user);

            return Ok();
        }

        // POST api/AdministratorAccount/aaa/PasswordReset
        [Route("{id}/PasswordReset")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> PostPasswordReset(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            if (user.Id == User.Identity.GetUserId())
                return BadRequest();

            string password = PasswordGenerator();


            IdentityResult result = await _userManager.RemovePasswordAsync(user.Id);
            IdentityResult result2 = await _userManager.AddPasswordAsync(user.Id, password);

            if (!result.Succeeded && !result2.Succeeded)
            {
                return InternalServerError();
            }

            return Ok(new PasswordViewModel { Password = password });
        }
    }
}