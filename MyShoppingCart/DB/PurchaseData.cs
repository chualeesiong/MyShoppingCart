using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using MyShoppingCart.Models;
using MyShoppingCart.DB;

namespace MyShoppingCart.DB
{
    public class PurchaseData
    {
        public static string GetDateofPurchase(int productId, int customerId)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            string date = "";

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = @"Select DatePurchased From Purchase Where ProductId =@ProductId and CustomerId=@CustomerId";
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        date = (string)reader["DatePurchased"];
                    }
                }
            }
            return date;
        }

        public static List<string> GetActivationCode(int customerId, int productId)
        {
            List<string> listCodes = new List<string>();
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = @"Select ActivationCode From Purchase Where ProductId =@ProductId and CustomerId=@CustomerId";
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string ac = (string)reader["ActivationCode"];
                        listCodes.Add(ac);
                    }
                }
            }
            return listCodes;
        }
        public static int GetTotalQuantity(int customerId, int productId)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            int quantity = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = @"Select Count(*) From Purchase Where ProductId=@ProductId and CustomerId=@CustomerId";
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                quantity = (int)cmd.ExecuteScalar();
            }
            return quantity;
        }
        public static List<Product> GetProductHistory(int customerId)
        {
            List<Product> purchaseHistory = new List<Product>();

            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = @"Select distinct ProductId from Purchase where CustomerId = @CustomerId";
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product po1 = new Product()
                        {                         
                            ProductId = (int)reader["ProductId"],
                            ProductName = ProductData.GetProductById((int)reader["ProductId"]).ProductName,
                            ProductDesc = ProductData.GetProductById((int)reader["ProductId"]).ProductDesc,
                        };
                        purchaseHistory.Add(po1);
                    }
                }
            }
            return purchaseHistory;
        }
        public static List<Purchase> GetPurchaseHistory(int customerId)
        {
            List<Purchase> purchaseHistory = new List<Purchase>();

            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = @"Select PurchaseId, ProductId, ActivationCode, DatePurchased from Purchase where CustomerId = @CustomerId";
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Purchase po1 = new Purchase()
                        {
                            PurchaseId = (int)reader["PurchaseId"],
                            ProductId = (int)reader["ProductId"],
                            ActivationCode = (string)reader["ActivationCode"],
                            DatePurchased = (string)reader["DatePurchased"],
                            CustomerId = customerId
                        };
                        purchaseHistory.Add(po1);
                    }
                }
            }
            return purchaseHistory;
        }
        public static void CheckoutCart(string sessionId)
        {
            List<Cart> carts = new List<Cart>();

            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = @"Select CartId, SessionId, ProductId,QtyInCart from Cart WHERE SessionId=@SessionId";
                cmd.Parameters.AddWithValue("@SessionId", sessionId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Cart carty = new Cart()
                        {
                            CartId = (int)reader["CartId"],
                            SessionId = (string)reader["SessionId"],
                            ProductId = (int)reader["ProductId"],
                            QtyInCart = (int)reader["QtyInCart"]
                        };
                        carts.Add(carty);
                    }
                    
                }

                foreach (Cart carty in carts)
                {
                        
                        cmd.CommandText = @"Select Count(*) from Purchase";
                        int PurchaseCount = (int)cmd.ExecuteScalar();
                        PurchaseCount++;

                        string activationCode = Guid.NewGuid().ToString();
                        int customerId = CustomerData.GetCustomerbySessionId(carty.SessionId).CustomerId;

                        cmd.CommandText = @"Insert into Purchase Values(@PurchaseId2, @ProductId2,@QtyPurchased2,@ActivationCode2, 
                                    @DatePurchased2,@SessionId2,@CustomerId2)";
                        cmd.Parameters.AddWithValue("@PurchaseId2", PurchaseCount);
                        cmd.Parameters.AddWithValue("@ProductId2", carty.ProductId);
                        cmd.Parameters.AddWithValue("@QtyPurchased2", carty.QtyInCart);
                        cmd.Parameters.AddWithValue("@ActivationCode2", activationCode);
                        cmd.Parameters.AddWithValue("@DatePurchased2", DateTime.Now.ToString("dd MMM yyyy"));
                        cmd.Parameters.AddWithValue("@SessionId2", carty.SessionId);
                        cmd.Parameters.AddWithValue("@CustomerId2", customerId);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"Delete From Cart where SessionId=@SessionId3";
                        cmd.Parameters.AddWithValue("@SessionId3", sessionId);
                        cmd.ExecuteNonQuery();

                        cmd.Parameters.Clear();
                }
            }
        }
    }
}