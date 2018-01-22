using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Controllers;
using Server.Models;

namespace ServerTests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {

        //[TestMethod]
        //public async Task Register()
        //{
        //    RegisterBindingModel bindingModel = new RegisterBindingModel
        //    {
        //        ConfirmPassword = "Pssword1",
        //        Password = "Password1",
        //        Email = "jannowak@gmail.com"
        //    };

        //    ApplicationDbContext context = new ApplicationDbContext();
        //    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

        //    AccountController _accountController = new AccountController();

        //    var result = await _accountController.Register(bindingModel);
        //    Assert.IsNotNull(result);
        //}
    }
}
