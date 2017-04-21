using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
                }
                else
                {
                    throw new HttpParseException();
                }
            }
            catch
            {
                ViewBag.Error = Resources.SignUpError;
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpViewModel model)
        {
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

        public ActionResult GroupsPartial()
        {
            var model = HttpClientEngine.Get("api/Groups/Get",typeof(List<GroupViewModel>));
            return PartialView(model);
        }
    }
}