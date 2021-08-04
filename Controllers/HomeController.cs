using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ProjectManagement;
using TaskManagement.Models;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace TaskManagement.Controllers
{
    
    public class HomeController : Controller
    {

        public IConfiguration _server;
        public HomeController(IConfiguration configuration)
        {
            _server = configuration;
        }

        public IActionResult Index()
        {
            string StartDate = DateTime.Now.AddDays(-5).ToString("dd-MM-yyyy");
            string EndDate = DateTime.Now.AddDays(30).ToString("dd-MM-yyyy");
            Constant d = new Constant();
            ViewBag.totalPerPage = d.totalPerPage;
            ViewBag.finalProcess = d.final_process;
            ViewBag.Url = "Home";
            dynamic mymodel = new ExpandoObject();
            mymodel.Task = Read(StartDate, EndDate, "1","false","All_PIC","In_Progress","-");
            SharedController m = new SharedController(_server);
            mymodel.Process = m.ReadProcess(StartDate, EndDate,null);

            ViewBag.TotalOverDue = CheckDueDate();

            return View(mymodel);
        }

        public async Task<IActionResult> ReloadTable(
            String StartDate, 
            String EndDate,
            String page, 
            String searchpic, 
            String searchstatus,
            String searchproject)
        {
            Constant d = new Constant();
            ViewBag.finalProcess = d.final_process;
            ViewBag.totalPerPage = d.totalPerPage;
            ViewBag.Url = "Home";
            ViewBag.searchpic = searchpic;
            ViewBag.searchstatus = searchstatus;
            ViewBag.searchproject = searchproject;
            dynamic mymodel = new ExpandoObject();
            mymodel.Task = Read(StartDate, EndDate, page, "false", searchpic, searchstatus, searchproject);
            SharedController m = new SharedController(_server);
            mymodel.Process = m.ReadProcess(StartDate, EndDate, null);

            ViewBag.TotalOverDue = CheckDueDate();


            return View("Index", mymodel);
        }



        public string CheckDueDate()
        {
            Constant d = new Constant();
            //check for overdue item
            string sql = "SELECT " + d.duedate + " FROM " + d.table +
                            " WHERE " + d.status + " != '" + d.final_process + "' ";
            Database db = new Database(sql, _server);
            int totaloverdue = 0;
            if (db.data.HasRows)
            {
                while (db.data.Read())
                {
                    char[] sp = { '-' };
                    var due = db.data[0].ToString().Split(sp, StringSplitOptions.RemoveEmptyEntries);
                    string duedate = due[2] + due[1] + due[0];
                    var today = DateTime.Now.AddDays(0).ToString("yyyyMMdd");
                    if (int.Parse(today) >= int.Parse(duedate))
                        totaloverdue++;
                }
            }
            db.Close();
            return totaloverdue.ToString();
        }
        

        public List<DataModel> Read(
            String StartDate, 
            String EndDate, 
            String page, 
            String delay, 
            String searchpic, 
            String searchstatus,
            String searchproject)
        {

            CultureInfo culture = new CultureInfo("ja");

            DateTime startdate, enddate;

            char[] sp = { '-' };
            char[] sp2 = { '_' };
            var sdate = StartDate.Split(sp, StringSplitOptions.RemoveEmptyEntries);
            var edate = EndDate.Split(sp, StringSplitOptions.RemoveEmptyEntries);

            var _start = sdate[2] + sdate[1] + sdate[0];
            var _end = edate[2] + edate[1] + edate[0];

            startdate = Convert.ToDateTime(sdate[2] + "/" + sdate[1] + "/" + sdate[0] + " 12:10:15 PM", culture);
            enddate = Convert.ToDateTime(edate[2] + "/" + edate[1] + "/" + edate[0] + " 12:10:15 PM", culture);
            
            var nday = (enddate - startdate).Days;


            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.nDay = nday;
            ViewBag.page = page;
            ViewBag.searchpic = searchpic;
            ViewBag.searchstatus = searchstatus;
            ViewBag.searchproject = searchproject;

            DataModel list = new DataModel();

            var today_0 = Int32.Parse(DateTime.Now.AddDays(0).ToString("yyyyMMdd"));


            Constant d = new Constant();
                string sql = "SELECT " +
                    d.id + "," +
                    d.name + "," +
                    d.pic + "," +
                    d.spec + "," +
                    d.email + "," +
                    d.duedate + "," +
                    d.status + "," +
                    d.remark + ", " +
                    d.duedate2 + ", " +
                    d.collaborate + ", " +
                    d.email2 + ", " +
                    d.spec2 + " " +
                    " FROM " + d.table;
            sql += " WHERE " + d.id + " != '0' ";
            if (searchstatus == "Over_Due")
            {
                sql += " AND "+d.status+" != '"+d.final_process+"' AND split_part(" + d.duedate + ", '-', 3)||" +
                    "split_part(" + d.duedate + ", '-', 2)||" +
                    "split_part(" + d.duedate + ", '-', 1) <= '" + today_0 + "' ";
            }
            if (searchstatus == "In_Progress")
            {
                sql += " AND " + d.status + " != '" + d.final_process + "' ";
            }
            if (searchstatus == "Completed")
            {
                sql += " AND " + d.status + " = '" + d.final_process + "' ";
            }
            if (searchproject != "-" && searchproject != "" && searchproject != " ")
            {
                sql += " AND " + d.name + " LIKE ('%" + searchproject + "%') ";
            }
            if (searchpic != "All_PIC")
            {
                sql += " AND " + d.pic + " = '" + searchpic + "' ";
            }



            //sql += " WHERE " + d.startdate + " >= '" + _start + "' ";
            //sql += " AND " + d.startdate + " <= '" + _end + "' ";
            sql += " ORDER BY "+d.name+" ";

                Database db = new Database(sql, _server);
                //db.Open();
                if (db.data.HasRows)
                {
                    while (db.data.Read())
                    {
                        string _name = db.data[1].ToString().Trim();
                        _name = _name.Replace("\n", Environment.NewLine);

                    var _d= db.data[5].ToString().Split(sp, StringSplitOptions.RemoveEmptyEntries);
                    var _due = _d[2] + _d[1] + _d[0];


                    list.ListModel.Add(new DataModel
                        {
                            Id = Int32.Parse(db.data[0].ToString()),
                            name = _name,
                            pic = db.data[2].ToString(),
                            spec = db.data[3].ToString(),
                            email = db.data[4].ToString().Trim(),
                            duedate = db.data[8].ToString(),
                            status = db.data[6].ToString(),
                            remark = db.data[7].ToString(),
                            collaborate = db.data[9].ToString(),
                            email2 = db.data[10].ToString(),
                            spec2 = db.data[11].ToString(),
                            duedate2 = _due
                        });

                    }
                }
                db.Close();

            
            List<DataModel> model = list.ListModel.ToList();

            return model;

        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}