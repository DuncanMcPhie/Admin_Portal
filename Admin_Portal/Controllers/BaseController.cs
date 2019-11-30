using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Admin_Portal.Data;
using cfg = System.Configuration.ConfigurationManager;

namespace Admin_Portal.Controllers
{
    public class BaseController : Controller
    {
        protected AdminRepository arepository = new AdminRepository(cfg.AppSettings["mysqlconnection"]);
        protected LinkRepository lrepository = new LinkRepository(cfg.AppSettings["mysqlconnection"]);

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if(requestContext.HttpContext.Request.IsAuthenticated)
            {
                var currentAdmin = requestContext.HttpContext.User;
                var name = requestContext.HttpContext.User.Identity.Name;
                Admin admin = null;

                admin = arepository.GetAdmin(name);

                if(admin == null)
                {
                    admin = new Admin { AdminID = "00000", Admin_Type = "Unknown" };
                }

                admin.Attach(currentAdmin);
                HttpContext.User = admin;
            }
        }
    }
}