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
            var user = repository.GetUser(login.Email);
            if (user == null || String.IsNullOrEmpty(user.Email))
            {
                // invalid username
                ModelState.AddModelError("UserName", "Invalid UserName or password.");
                ModelState.AddModelError("Password", "Invalid UserName or password.");
            }
            else if (!user.CheckLogin(login))
            {
                // invalid password
                ModelState.AddModelError("Password", "Invalid UserName or password.");
                ModelState.AddModelError("UserName", "Invalid UserName or password.");
            }
            else // this is a good login
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(20), true, "some json data here");
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
            }

            // _userRepo.UpdateItem(user.Id.ToString(), user); // keep track of logins

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