using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Admin_Portal.Data
{
    public class AdminRepository
    {
        private string _connection;

        public AdminRepository(string constr)
        {
            _connection = constr;
        }

        public IEnumerable<Admin> GetAdmins()
        {
            return GetItems<Admin>("SELECT * FROM admin");
        }

        public IEnumerable<Admin> SearchAdmins(string type, string op, string searchtext)
        {
            var sql = "SELECT * FROM admin WHERE {0} LIKE @search";
            var field = "";
            switch (type)
            {
                case "AdminName": field = "Email"; break;
                case "Admin Type": field = "Admin_Type"; break;
                default: field = "Email"; break;
            }
            var ops = op == "Starts With" ? searchtext + "%" : op == "Contains" ? "%" + searchtext + "%" : "%" + searchtext;
            return GetItems<Admin>(String.Format(sql, field), new { search = ops});
        }

        public Admin GetAdmin(string name)
        {
            return GetItems<Admin>("SELECT * FROM admin WHERE Email = @name", new { name = name }).FirstOrDefault();
        }

        public Admin GetAdmin(int id)
        {
            return GetItems<Admin>("SELECT * FROM admin WHERE AdminID = @id", new { id = id }).FirstOrDefault();
        }

        public void SaveAdmin(Admin admin)
        {
            var sql = @"
            UPDATE admin
            SET Email = @Email,
            Password = @Password,
            Admin_Type = @Admin_Type
            WHERE AdminID = @AdminID;";

            Execute (sql, admin);
        }

        public void AddAdmin(Admin admin)
        {
            var sql = @"
            INSERT admin (Email, Password, Admin_Type)
            VALUES (@Email, @Password, @Admin_Type)";

            Execute(sql, admin);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        public IEnumerable<T> GetItems<T>(string sql, object parms = null)
        {
            using (var conn = new MySqlConnection(_connection))
            {
                try
                {
                    return conn.Query<T>(sql, parms);
                }
                catch (Exception e)
                {
                    var err = e.Message;
                    return null;
                }
            }
        }

        public void Execute(string sql, object parms = null)
        {
            using (var conn = new MySqlConnection(_connection))
            {
                conn.Execute(sql, parms);
            }
        }
    }
}