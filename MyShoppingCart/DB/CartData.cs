using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using MyShoppingCart.Models;

namespace MyShoppingCart.DB
{
    public class CartData
    {
        //view cart method
        public static List<Product> GetProductsInCart(string sessionId)
        {
            List<Product> productsInCart = new List<Product>();

            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = @"Select ProductId, SessionId from Cart group by ProductId, SessionId Having SessionId= @SessionId";
                cmd.Parameters.AddWithValue("@SessionId", sessionId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product p1 = new Product()
                        {
                            ProductId = (int)reader["ProductId"],
                            ProductName = ProductData.GetProductById((int)reader["ProductId"]).ProductName,
                            ProductDesc = ProductData.GetProductById((int)reader["ProductId"]).ProductDesc,
                            ProductPrice = ProductData.GetProductById((int)reader["ProductId"]).ProductPrice,

                        };
                        productsInCart.Add(p1);
                    }
                }
                return productsInCart;
            }
        }
        public static void AddToCart(string sessionId, int productId)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = @"Select Count(*) from Cart";
                int CartCount = (int)cmd.ExecuteScalar();
                CartCount++;

                cmd.CommandText = @"Insert Into Cart Values(@CartId,@SessionId,@ProductId,'1')";
                cmd.Parameters.AddWithValue("@CartId", CartCount);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.ExecuteNonQuery();
            }
        }

        public static int GetTotalQuantity(string sessionId, int productId)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            int quantity = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = @"Select Count(*) From Cart Where ProductId=@ProductId and SessionId=@SessionId";
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);
                quantity = (int)cmd.ExecuteScalar();
            }
            return quantity;
        }

        /*public static int GetTotalPrice(string sessionId, int productId)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            int price = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = @"Select Count(*) From Cart Where ProductId=@ProductId and SessionId=@SessionId";
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);
                price = (int)cmd.ExecuteScalar();
            }
            return price;
        }*/
    }
}