using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShoppingCart.DB;
using MyShoppingCart.Models;

namespace MyShoppingCart.Controllers
{
    public class PurchasesController : Controller
    {
        // GET: Purchases
        public ActionResult Index(string sessionId)
        {
            PurchaseData.CheckoutCart(sessionId);
            int customerId = CustomerData.GetCustomerbySessionId(sessionId).CustomerId;

            List<Product> productHistory = PurchaseData.GetProductHistory(customerId);
            
            ViewData["ProductHistory"] = productHistory;
            ViewData["sessionId"] = sessionId;
            ViewData["CustomerId"] = customerId;

            Product p1 = new Product();

            return View(p1);
        }
    }
}