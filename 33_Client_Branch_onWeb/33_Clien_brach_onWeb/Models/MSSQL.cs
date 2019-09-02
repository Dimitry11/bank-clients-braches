using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _33_Clien_brach_onWeb.Models
{
    public class MSSQL
    {
        readonly string connectionString = @"Data Source=DESKTOP-6NBVMFM\MSSQLSERVER1;Initial Catalog=32_new_test;Integrated Security=True";

        public SqlConnection conn = null;
        public string error { get; private set; }
        public string query { get; set; }

        public MSSQL()
        {
            try
            {
                error = "";
                query = "OPEN CONNECTION TO MS SQL";
                conn = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                conn = null;
            }
        }

        ~MSSQL()
        {
            try
            {
                conn.Close();
            }
            catch { }
        }

        public DataTable Select(string myQuery)
        {
            if (IsError()) return null;
            try
            {
                query = myQuery;
                DataTable table = new DataTable();
                SqlCommand cmd = new SqlCommand(myQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                table.Load(reader);
                return table;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        public long Insert(string myQuery)  // Результатом являеться id добавленной записи
        {
            if (IsError()) return -1;
            try
            {
                query = myQuery;
                SqlCommand cmd = new SqlCommand(query, conn);
                int result = cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return -1;
            }
        }

        public bool IsError()
        {
            return error != "";

        }

        public string AddSlashes(string text)  // Защита от SQL инекций
        {
            return text.Replace("\'", "\\\'");
        }
    }
}