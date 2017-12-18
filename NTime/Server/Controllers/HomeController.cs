using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaseCore.DataBase;

namespace Server.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var cp = new ContextProvider();
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
