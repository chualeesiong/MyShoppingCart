using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShoppingCart.Models;
using MyShoppingCart.DB;
using System.Diagnostics;

namespace MyShoppingCart.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(string Username, string Password)
        {
            if (Username == null)
                return View();

            Customer c1 = CustomerData.GetCustomerbyUserName(Username);

            if (c1 == null)
                return RedirectToAction("Contact", "Home");

            if(c1.Username != Username)
                return RedirectToAction("Contact", "Home");

            if (c1.Password != Password)
                return RedirectToAction("Contact", "Home");

            string sessionId = SessionData.CreateSession(c1.CustomerId);
            Debug.WriteLine(c1.CustomerId, c1.LastName);
            Session[c1.Username] = sessionId;
            Session[sessionId] = c1.Username;

            return RedirectToAction("Index","Gallery", new { sessionId });
        }
    }
}