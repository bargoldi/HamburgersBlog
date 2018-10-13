using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HamburgersBlog.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin (Login)
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            var isAdmin = username == "brr" && password == "1234";
            if (isAdmin)
            {
                FormsAuthentication.SetAuthCookie("Admin", false);

                return Json(new { succeeded = true });
            }

            return Json(new { error = "User name or password are incorrect." });
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View("~/Views/Admin/Login.cshtml");
        }
    }
}