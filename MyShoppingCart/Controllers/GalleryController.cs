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
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index(string sessionId)
        {
            List<Product> products = ProductData.DisplayProducts();
            string Username = (string)Session[sessionId];
            Customer c1 = CustomerData.GetCustomerbyUserName(Username);
            Product p1 = new Product();
            ViewData["customerName"] = c1.FirstName + " " + c1.LastName;
            ViewData["sessionId"] = sessionId;
            ViewData["products"] = products;

            return View(p1);
        }
    }
}