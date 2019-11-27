using System.Web.Mvc;
using Admin_Portal.Data;

namespace Admin_Portal.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class UsersController : BaseController
    {
        // GET: Users
        public ActionResult Index()
        {
            ViewBag.Message = "Your users page.";
            ViewBag.FromSearch = false;
            var users = repository.GetUsers();
            return View(users);
        }

        public ActionResult Search(string searchtype, string searchop, string searchtxt)
        {
            ViewBag.FromSearch = true;
            var users = repository.SearchUsers(searchtype, searchop, searchtxt);
            return View("Index", users);
        }

        public ActionResult Edit(string Email)
        {
            return View(repository.GetUser(Email));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user, FormCollection collection)
        {
            var password = collection["NewPassword"];
            if(!string.IsNullOrEmpty(password))
            {
                user.Password = password;
            }
            repository.SaveUser(user);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            repository.AddUser(user);
            return RedirectToAction("Index");
        }
    }
}