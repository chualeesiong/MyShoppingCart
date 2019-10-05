using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShoppingCart.DB;

namespace MyShoppingCart.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index(string sessionId)
        {
            SessionData.RemoveSession(sessionId);
            return View();
        }
    }
}