﻿using System.Web.Mvc;
using Admin_Portal.Data;

namespace Admin_Portal.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class AdminsController : BaseController
    {
        // GET: Admins
        public ActionResult Index()
        {
            ViewBag.Message = "Your admins page.";
            ViewBag.FromSearch = false;
            var admins = repository.GetAdmins();
            return View(admins);
        }

        public ActionResult Search(string searchtype, string searchop, string searchtxt)
        {
            ViewBag.FromSearch = true;
            var admins = repository.SearchAdmins(searchtype, searchop, searchtxt);
            return View("Index", admins);
        }

        public ActionResult Edit(string Email)
        {
            return View(repository.GetAdmin(Email));
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
            repository.SaveAdmin(admin);
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
            repository.AddAdmin(admin);
            return RedirectToAction("Index");
        }
    }
}