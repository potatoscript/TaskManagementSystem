using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using ProjectManagement;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class UploadController : Controller
    {
        public IConfiguration _server;
        private readonly IJSRuntime js;

        private readonly IHostingEnvironment hostingEnvironment;

        private String myPath;

        public UploadController(
            IConfiguration configuration, 
            IJSRuntime js,
            IHostingEnvironment environment)
        {
            _server = configuration;
            this.js = js;
            hostingEnvironment = environment;

            myPath = Path.Combine(hostingEnvironment.WebRootPath, "../../tms_doc");

        }

        public IActionResult Index()
        {
            return View();
        }


   
        public IActionResult Upload(
            String taskname,
            String StartDate,
            String EndDate,
            String page,
            String authorized
            )
        {

            // must replace the blank to something 
            string name = taskname.Replace(" ", "_");
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.page = page;
            ViewBag.taskname = name;
            ViewBag.authorized = authorized;

            UploadModel model = new UploadModel();
            Constant d = new Constant();
            string sql = "SELECT " + d.doc + " FROM " + d.tableDoc;
            sql += " WHERE " + d.taskname + " = '" +  name + "' ";
            sql += " ORDER BY " + d.doc + " ";

            Database db = new Database(sql, _server);

            if (db.data.HasRows)
            {
                while (db.data.Read())
                {
                   model.ListFile.Add(db.data[0].ToString().Trim());
                }
            }
            db.Close();

            ViewData["File"] = model.ListFile;

            return View(new UploadModel());
        }

        [HttpPost]
        public IActionResult Upload(
            String taskname,
            UploadModel model)
        {

                Constant d = new Constant();
                string sql0 = "SELECT " + d.doc + " FROM " + d.tableDoc;
                sql0 += " WHERE " + d.taskname + "= '" + taskname + "' ";
                sql0 += " ORDER BY " + d.doc + " ";
                Database db0 = new Database(sql0, _server);
                int existing_doc = 0;
                if (db0.data.HasRows)
                {
                    while (db0.data.Read())
                    {
                        if (db0.data[0].ToString().Contains(model.MyFile.FileName))
                        {
                            existing_doc = 1;


                        }
                    }
                }


                if (existing_doc == 0)
                {

                /*
                var fileName = System.IO.Path.GetFileName(model.MyFile.FileName);
                using (var localFile = System.IO.File.OpenWrite(fileName)) 
                using(var uploadedFile = model.MyFile.OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }
                */

                    //var uploads = Path.Combine(hostingEnvironment.WebRootPath,"doc");
                    String SavePath = Path.Combine(myPath, taskname + "_" + model.MyFile.FileName);
                    //String SavePath = Path.Combine(Directory.GetCurrentDirectory(),"doc", taskname + "_" + model.MyFile.FileName);
                   // String SavePath = Path.GetTempPath   ory(),"doc", taskname + "_" + model.MyFile.FileName);
                    /*
                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        model.MyFile.CopyTo(stream);
                    }
                    */
                    var stream = new FileStream(SavePath, FileMode.Create);
                    var str = model.MyFile.FileName;
                    try
                    {
                        model.MyFile.CopyTo(stream);
                        stream.Close();
                    }
                    catch (Exception e) {
                        str = e.ToString();
                        
                    }
                

                    String sql = "INSERT INTO " + d.tableDoc + "( " +
                                d.id + "," +
                                d.taskname + "," +
                                d.doc + " " +
                        ")SELECT COALESCE(MAX(" + d.id + "::Integer),0)+1," +
                        "'" + taskname + "', " +
                        "'" + str + "' " +
                        //"'" + SavePath + "' " +
                        " FROM " + d.tableDoc;
                        Database db = new Database(sql, _server);
                        db.Close();

                        return Json(new { isValid = true });
                }

            return Json(new {isValid = false});


        }


        [HttpPost]
        public IActionResult DeleteFile(
            bool confirm,
            String taskname,
            String doc
        )
        {
           if (ModelState.IsValid && confirm == true)
           {
                //Build the File Path
               // String ReadPath = Path.Combine(Directory.GetCurrentDirectory(),"doc", taskname+"_"+doc);
                
                String ReadPath = Path.Combine(myPath, taskname + "_" + doc);

                //Delete the file
                if (System.IO.File.Exists(ReadPath))
                {
                    System.IO.File.Delete(ReadPath);

                    Constant d = new Constant();
                    string sql = "DELETE FROM " + d.tableDoc;
                    sql += " WHERE " + d.taskname + "= '" + taskname + "' ";
                    sql += " AND " + d.doc + "= '" + doc + "' ";
                    Database db = new Database(sql, _server);
                    db.Close();

                    return Json(new { isValid = true });

                }

            }
            return Json(new { isValid = false });
        }

        [HttpGet("download")]
        public IActionResult Download(String fileName)
        {
            //Build the File Path
            //String ReadPath = Path.Combine(Directory.GetCurrentDirectory(),  "doc", fileName);
            
            String ReadPath = Path.Combine(myPath, fileName);

            /*
            //Read the File data into Byte Array
            byte[] bytes = System.IO.File.ReadAllBytes(ReadPath);

            //Send the File to Download
            return File(bytes, "application/octet-stream", fileName);
            */

            var net = new System.Net.WebClient();
            var data = net.DownloadData(ReadPath);
            var content = new System.IO.MemoryStream(data);

            return File(content, "application/octet-stream", fileName);

        }



        public List<DataModel> Read(String id)
        {

            DataModel list = new DataModel();

            Constant d = new Constant();
            string sql = "SELECT " + d.doc + " FROM " + d.table;
            sql += " WHERE " + d.id + " = '" + id + "' ";
            sql += " ORDER BY " + d.doc + " ";

            Database db = new Database(sql, _server);
            
            if (db.data.HasRows)
            {
                while (db.data.Read())
                {
                    var d_ = db.data[0].ToString().Split('#', StringSplitOptions.RemoveEmptyEntries);

                    for(int i=0; i < d_.Length; i++)
                    {
                        list.ListModel.Add(new DataModel
                        {
                            name = d_[i]
                        });
                    }
                    

                }
            }
            db.Close();


            List<DataModel> model = list.ListModel.ToList();

            return model;

        }



        [HttpGet("downloadmanual")]
        public IActionResult DownloadManual()
        {
            String ReadPath = Path.Combine(myPath, "manual.pptx");

            var net = new System.Net.WebClient();
            var data = net.DownloadData(ReadPath);
            var content = new System.IO.MemoryStream(data);

            return File(content, "application/octet-stream", "manual.pptx");

        }

    }

}