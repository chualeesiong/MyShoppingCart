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
    public class CustomerData
    {
        public static Customer GetCustomerbyUserName(string username)
        {
            Customer c1 = null;

            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = @"Select CustomerId, FirstName, LastName, Username, Password From Customer Where Username =@Username";
                cmd.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        c1 = new Customer()
                        {
                            CustomerId = (int)reader["CustomerId"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Username = (string)reader["Username"],
                            Password = (string)reader["Password"]
                        };
                    }
                }
            }
            return c1;
        }

        public static Customer GetCustomerbySessionId(string sessionId)
        {
            Customer c1 = null;

            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = @"Select CustomerId, FirstName, LastName, Username, Password From Customer Where SessionId =@SessionId";
                cmd.Parameters.AddWithValue("@SessionId", sessionId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        c1 = new Customer
                        {
                            CustomerId = (int)reader["CustomerId"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Username = (string)reader["Username"],
                            Password = (string)reader["Password"]
                        };
                    }
                }
            }
            return c1;
        }
    }
}