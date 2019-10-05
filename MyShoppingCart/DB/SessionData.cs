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
    public class SessionData
    {
        public static string CreateSession(int CustomerId)
        {
            string sessionId = Guid.NewGuid().ToString();
            

            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Customer Set SessionId=@SessionId where CustomerId=@Id";
                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@Id", CustomerId);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);

                cmd.ExecuteNonQuery();
            }
            return sessionId;
        }

        public static void RemoveSession(string sessionId)
        {
            
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = @"Update Customer Set SessionId=NULL Where SessionId = @SessionId";
                cmd.Parameters.AddWithValue("@SessionId", sessionId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}