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
    public class ProductData
    {
        public static Product GetProductById(int productId)
        {
            Product p1 = null;

            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = @"Select ProductId,ProductName, ProductDesc, ProductPrice from Product where ProductId=@ProductId";

                cmd.Parameters.AddWithValue("@ProductId", productId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        p1 = new Product()
                        {
                            ProductId = (int)reader["ProductId"],                       
                            ProductName = (string)reader["ProductName"],
                            ProductDesc = (string)reader["ProductDesc"],
                            ProductPrice = (int)reader["ProductPrice"]
                        };                     
                    }
                }
            }
            return p1;
        }

        public static List<Product> DisplayProducts()
        {
            List<Product> products = new List<Product>();

            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = @"Select ProductId, ProductName, ProductDesc, ProductPrice from Product";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Product p1 = new Product()
                        {
                            ProductId = (int)reader["ProductId"],
                            ProductName = (string)reader["ProductName"],
                            ProductDesc = (string)reader["ProductDesc"],
                            ProductPrice = (int)reader["ProductPrice"]
                        };
                        products.Add(p1);
                    }                   
                }
            }
            return products;
        }
    }
}