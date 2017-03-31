using System;
using System.Web;
using System.Web.Mvc;
using MVCClient.Models;

namespace MVCClient.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Cabinet()
        {
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(SignInViewModel model)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(SignUpViewModel model)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignOut()
        {
            Response.Cookies.Remove("token");
            HttpCookie c = new HttpCookie("token");
            c.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Response.Cookies.Add(c);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ProfileTab()
        {
            return PartialView();
        }
        public ActionResult DebtsTab()
        {
            return PartialView();
        }
        public ActionResult AbsentsTab()
        {
            return PartialView();
        }
        public ActionResult RegisterTab()
        {
            return PartialView();
        }
    }
}