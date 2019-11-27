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

        public IEnumerable<User> GetUsers()
        {
            return GetItems<User>("SELECT * FROM user");
        }

        public IEnumerable<User> SearchUsers(string type, string op, string searchtext)
        {
            var sql = "SELECT * FROM user WHERE {0} LIKE @search";
            var field = "";
            switch (type)
            {
                case "UserName": field = "UserID"; break;
                default: field = "UserID"; break;
            }
            var ops = op == "Starts With" ? searchtext + "%" : op == "Contains" ? "%" + searchtext + "%" : "%" + searchtext;
            return GetItems<User>(String.Format(sql, field), new { search = ops});
        }

        public User GetUser(String userid)
        {
            return GetItems<User>("SELECT * FROM admins WHERE userid = @Email", new { Email = userid }).FirstOrDefault();
        }

        public void SaveUser(User user)
        {

            var sql = @"
            UPDATE user
            SET UserID = @UserID,
            Email = @Email,
            Password = @Password,
            User_Type = @User_Type
            WHERE UserID = @UserID;";

            Execute (sql, user);
        }

        public void AddUser(User user)
        {
            var sql = @"
            INSERT user (UserID, Email, Password, User_Type)
            VALUES (@UserID, @Email, @Password, @User_Type)";

            Execute(sql, user);
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