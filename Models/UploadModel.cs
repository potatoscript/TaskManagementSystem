using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class UploadModel
    {


        public List<String> ListFile = new List<String>();
        public string doc { get; set; }
        public string taskname { get; set; }
        public Boolean confirm { get; set; }

        public IFormFile MyFile { get; set; }
    }
}
