using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ProjectManagement;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class SharedController : Controller
    {

        public IConfiguration _server;
        public IConfiguration _email;
        public string controller="Home";

        public SharedController(IConfiguration configuration)
        {
            _server = configuration;
            _email = configuration;

        }


        [Authorize]
        public IActionResult Create(
            String Controllerx, 
            String StartDate, 
            String EndDate, 
            String page, 
            String UserName,
            String td_tms_id, 
            String taskname,
            String email,
            String duedate,
            String searchpic,
            String searchstatus,
            String searchproject
        )
        {
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.username = UserName;
            ViewBag.page = page;
            ViewBag.Controllerx = Controllerx;
            ViewBag.td_tms_id = td_tms_id;
            ViewBag.taskname = taskname;
            ViewBag.email = email;
            ViewBag.duedate = duedate;
            ViewBag.searchpic = searchpic;
            ViewBag.searchstatus = searchstatus;
            ViewBag.searchproject = searchproject;

            DataModel model = new DataModel();
            

            Constant d = new Constant();

            string sqle = "SELECT " +
                            d.name + ", " +
                            d.email + ", "+
                            d.spec + " ";
            sqle += " FROM " + d.tableUser ;
            Database dbe = new Database(sqle, _server);
            if (dbe.data.HasRows)
            {
                while (dbe.data.Read())
                {
                    model.ListEmail.Add(
                        new SelectListItem { 
                            Text = dbe.data[0].ToString().Trim(), 
                            Value = dbe.data[1].ToString().Trim()+"#"+ dbe.data[2].ToString().Trim()+"#"+dbe.data[0].ToString().Trim()
                        }
                    );

                }
            }
            dbe.Close();

            ViewData["Email"] = model.ListEmail;

            
            string sql = "SELECT " + d.name + "," + d.spec + " FROM " + d.tableUser +
                            " WHERE " + d.email + " = '" + UserName + "' ";
            Database db = new Database(sql, _server);
            if (db.data.HasRows)
            {
                while (db.data.Read())
                {
                    ViewBag.username = db.data[0].ToString().Trim();
                    ViewBag.userspec = db.data[1].ToString().Trim();
                }
            }
            db.Close();
            

            return View(new CreateModel());
        }

        //post method
        [HttpPost]
        // to prevent cross-site request forgery attacks. 
        //an attack in which a harmful script element, malicious commnad or code is sent from the browser of a trusted user
        [ValidateAntiForgeryToken]
        public IActionResult Create(String Controllerx, 
            String StartDate, 
            String EndDate,
            String td_tms_id, 
            String taskname,
            String page,
            String email,
            String duedate,
            String searchpic,
            String searchstatus,
            String searchproject,
            CreateModel model
            )
        {
           // if (ModelState.IsValid)  //this is used for the validation as well but in the server side
           // {

                char[] sp = { '-' };
                var sdate = model.registerdate.Split(sp, StringSplitOptions.RemoveEmptyEntries);
                var _start = sdate[2] + sdate[1] + sdate[0];

                Constant d = new Constant();
                string sql = "INSERT INTO " + d.table; 
                if(Controllerx=="Detail") sql = "INSERT INTO " + d.tableDetail;
                sql += "( " + d.id + ",";
                if (Controllerx == "Detail") sql += d.td_tms_id + ",";
            sql += d.name + ", " +
                            d.pic + ", " +
                            d.collaborate + ", " +
                            d.email2 + ", " +
                            d.spec + ", " +
                            d.spec2 + ", " +
                            d.email + ", " +
                            d.duedate + ", " +
                            d.startdate + ", " +
                            d.status + ", ";

            if (Controllerx == "Home") sql += d.duedate2 + ",";

            var pic2 = model.pic2.Replace(" ","_");

            sql += d.remark + " " +
                                ")SELECT COALESCE(MAX(" + d.id + "::Integer),0)+1,";
                if (Controllerx == "Detail") sql +=  "'" + model.td_tms_id + "',";
            sql += "'" + model.name + "'," +
                    "'" + pic2 + "', " +
                    "'" + model.collaborate + "', " +
                    "'" + model.email2 + "', " +
                    "'" + model.spec + "', " +
                    "'" + model.spec2 + "', " +
                    "'" + model.email + "', " +
                    "'" + model.duedate + "', " +
                    "'" + _start + "', " +
                    "'-', ";
            if (Controllerx == "Home") sql += "'" + model.duedate + "', ";

            sql += "'" + model.remark + "' ";
                if (Controllerx == "Home") sql +=" FROM " + d.table;
                if (Controllerx == "Detail") sql +=" FROM " + d.tableDetail;
                Database db = new Database(sql, _server);
                db.Close();


            //Note only small case was allowed here
            return Json(new { isValid = true, 
                    controllerx = Controllerx, 
                    startdate = StartDate, 
                    enddate = EndDate,
                    td_tms_id = td_tms_id,
                    taskname = taskname,
                    page = page,
                    duedate = duedate,
                    email = email,
                    searchpic = searchpic,
                    searchstatus = searchstatus,
                    searchproject = searchproject
            });


           // }


            //return View();
           // return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", model) });
        }



        


        [Authorize]
        public IActionResult Edit(int? Id, 
            String Controllerx, 
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
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.page = page;
            ViewBag.searchpic = searchpic;
            ViewBag.searchstatus = searchstatus;
            ViewBag.searchproject = searchproject;
            ViewBag.td_tms_id = td_tms_id;
            ViewBag.taskname = taskname;
            ViewBag.email = email;
            ViewBag.duedate = duedate;
            ViewBag.Controllerx = Controllerx;


            DataModel model = new DataModel();


            Constant d = new Constant();

            string sqle = "SELECT " +
                            d.name + ", " +
                            d.email + ", " +
                            d.spec + " ";
            sqle += " FROM " + d.tableUser;
            Database dbe = new Database(sqle, _server);
            if (dbe.data.HasRows)
            {
                while (dbe.data.Read())
                {
                    model.ListEmail.Add(
                        new SelectListItem
                        {
                            Text = dbe.data[0].ToString().Trim(),
                            Value = dbe.data[1].ToString().Trim() + "#" + dbe.data[2].ToString().Trim() + "#" + dbe.data[0].ToString().Trim()
                        }
                    );

                }
            }
            dbe.Close();

            ViewData["Email"] = model.ListEmail;


                string sql = "SELECT " +
                                d.name + ", " +
                                d.pic + ", " +
                                d.spec + ", " +
                                d.email + ", " +
                                d.duedate + ", " +
                                d.startdate + ", " +
                                d.remark + ", " +
                                d.collaborate + ", " +
                                d.email2 + ", " +
                                d.spec2 + " ";
                if (Controllerx == "Detail")
                {
                    sql += " FROM " + d.tableDetail + " WHERE " + d.id + "= '" + Id + "' ";
                }
                else
                {
                    sql += " FROM " + d.table + " WHERE " + d.id + "= '" + Id + "' ";
                }
                Database db = new Database(sql, _server);
                //db.Open();
                if (db.data.HasRows)
                {
                    while (db.data.Read())
                    {
                        TempData["name"] = db.data[0].ToString().Trim();
                        TempData["pic"] = db.data[1].ToString().Trim();
                        TempData["spec"] = db.data[2].ToString().Trim();
                        TempData["email"] = db.data[3].ToString().Trim();
                        TempData["duedate"] = db.data[4].ToString().Trim();
                        TempData["duedate2"] = db.data[4].ToString().Trim();
                        TempData["startdate"] = db.data[5].ToString().Trim();
                        TempData["remark"] = db.data[6].ToString().Trim();
                        TempData["collaborate"] = db.data[7].ToString().Trim();
                        TempData["email2"] = db.data[8].ToString().Trim();
                        TempData["spec2"] = db.data[9].ToString().Trim();
                    }
                }
                db.Close();

            return View();
        }

        //post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? Id, 
            String Controllerx, 
            String StartDate, 
            String EndDate,
            String td_tms_id,
            String taskname,
            String page, 
            String email,
            String duedate,
            String searchpic,
            String searchstatus,
            String searchproject,
            DataModel model)
        {
            if (Id == null || Id == 0)  //this is used for the validation as well but in the server side
            {
                return NotFound();
            }
            else
            {
                    

                    Constant d = new Constant();

                    string sql = "UPDATE " + d.table;
                    if (Controllerx == "Detail") sql = "UPDATE " + d.tableDetail;


                sql += " SET( " +
                                    d.name + ", " +
                                    d.pic + ", " +
                                    d.collaborate + ", " +
                                    d.email2 + ", " +
                                    d.spec + ", " +
                                    d.spec2 + ", " +
                                    d.duedate + ", ";

                if (model.duedate != model.duedate2) sql += d.duedate2 + ", ";

                sql += d.remark + " " +
                                   ")=(" +
                                   "'" + model.name + "'," +
                                   "'" + model.pic + "', " +
                                   "'" + model.collaborate + "', " +
                                   "'" + model.email2 + "', " +
                                   "'" + model.spec + "', " +
                                   "'" + model.spec2 + "', " +
                                   "'" + model.duedate + "', ";

                if (model.duedate != model.duedate2) sql += d.duedate2 + "||'_" + model.duedate + "', ";


                sql +=         "'" + model.remark + "') " +
                                    " WHERE " + d.id + "= '" + Id + "' ";

                    Database db = new Database(sql, _server);
                    db.Close();
                    //return RedirectToAction("ReloadTable", new { StartDate = StartDate, EndDate = EndDate, page = page });
                    return Json(new { isValid = true, 
                        controllerx = Controllerx, 
                        startdate = StartDate, 
                        enddate = EndDate,
                        td_tms_id = td_tms_id,
                        taskname = taskname,
                        page = page,
                        duedate = duedate,
                        email = email,
                        searchpic = searchpic,
                        searchstatus = searchstatus,
                        searchproject = searchproject
                    });

            }
            return View();
        }

        [Authorize]
        public IActionResult Delete(
            bool confirm, 
            int Id, 
            String Controllerx, 
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
            if (Id == null || Id == 0)  //this is used for the validation as well but in the server side
            {
                return NotFound();
            }
            else
            {
                if (ModelState.IsValid && confirm == true)
                {

                        Constant d = new Constant();
                        string sql = "DELETE FROM " + d.table;
                        if (Controllerx == "Detail") sql = "DELETE FROM " + d.tableDetail;
                        sql += " WHERE " + d.id + "= '" + Id + "' ";
                        Database db = new Database(sql, _server);
                        db.Close();

                        sql = "DELETE FROM " + d.tableProcess +
                                      " WHERE " + d.td_tms_id + "= '" + Id + "' ";
                        sql += " OR "+d.project_id+"= '"+Id+"'";
                        Database db2 = new Database(sql, _server);
                        db2.Close();

                    if (Controllerx == "Home")
                        {
                            sql = "DELETE FROM " + d.tableDetail +
                                  " WHERE " + d.td_tms_id + "= '" + Id + "' ";
                            Database db3 = new Database(sql, _server);
                            db3.Close();
                        }


                        return RedirectToAction(
                            "ReloadTable",
                            Controllerx, 
                            new { 
                                StartDate = StartDate, 
                                EndDate = EndDate, 
                                td_tms_id = td_tms_id,
                                taskname = taskname,
                                page = page,
                                email = email,
                                duedate = duedate,
                                searchpic = searchpic,
                                searchstatus = searchstatus,
                                searchproject = searchproject
                            });

                }
            }

            return View();
        }

        public IActionResult Email(
            bool confirm,
            String Controllerx,
            String StartDate,
            String EndDate,
            String td_tms_id,
            String taskname,
            String email,
            String duedate,
            String pic,
            String page,
            String searchpic,
            String searchstatus,
            String searchproject
        )
        {
            if (confirm == true)
            {
                MailSettings mail_ = new MailSettings(_email);
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(mail_.Host);

                mail.From = new MailAddress(mail_.Mail,mail_.DisplayName);
                mail.To.Add(email);
                mail.Subject = taskname;
                mail.IsBodyHtml = true;
                String bodyText = "<div style='background:#e6f3ff;width:900px;border:1px solid black;border-radius:10px;margin-left:20px;padding:10px;padding-left:20px;font-family: Trebuchet MS, Arial, Helvetica, sans-serif;'>";
                bodyText += "<H3 style='color:orange'> Hello "+pic+",</H3>";
                bodyText += "<li>You have the following outstanding Task :</li><br>";
                bodyText += "<br><table style='margin-left:50px;border-collapse: collapse;'>";
                bodyText += "<tr><th style='padding:10px;border:1px solid black'>Task Name</th><th style='padding:10px;border:1px solid black'>Due Date</th></tr>";
                bodyText += "<tr><td style='padding:10px;border:1px solid black;background:white'>" + taskname+ "</td><td style='padding:10px;border:1px solid black;background:white'>" + duedate+"</td></tr>";
                bodyText += "</table>";
                bodyText += "<br><li>Please click the following link to confirm:</li>";
                bodyText += "<p style='margin-left:50px'><a href='http://nfbdev.nab.co.id/tmsystem'>Task Management System</a></p>";
                bodyText += "<br><br><hr><br>";
                bodyText += "<p><I>This is an automated message - Please do not reply directly to this email</I></p>";
                bodyText += "</div>";
                mail.Body = bodyText;

                SmtpServer.Port = mail_.Port;
                SmtpServer.EnableSsl = false;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Send(mail);


                return RedirectToAction(
                            "ReloadTable",
                            Controllerx,
                            new
                            {
                                StartDate = StartDate,
                                EndDate = EndDate,
                                td_tms_id = td_tms_id,
                                taskname = taskname,
                                page = page,
                                email = email,
                                duedate = duedate,
                                searchpic = searchpic,
                                searchstatus = searchstatus,
                                searchproject = searchproject
                            });

            }

            return View();
        }



        public void EditRemark(int id, String remark, String Controllerx)
        {
            Constant d = new Constant();

            string sql = "UPDATE " + d.table;
            if (Controllerx == "Detail") sql = "UPDATE " + d.tableDetail;
            sql += " SET " + d.remark + "='" + remark + "' WHERE " + d.id + "= '" + id + "' ";
            Database db = new Database(sql, _server);
            db.Close();
        }

        //before add the process check for the process existing
        public String CheckProcess(
            int id, 
            String date, 
            String content, 
            String td_tms_id)
        {
            Constant d = new Constant();

            string sql = "SELECT " + d.name + " FROM " + d.tableProcess +
                " WHERE " + d.td_tms_id + " = '" + id + "' ";
            sql += " AND " + d.date + "='" + date + "' ";
            sql += " AND " + d.name + "='" + content + "' ";
            if (td_tms_id != "0")
            {
                sql += " AND " + d.td_tms_id + "='" + td_tms_id + "' ";
                sql += " AND " + d.type + "='Detail' ";
            }
            else
            {
                sql += " AND " + d.type + "!='Detail' ";
            }
            Database db = new Database(sql, _server);
            if (db.data.HasRows)
            {
                return "1";
            }
            db.Close();
            return "0";
        }

        public void AddProcess(
            int id,
            String date,
            String content,
            String spec,
            String colour,
            String tdtmsid
        )
        {

            Constant d = new Constant();

            if (content != "Due Date")
            {
                String sql = "INSERT INTO " + d.tableProcess + "( " +
                        d.id + "," +
                        d.td_tms_id + "," +
                        d.date + "," +
                        d.spec + "," +
                        d.colour + ",";
                sql += d.type + ",";
                sql += d.project_id + ",";
                sql += d.name + " " +
                ")SELECT COALESCE(MAX(" + d.id + "::Integer),0)+1," +
                "'" + id + "'," +
                "'" + date + "', " +
                "'" + spec + "', " +
                "'" + colour + "', ";
                if (tdtmsid != "0") sql += "'Detail',";
                else sql += "'-',";
                sql += "'" + tdtmsid + "', ";
                sql += "'" + content + "' " +
                            " FROM " + d.tableProcess;
                Database db = new Database(sql, _server);
                db.Close();

            }
            else
            {
                Constant d2 = new Constant();
                var date2 = date.Substring(6,2)+"-"+date.Substring(4,2)+"-"+date.Substring(0,4);

                string sql2 = "UPDATE " + d2.table;
                if (tdtmsid != "0")
                {
                    sql2 = "UPDATE " + d2.tableDetail;
                    sql2 += " SET " + d2.duedate + "='" + date2 + "' ";
                }
                else
                {
                    sql2 += " SET( " + d2.duedate + ","+d2.duedate2+")=('" + date2 + "', ";
                    sql2 += d2.duedate2+"||'_"+date2+"') ";
                }

                sql2 += " WHERE " + d2.id + "= '" + id + "' ";
                Database db2 = new Database(sql2, _server);
                db2.Close();



            }
                

            if (content == d.final_process)
            {
                Constant d2 = new Constant();

                string sql2 = "UPDATE " + d2.table;
                if (tdtmsid != "0")
                    sql2 = "UPDATE " + d2.tableDetail;
                sql2 += " SET " + d2.status + "='"+d.final_process+"' " +
                       " WHERE " + d2.id + "= '" + id + "' ";
                Database db2 = new Database(sql2, _server);
                db2.Close();

            }


        }

        public void DeleteProcess(
            int id, 
            String date, 
            String content, 
            String td_tms_id)
        {

                Constant d = new Constant();
                string sql = "DELETE FROM " + d.tableProcess +
                             " WHERE " + d.td_tms_id + "= '" + id + "' ";
                sql += " AND " + d.date + "= '" + date + "' ";
                sql += " AND " + d.name + "= '" + content + "' ";
                if (td_tms_id != "0")
                {
                    sql += " AND " + d.project_id + "='" + td_tms_id + "' ";
                    sql += " AND " + d.type + "='Detail' ";
                }
                Database db = new Database(sql, _server);
                db.Close();

                if (content == d.final_process)
                {
                    Constant d2 = new Constant();

                    string sql2 = "UPDATE " + d2.table;
                    if (td_tms_id != "0")
                        sql2 = "UPDATE " + d2.tableDetail;
                    sql2 +=" SET " + d2.status + "='-' " +
                        " WHERE " + d2.id + "= '" + id + "' ";
                    Database db2 = new Database(sql2, _server);
                    db2.Close();

                }
        }


        public IActionResult Csv()
        {
            DataModel list = new DataModel();
            var builder = new StringBuilder();
            builder.AppendLine("Id,Name");
            foreach(var data in list.ListModel)
            {
                builder.AppendLine($"{data.Id},{data.name}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "users.csv");
        }
        
        public IActionResult Excel(String StartDate, String EndDate, String Url, String td_tms_id)
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

            var nDay = (enddate - startdate).Days;

            var today_ = DateTime.Now.AddDays(0).ToString("yyyyMMdd");

            using (var workbook =  new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("TMS Data");

                var currentRow = 1;
                String[] title = { "ID", "Task Name", "Peron InCharge", "Group", "Email", "Due Date", "Status", "Remark" };
                for (var i = 1; i <9 ; i++)
                {
                    worksheet.Cell(currentRow, i).Value = title[i-1];
                    worksheet.Cell(currentRow, i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, i).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    worksheet.Cell(currentRow, i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(currentRow, i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                }

                var n = 9;
                for (var i = 0; i <= nDay; i++)
                {
                    worksheet.Cell(currentRow, n).Value = startdate.AddDays(i).ToString("dd_MM");
                    worksheet.Cell(currentRow, n).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, n).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, n).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, n).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, n).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    worksheet.Cell(currentRow, n).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(currentRow, n).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    n++;
                }


                List<DataModel> model = ReadProcess(StartDate, EndDate, td_tms_id);
                //if (Url.Contains("Detail")) model = ReadProcess(StartDate, EndDate, td_tms_id);

                Constant d = new Constant();
                string table = d.table;
                if (Url.Contains("Detail")) table = d.tableDetail;

                string sql = "SELECT " +
                    d.id + "," +
                    d.name + "," +
                    d.pic + "," +
                    d.spec + "," +
                    d.email + "," +
                    d.duedate + "," +
                    d.status + "," +
                    d.remark + " " +
                    " FROM " + table;
                if (Url.Contains("Detail")) sql += " WHERE "+d.td_tms_id+ "='"+td_tms_id+"'";
                sql += " ORDER BY " + d.name + " ";

                Database db = new Database(sql, _server);
                if (db.data.HasRows)
                {
                    while (db.data.Read())
                    {
                        var _d = db.data[5].ToString().Split(sp, StringSplitOptions.RemoveEmptyEntries);
                        var _due = _d[2] + _d[1] + _d[0];

                        currentRow++;
                        for (var i = 1; i < 9; i++)
                        {
                            worksheet.Cell(currentRow, i).Value = db.data[i-1];
                            worksheet.Cell(currentRow, i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            worksheet.Cell(currentRow, i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            if (
                                i == 7 &&
                                int.Parse(today_) >= int.Parse(_due) &&
                                db.data[6].ToString().Trim() != d.final_process)
                            {
                                worksheet.Cell(currentRow, 7).Value = "overdue";
                                worksheet.Cell(currentRow, 7).Style.Font.FontColor = XLColor.Red;
                                worksheet.Cell(currentRow, 7).Style.Fill.BackgroundColor = XLColor.Yellow;
                            }
                        }
                        var m = 9;
                        for (var i = 0; i <= nDay; i++)
                        {


                            String PContent = "";
                            foreach (var process in model)
                            {
                                if (
                                    process.process_td_tms_id == db.data[0].ToString() &&
                                    process.process_date == startdate.AddDays(i).ToString("yyyyMMdd")
                                )
                                {
                                    if (PContent != "") PContent += Environment.NewLine;
                                    PContent += process.process_content;
                                    string hex = process.process_colour;
                                    Color _color = Color.FromArgb(int.Parse(hex.Replace("#", ""), System.Globalization.NumberStyles.AllowHexSpecifier));
                                    worksheet.Cell(currentRow, m).Style.Fill.BackgroundColor = XLColor.FromColor(_color);
                                }
                            }
                            if(startdate.AddDays(i).ToString("yyyyMMdd") == _due)
                            {
                                PContent += "[Due Date]";
                                worksheet.Cell(currentRow, m).Style.Font.FontColor = XLColor.Red;
                                worksheet.Cell(currentRow, m).Style.Fill.BackgroundColor = XLColor.Yellow;
                            }

                            worksheet.Cell(currentRow, m).Value = PContent;

                            worksheet.Cell(currentRow, m).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, m).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, m).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, m).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(currentRow, m).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            worksheet.Cell(currentRow, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                            m++;
                        }
                    }
                }
                db.Close();

                worksheet.Columns().Style.Font.FontSize = 12;
                worksheet.Columns().AdjustToContents();
                

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "TMS Data.xlsx");

                }
            }
        }

        public List<DataModel> ReadProcess(
            String StartDate, 
            String EndDate, 
            String td_tms_id)
        {

            char[] sp = { '-' };
            var sd = StartDate.Split(sp, StringSplitOptions.RemoveEmptyEntries);
            var ed = EndDate.Split(sp, StringSplitOptions.RemoveEmptyEntries);

            var start = sd[2] + sd[1] + sd[0];
            var end = ed[2] + ed[1] + ed[0];

            DataModel list = new DataModel();
            try
            {
                Constant d = new Constant();
                string sql = "SELECT " +
                    d.td_tms_id + "," +
                    d.date + "," +
                    d.name + "," +
                    d.colour + " " +
                    " FROM " + d.tableProcess;

                sql += " WHERE " + d.date + " >= '" + start + "' ";
                sql += " AND " + d.date + " <= '" + end + "' ";
                if (td_tms_id != null)
                {
                    sql += " AND " + d.project_id + "='" + td_tms_id + "' ";
                    sql += " AND " + d.type.Trim() + "='Detail' ";
                }
                else
                {
                    sql += " AND " + d.type + "!='Detail' ";
                }
                Database db = new Database(sql, _server);

                if (db.data.HasRows)
                {
                    while (db.data.Read())
                    {
                        list.ListModel.Add(new DataModel
                        {
                            process_td_tms_id = db.data[0].ToString(),
                            process_date = db.data[1].ToString(),
                            process_content = db.data[2].ToString(),
                            process_colour = db.data[3].ToString()
                        });

                    }
                }
                db.Close();
            }
            catch (Exception err)
            {

            }
            List<DataModel> model = list.ListModel.ToList();

            return model;
        }



        public List<Master> SearchPic()
        {
            Master list = new Master();

            Constant d = new Constant();
            string sqle = "SELECT DISTINCT "+d.pic+" FROM " + d.table;
            Database db = new Database(sqle, _server);
            if (db.data.HasRows)
            {
                while (db.data.Read())
                {
                    list.MasterModel.Add(new Master
                    {
                        name = db.data[0].ToString()
                    });

                }
            }
            db.Close();

            List<Master> model = list.MasterModel.ToList();

            return model;
        }


    }
}

