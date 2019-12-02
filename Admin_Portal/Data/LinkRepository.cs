﻿using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Security.Principal;
using System.Collections.Generic;
using System.Linq;

namespace Admin_Portal.Data
{
    public class LinkRepository
    {
        private string _connection;

        public LinkRepository(string constr)
        {
            _connection = constr;
        }

        public IEnumerable<Link> GetLink()
        {
            return GetItems<Link>("SELECT * FROM links");
        }

        public IEnumerable<Link> SearchLinks(string type, string op, string searchtext)
        {
            var sql = "SELECT * FROM links WHERE {0} LIKE @search";
            var field = "";
            switch (type)
            {
                case "Link Name": field = "Link_Name"; break;
                case "Link Type": field = "Link_Type"; break;
                default: field = "Link_Name"; break;
            }
            var ops = op == "Starts With" ? searchtext + "%" : op == "Contains" ? "%" + searchtext + "%" : "%" + searchtext;
            return GetItems<Link>(String.Format(sql, field), new { search = ops});
        }

        public IEnumerable<Link> ListLinks (string op, IPrincipal user)
        {
            var sql = "SELECT * FROM links WHERE {0} LIKE @search";
            var field = "Link_Type";
            var searchtext = (user as Admin_Portal.Data.Admin).Admin_Type;
            var ops = op == "Starts With" ? searchtext + "%" : op == "Contains" ? "%" + searchtext + "%" : "%" + searchtext;
            return GetItems<Link>(String.Format(sql, field), new { search = ops });
        }

        public Link GetLink(int id)
        {
            return GetItems<Link>("SELECT * FROM links WHERE LinkID = @id", new { id = id }).FirstOrDefault();
        }

        public Link GetLink(string Link_Name)
        {
            return GetItems<Link>("SELECT * FROM links WHERE Link_Name = @Link_Name", new { Link_Name = Link_Name }).FirstOrDefault();
        }

        public void SaveLink(Link link)
        {
            var sql = @"
            UPDATE links
            SET Link_Name = @Link_Name,
            Link_Type = @Link_Type
            WHERE LinkID = @LinkID;";

            Execute (sql, link);
        }

        public void AddLink(Link link)
        {
            var sql = @"
            INSERT links (Link_Name, Link_Type)
            VALUES (@Link_Name, @Link_Type)";

            Execute(sql, link);
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