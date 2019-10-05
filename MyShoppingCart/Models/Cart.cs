using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShoppingCart.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string SessionId { get; set; }
        public int ProductId { get; set; }
        public int QtyInCart { get; set; }
    }
}