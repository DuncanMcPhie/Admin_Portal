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

        public ActionResult Search(string searchtype, string searchop, string searchtxt)
        {
            ViewBag.FromSearch = true;
            var links = lrepository.SearchLinks(searchtype, searchop, searchtxt);
            return View("Index", links);
        }

        public ActionResult List(IPrincipal user)
        {
            ViewBag.Redirects = true;
            var links = lrepository.ListLinks("Starts With", user);
            return View("Redirects", links);
        }

        public ActionResult Edit(int id)
        {
            return View(lrepository.GetLink(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Link link, FormCollection collection)
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