using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectManagement;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{

    public class DetailController : Controller
    {

        public IConfiguration _server;
        public DetailController(IConfiguration configuration)
        {
            _server = configuration;
        }

        public IActionResult Index(
            int? id, 
            String taskname, 
            String StartDate, 
            String EndDate, 
            String page,
            String email,
            String duedate,
            String searchpic,
            String searchstatus,
            String searchproject
            )
        {

            string name = taskname;//.Replace(" ", "_");
            try
            {
                char[] separatingChars = {'.'};
                var str = name.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
                name = str[0];
            }
            catch (Exception) { }


            dynamic mymodel = new ExpandoObject();
            ViewBag.td_tms_id = id;
            ViewBag.taskname = name;
            ViewBag.page = page;
            ViewBag.searchpic = searchpic;
            ViewBag.searchstatus = searchstatus;
            ViewBag.searchproject = searchproject;
            ViewBag.duedate = duedate;
            ViewBag.email = email.Trim();
            ViewBag.Url = "Detail";
            Constant d = new Constant();
            ViewBag.finalProcess = d.final_process;
            mymodel.Task = Read(StartDate, EndDate, id.ToString());
            SharedController m = new SharedController(_server);
            mymodel.Process = m.ReadProcess(StartDate, EndDate, id.ToString());
            return View(mymodel);

        }

        public async Task<IActionResult> ReloadTable(
            String StartDate, 
            String EndDate, 
            String td_tms_id, 
            String taskname, 
            String page,
            String email,
            String duedate,
            String searchpic,
            String searchstatus,
            String searchproject
            )
        {
            dynamic mymodel = new ExpandoObject();
           
            Constant d = new Constant();
            ViewBag.finalProcess = d.final_process;

            ViewBag.td_tms_id = td_tms_id;
            ViewBag.taskname=taskname;
            ViewBag.page = page;
            ViewBag.duedate = duedate;
            ViewBag.searchpic = searchpic;
            ViewBag.searchstatus = searchstatus;
            ViewBag.searchproject = searchproject;
            ViewBag.Url = "Detail";
            ViewBag.email = email.Trim();
            mymodel.Task = Read(StartDate, EndDate, td_tms_id);
            SharedController m = new SharedController(_server);
            mymodel.Process = m.ReadProcess(StartDate, EndDate, td_tms_id);
            return View("Index", mymodel);
        }

        public List<DataModel> Read(
            String StartDate, String EndDate, String id)
        {

            CultureInfo culture = new CultureInfo("ja");

            DateTime startdate, enddate;

            char[] sp = { '-' };
            var sdate = StartDate.Split(sp, StringSplitOptions.RemoveEmptyEntries);
            var edate = EndDate.Split(sp, StringSplitOptions.RemoveEmptyEntries);

            var _start = sdate[2] + sdate[1] + sdate[0];
            var _end = edate[2] + edate[1] + edate[0];

            startdate = Convert.ToDateTime(sdate[2] + "/" + sdate[1] + "/" + sdate[0] + " 12:10:15 PM", culture);
            enddate = Convert.ToDateTime(edate[2] + "/" + edate[1] + "/" + edate[0] + " 12:10:15 PM", culture);
            
            var nday = (enddate - startdate).Days;


            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.id = id;
            ViewBag.nDay = nday;

            DataModel list = new DataModel();
            try
            {
                Constant d = new Constant();
                string sql = "SELECT " +
                    d.id + "," +
                    d.name + "," +
                    d.pic + "," +
                    d.spec + "," +
                    d.email + "," +
                    d.duedate + "," +
                    d.status + "," +
                    d.remark + " " +
                    " FROM " + d.tableDetail;
  
                    sql += " WHERE " + d.td_tms_id + " = '" + id + "' ";
               // sql += " AND " + d.startdate + " >= '" + _start + "' ";
               // sql += " AND " + d.startdate + " <= '" + _end + "' ";
                sql += " ORDER BY " + d.name + " ";


                Database db = new Database(sql, _server);
                //db.Open();
                if (db.data.HasRows)
                {
                    while (db.data.Read())
                    {

                        var _d = db.data[5].ToString().Split(sp, StringSplitOptions.RemoveEmptyEntries);
                        var _due = _d[2] + _d[1] + _d[0];

                        list.ListModel.Add(new DataModel
                        {
                            Id = Int32.Parse(db.data[0].ToString()),
                            name = db.data[1].ToString(),
                            pic = db.data[2].ToString(),
                            spec = db.data[3].ToString(),
                            email = db.data[4].ToString().Trim(),
                            duedate = db.data[5].ToString(),
                            status = db.data[6].ToString(),
                            remark = db.data[7].ToString(),
                            duedate2 = _due
                        });

                    }
                }
                db.Close();
            }
            catch (Exception e)
            {

            }

            List<DataModel> model = list.ListModel.ToList();

            return model;

        }


    }
}