using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVCClient.Models;
using MVCClient.Properties;
using MVCClient.Util;
using Newtonsoft.Json;

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
        public async Task<ActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = Resources.SignInError;
                return View(model);
            }
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("UserName", model.Email),
                new KeyValuePair<string, string>("Password", model.Password),
            };
            try
            {
                Dictionary<string, string> responce = (Dictionary<string, string>) (await HttpClientEngine.Token(list));
                if (responce.ContainsKey("access_token"))
                {
                    HttpContext.Response.Cookies.Add(new HttpCookie("token", responce["access_token"]));
                    HttpClientEngine.AccessToken = Request.Cookies.Get("token")?.Value;
                }
                else
                {
                    throw new HttpParseException();
                }
            }
            catch
            {
                ViewBag.Error = Resources.SignInError;
                return View(model);
            }
            string role = (string)HttpClientEngine.Get("api/Account/GetRole", typeof(string));
            HttpContext.Response.Cookies.Add(new HttpCookie("Role", role));
            return RedirectToAction("Index", "Home");
        }


        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = Resources.SignUpError;
                return View(model);
            }
            try
            {
                await HttpClientEngine.Post("api/Account/Register", model);
            }
            catch
            {
                ViewBag.Error = Resources.SignUpError;
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            HttpClientEngine.AccessToken = Request.Cookies.Get("token")?.Value;
            if (!ModelState.IsValid)
            {
                ViewBag.Error = Resources.ChangePasswordError;
                return RedirectToAction("Cabinet", "Account");
            }
            try
            {
                await HttpClientEngine.Post("api/Account/ChangePassword", model);
                ViewBag.Error = "Password successfully changed";
            }
            catch
            {
                ViewBag.Error = Resources.ChangePasswordError;
            }
            return RedirectToAction("Cabinet", "Account");
        }

        public ActionResult SignOut()
        {
            HttpClientEngine.AccessToken = "";
            Response.Cookies.Remove("token");
            HttpCookie c = new HttpCookie("token");
            c.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Response.Cookies.Add(c);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ProfileTab()
        {
            HttpClientEngine.AccessToken = Request.Cookies.Get("token")?.Value;
            var model = HttpClientEngine.Get("api/Account/GetUserInfo", typeof(UserDetailsViewModel));
            return PartialView(model);
        }
        public ActionResult DebtsTab()
        {
            HttpClientEngine.AccessToken = Request.Cookies.Get("token")?.Value;
            var model = HttpClientEngine.Get("api/Lessons/Debts", typeof(List<LessonViewModel>));
            return PartialView(model);
        }
        public ActionResult AbsentsTab()
        {
            HttpClientEngine.AccessToken = Request.Cookies.Get("token")?.Value;
            var model = HttpClientEngine.Get("api/Lessons/Absents", typeof(List<LessonViewModel>));
            return PartialView(model);
        }
        public ActionResult RegisterTab()
        {
            return PartialView();
        }

        public ActionResult GetRegister(string date)
        {
            DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", null);
            HttpClientEngine.AccessToken = Request.Cookies.Get("token")?.Value;
            var model = HttpClientEngine.Get("api/Lessons/Register", typeof(List<LessonViewModel>));
            return PartialView("RegisterTab",((List<LessonViewModel>)model).Where(x=>x.BeginigDateTime.Date==dateTime.Date).ToList());
        }

        public ActionResult GroupsPartial()
        {
            HttpClientEngine.AccessToken = Request.Cookies.Get("token")?.Value;
            var model = HttpClientEngine.Get("api/Groups/Get",typeof(List<GroupViewModel>));
            return PartialView(model);
        }
    }
}