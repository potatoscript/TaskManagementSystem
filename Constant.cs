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
    public class Constant
    {
        public string id { get; set; }

        public string collaborate { get; set; }
        public string colour { get; set; }
        public string date { get; set; }

        public string doc { get; set; }
        public string duedate { get; set; }
        public string duedate2 { get; set; }
        public string email { get; set; }
        public string email2 { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string pic { get; set; }
        public string project_id { get; set; }
        public string remark { get; set; }
        public string spec { get; set; }
        public string spec2 { get; set; }
        public string startdate { get; set; }
        public string status { get; set; }
        public string table { get; set; }
        public string tableDetail { get; set; }
        public string tableMaster { get; set; }
        public string tableProcess { get; set; }
        public string taskname { get; set; }
        public Int32 totalPerPage { get; set; }
        public string  type { get; set; }

        public string tableUser { get; set; }

        public string tableDoc { get; set; }
        public string td_tms_id { get; set; }
        public string final_process { get; set; }

        public string url { get; set; }

        public Constant()
        {
            id = "id";
            collaborate = "collaborate";
            colour = "colour";
            date = "date";
            doc = "doc";
            duedate = "duedate";
            duedate2 = "duedate2";
            email = "email";
            email2 = "email2";
            final_process = "Done";
            name = "name";
            password = "password";
            pic = "pic";
            project_id = "project_id";
            remark = "remark";
            spec = "spec";
            spec2 = "spec2";
            startdate = "startdate";
            status = "status";

            table = "td_tms";
            tableDetail = "td_tms_detail";
            tableDoc = "td_tms_doc";
            tableMaster = "tm_tms";
            tableProcess = "td_tms_process";
          
            
            taskname = "taskname";
            td_tms_id = "td_tms_id";
            totalPerPage = 10;
            type = "type";

        }
    }
}