using System.Web.Mvc;
using Admin_Portal.Data;

namespace Admin_Portal.Controllers
{
    [Authorize(Roles = "Super")]
    public class AdminsController : BaseController
    {
        // GET: Admins
        public ActionResult Index()
        {
            ViewBag.Message = "Your admins page.";
            ViewBag.FromSearch = false;
            var admins = arepository.GetAdmins();
            return View(admins);
        }

        public ActionResult Search(string searchtype, string searchop, string searchtxt)
        {
            ViewBag.FromSearch = true;
            var admins = arepository.SearchAdmins(searchtype, searchop, searchtxt);
            return View("Index", admins);
        }

        public ActionResult Edit(int id)
        {
            return View(arepository.GetAdmin(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin admin, FormCollection collection)
        {
            var password = collection["NewPassword"];
            if(!string.IsNullOrEmpty(password))
            {
                admin.Password = password;
            }
            arepository.SaveAdmin(admin);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View(new Admin());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin admin)
        {
            arepository.AddAdmin(admin);
            return RedirectToAction("Index");
        }

        public ActionResult Signup()
        {
            return View(new Admin());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(Admin admin)
        {
            arepository.AddTempAdmin(admin);
            return RedirectToAction("Index");
        }

    }
}