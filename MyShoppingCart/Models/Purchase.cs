using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShoppingCart.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int QtyPurchased { get; set; }
        public string ActivationCode { get; set; }
        public int TotalPriceAmt { get; set; }
        public string DatePurchased { get; set; }
        public string CartId { get; set; }
        public string SessionId { get; set; }
    }
}