using System.Web.Mvc;
using Admin_Portal.Data;
using System.Security.Principal;

namespace Admin_Portal.Controllers
{
    [Authorize(Roles = "Super")]
    public class LinksController : BaseController
    {
        // GET: Links
        public ActionResult Index()
        {
            ViewBag.Message = "Your links page.";
            ViewBag.FromSearch = false;
            var links = lrepository.GetLink();
            return View(links);
        }

        public ActionResult linkAccess()
        {
            var admintype = (User as Admin_Portal.Data.Admin).Admin_Type;
            ViewBag.Message = "Your links page.";
            var links = lrepository.ListLinks(admintype);
            return View(links);
        }

        public ActionResult Search(string searchtype, string searchop, string searchtxt)
        {
            ViewBag.FromSearch = true;
            var links = lrepository.SearchLinks(searchtype, searchop, searchtxt);
            return View("Index", links);
        }

        public ActionResult Edit(int id)
        {
            return View(lrepository.GetLink(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Link link)
        {
            lrepository.SaveLink(link);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View(new Link());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Link link)
        {
            lrepository.AddLink(link);
            return RedirectToAction("Index");
        }
    }
}