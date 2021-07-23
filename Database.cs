using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TaskManagement.Controllers;

namespace ProjectManagement
{
    public class Database
    {


        public Server server;

        private string sql;
        public NpgsqlDataReader data;
        public Database(string sql, IConfiguration _server)
        {
            this.sql = sql;
            server = new Server(_server);

            NpgsqlCommand query = new NpgsqlCommand(sql, server.conn);
            data = query.ExecuteReader();

        }


        public void Close()
        {
            server.conn.Close();
        }


    }



    public class Server
    {
        public NpgsqlConnection conn;
        public Server(IConfiguration _server)
        {

            /*
            string connstring = String.Format(
                                    "Server={0};" + "Port={1};" + "UserId = {2};" + "Password = {3};" + " Database = {4};",
                                    " localhost", "5432", "tms", "tms", "tms");
            */


            string connstring = String.Format(
                _server
                .GetSection("ConnectionStrings")
                .GetSection("AuthDbContextConnection").Value
                );

            conn = new NpgsqlConnection(connstring);
            conn.Open();

        }
    }


}