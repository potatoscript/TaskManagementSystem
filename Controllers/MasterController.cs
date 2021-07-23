using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ProjectManagement;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class MasterController : Controller
    {
        public IConfiguration _server;

        public MasterController(IConfiguration configuration)
        {
            _server = configuration;

            
        }


        [Authorize]
        public IActionResult Index(
            String StartDate,
            String EndDate, 
            String page, 
            String searchpic, 
            String searchstatus,
            String searchproject)
        {
            if(StartDate==""||StartDate==null)
                StartDate = DateTime.Now.AddDays(-5).ToString("dd-MM-yyyy");
            if (EndDate == ""||EndDate==null)
                EndDate = DateTime.Now.AddDays(30).ToString("dd-MM-yyyy");

            ViewBag.StartDate= StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.page = page;
            ViewBag.searchpic = searchpic;
            ViewBag.searchstatus = searchstatus;
            ViewBag.searchproject = searchproject;
            return View(Read());
        }

        public IActionResult Create(
            String StartDate, 
            String EndDate, 
            String page, 
            String searchpic,
            String searchstatus,
            String searchproject
            )
        {
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.page = page;
            ViewBag.searchpic = searchpic;
            ViewBag.searchstatus = searchstatus;
            ViewBag.searchproject = searchproject;

            Master model  = new Master();
            ViewData["Color"] = model.list;//from the Master Model List<SelectListItem> list

            return View(new Master());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            Master model, 
            String StartDate,
            String EndDate, 
            String page,
            String searchpic,
            String searchstatus,
            String searchproject
            )
        {
            try
            {
                Constant d = new Constant();
                string sql = "INSERT INTO " + d.tableMaster + "( " +
                                d.id + "," +
                                d.name + ", " +
                                d.colour + ", " +
                                d.spec + " " +
                                ")SELECT COALESCE(MAX(" + d.id + "::Integer),0)+1," +
                                "'" + model.name + "'," +
                                "'" + model.colour + "', " +
                                "'" + model.spec + "' " +
                                " FROM " + d.tableMaster;
                Database db = new Database(sql, _server);
                db.Close();
                //return RedirectToAction("Index", new { StartDate = StartDate, EndDate = EndDate, page = page });
                return Json(new { 
                    isValid = true,
                    controllerx = "Master",
                    startdate = StartDate, 
                    enddate = EndDate, 
                    page = page, 
                    searchpic = searchpic, 
                    searchstatus = searchstatus, 
                    searchproject = searchproject
                });

            }
            catch (Exception e)
            {
                TempData["alert"] = e.Message;
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", model) });
        }


        public IActionResult Edit(
            int? Id, 
            String StartDate, 
            String EndDate, 
            String page,
            String searchpic,
            String searchstatus,
            String searchproject
            )
        {
            ViewBag.startdate = StartDate;
            ViewBag.enddate = EndDate;
            ViewBag.page = page;
            ViewBag.searchpic = searchpic;
            ViewBag.searchstatus = searchstatus;
            ViewBag.searchproject = searchproject;
            Constant d = new Constant();
            string sql = "SELECT " +
                    d.name + "," +
                    d.colour + "," +
                    d.spec + " " +
                    " FROM " + d.tableMaster;
            sql += " WHERE " + d.id + "='"+Id+"' ";
            Database db = new Database(sql, _server);
            if (db.data.HasRows)
            {
                while (db.data.Read())
                {
                    TempData["name"] = db.data[0].ToString().Trim();
                    TempData["colour"] = db.data[1].ToString().Trim();
                    TempData["spec"] = db.data[2].ToString().Trim();
                    
                }
            }
            
            db.Close();

            Master model = new Master();
            ViewData["Color"] = model.list;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            int? Id, 
            String StartDate, 
            String EndDate, 
            String page, 
            String searchpic,
            String searchstatus,
            String searchproject,
            Master model)
        {
            if (Id == null || Id == 0)  //this is used for the validation as well but in the server side
            {
                return NotFound();
            }
            else
            {
                try
                {
                    Constant d = new Constant();

                    string sql = "UPDATE " + d.tableMaster + " SET( " +
                                    d.name + ", " +
                                    d.spec + ", " +
                                    d.colour + " " +
                                    ")=(" +
                                    "'" + model.name + "'," +
                                    "'" + model.spec + "', " +
                                    "'" + model.colour + "') " +
                                    " WHERE " + d.id + "= '" + Id + "' ";
                    Database db = new Database(sql, _server);
                    db.Close();
                    return RedirectToAction(
                        "Index", 
                        new { 
                            StartDate = StartDate, 
                            EndDate = EndDate, 
                            page = page, 
                            searchpic = searchpic,
                            searchstatus = searchstatus,
                            searchproject = searchproject
                        });
                }
                catch (Exception e)
                {
                    TempData["alert"] = e.Message;
                }
            }
            return View();
        }

        public IActionResult Delete(
            bool confirm, 
            int Id, 
            String StartDate, 
            String EndDate, 
            String page,
            String searchpic,
            String searchstatus,
            String searchproject
            )
        {
            if (Id == null || Id == 0)  //this is used for the validation as well but in the server side
            {
                return NotFound();
            }
            else
            {
                if (ModelState.IsValid && confirm == true)
                {
                    Constant d = new Constant();
                    string sql = "DELETE FROM " + d.tableMaster +
                                    " WHERE " + d.id + "= '" + Id + "' ";
                    Database db = new Database(sql, _server);
                    db.Close();
                    return RedirectToAction(
                        "Index", 
                        new { 
                            StartDate = StartDate, 
                            EndDate = EndDate, 
                            page = page,
                            searchpic= searchpic, 
                            searchstatus = searchstatus,
                            searchproject = searchproject
                        });
                }
            }
            return View();
        }

        public List<Master> Process(String spec)
        {
            Master list = new Master();

            Constant d = new Constant();
            string sql = "SELECT * FROM " + d.tableMaster;
            sql += " WHERE "+d.spec+" = '"+spec+"' OR "+d.spec+"='' OR "+d.spec+"='-' ";
            sql += " ORDER BY name ";

            Database db = new Database(sql, _server);

            if (db.data.HasRows)
            {
                while (db.data.Read())
                {
                    list.MasterModel.Add(new Master
                    {
                        Id = Int32.Parse(db.data[0].ToString()),
                        name = db.data[1].ToString(),
                        colour = db.data[3].ToString()
                    });

                }
            }
            db.Close();

            List<Master> model = list.MasterModel.ToList();

            return model;
        }

        public List<Master> Read()
        {

            Master list = new Master();

                Constant d = new Constant();
                string sql = "SELECT " +
                    d.id + "," +
                    d.name + "," +
                    d.colour + "," +
                    d.spec + " " +
                    " FROM " + d.tableMaster;
                sql += " ORDER BY " + d.name;

                Database db = new Database(sql, _server);
                if (db.data.HasRows)
                {
                    while (db.data.Read())
                    {
                        list.MasterModel.Add(new Master
                        {
                            Id = Int32.Parse(db.data[0].ToString()),
                            name = db.data[1].ToString(),
                            colour = db.data[2].ToString(),
                            spec = db.data[3].ToString()
                        });

                    }
                }
                db.Close();


            List<Master> model = list.MasterModel.ToList();

            return model;

        }

    }
}