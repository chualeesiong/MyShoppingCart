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
    public class CartController : Controller
    {
        // GET: Cart

        public ActionResult AddtoCart(string sessionId,int productId)
        {
            CartData.AddToCart(sessionId, productId); //add to cart

            return RedirectToAction("Index", "Gallery", new { sessionId });
        }
        public ActionResult Index(string sessionId)
        {                      
            List<Product> productsInCart = CartData.GetProductsInCart(sessionId);
            Product p1 = new Product();

            ViewData["ProductList"] = productsInCart;
            ViewData["sessionId"] = sessionId;

            return View(p1);
        }
    }
}