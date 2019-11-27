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
        protected AdminRepository repository = new AdminRepository(cfg.AppSettings["mysqlconnection"]);

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if(requestContext.HttpContext.Request.IsAuthenticated)
            {
                var currentUser = requestContext.HttpContext.User;
                var name = requestContext.HttpContext.User.Identity.Name;
                User user = null;

                user = repository.GetUser(name);

                if(user == null)
                {
                    user = new User { UserID = 00000, User_Type = "Unknown" };
                }

                user.Attach(currentUser);
                HttpContext.User = user;
            }
        }
    }
}