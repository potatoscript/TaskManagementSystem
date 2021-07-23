using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectManagement;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class DashboardController : Controller
    {

        public IConfiguration _server;
        public DashboardController(IConfiguration configuration)
        {
            _server = configuration;
        }

        public IActionResult Index(
            String StartDate, 
            String EndDate, 
            String page,
            String searchpic,
            String searchstatus,
            String searchproject,
            String thisyear)
        {

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.page = page;
            ViewBag.searchpic = searchpic;
            ViewBag.searchstatus = searchstatus;
            ViewBag.searchproject = searchproject;


            if (thisyear == "-")
            {
                ViewBag.thisyear = Int32.Parse(DateTime.Now.AddDays(0).ToString("yyyy"));
            }
            else
            {
                ViewBag.thisyear = thisyear;
            }
            var model = new List<DataModel>();

            Constant d = new Constant();
            string sql = "SELECT DISTINCT " + d.pic + ", COUNT(" + d.name + "), ";

            sql += " COUNT( CASE WHEN status != 'Done' THEN 1 END),";
            sql += " COUNT( CASE WHEN status = 'Done' THEN 1 END)";

            sql += " FROM " + d.table;
            sql += " WHERE  substring(" + d.startdate + ",0,5) = '" + ViewBag.thisyear+"' ";
            sql += " GROUP BY " + d.pic + " ";
            sql += " ORDER BY COUNT(" + d.name + ") DESC ";

            Database db = new Database(sql, _server);
            if (db.data.HasRows)
            {
                while (db.data.Read())
                {
                    model.Add(new DataModel
                    {
                        pic = db.data[0].ToString().Trim(),
                        status = db.data[2].ToString().Trim(),
                        status_done = db.data[3].ToString().Trim()
                    
                    });
                    
                }
            }
            db.Close();


            return View(model);
        }






    }
}