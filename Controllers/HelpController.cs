using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    public class HelpController : Controller
    {
        public IActionResult Index(
            String StartDate, 
            String EndDate, 
            String page, 
            String searchpic, 
            String searchstatus,
            String searchproject)
        {
            if (StartDate == "" || StartDate == null)
                StartDate = DateTime.Now.AddDays(-5).ToString("dd-MM-yyyy");
            if (EndDate == "" || EndDate == null)
                EndDate = DateTime.Now.AddDays(30).ToString("dd-MM-yyyy");

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.page = page;
            ViewBag.searchpic = searchpic;
            ViewBag.searchstatus = searchstatus;
            ViewBag.searchproject = searchproject;
            return View();
        }
    }
}