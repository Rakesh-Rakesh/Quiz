using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizApplication.ViewModels;
using QuizApplication.Models;
using System.Web.Security;

namespace QuizApplication.Controllers
{
    public class AccountController : Controller
    {
        QuizDBEntities objQuiz = new QuizDBEntities();
        // GET: Account
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(AdminViewModel objAdminViewModel)
        {
            if (ModelState.IsValid)
            {
                Admin admin = objQuiz.Admins.SingleOrDefault(model => model.UserName == objAdminViewModel.UserName);
                if (admin == null)
                {
                    ModelState.AddModelError(string.Empty, "Email Not Exist");
                }
                else if (admin.UserPassword != objAdminViewModel.UserPassword)
                {
                    ModelState.AddModelError(string.Empty, "Email & Passord  is Invalid");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(objAdminViewModel.UserName, false);
                    var authTicket = new FormsAuthenticationTicket(1, objAdminViewModel.UserName, DateTime.Now, DateTime.Now.AddMinutes(20),
                        false,"Admin");
                    string encryptTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket);
                    HttpContext.Response.Cookies.Add(authCookie);
                    return RedirectToAction("Index", "Admin"); 
                }
            }
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}