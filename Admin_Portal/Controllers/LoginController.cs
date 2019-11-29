using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Admin_Portal.Data;

namespace Admin_Portal.Controllers
{
    public class LoginController : BaseController
    {

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Auth(LoginAttempt login)
        {
            var admin = repository.GetAdmin(login.Email);
            if (admin == null || String.IsNullOrEmpty(admin.Email))
            {
                // invalid username
                ModelState.AddModelError("UserName", "Invalid UserName or password.");
                ModelState.AddModelError("Password", "Invalid UserName or password.");
            }
            else if (!admin.CheckLogin(login))
            {
                // invalid password
                ModelState.AddModelError("Password", "Invalid UserName or password.");
                ModelState.AddModelError("UserName", "Invalid UserName or password.");
            }
            else // this is a good login
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, admin.Email, DateTime.Now, DateTime.Now.AddMinutes(20), true, "some json data here");
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("Index");
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}