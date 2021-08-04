using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class CreateModel
    {
        public List<SelectListItem> ListEmail = new List<SelectListItem>();

        [DisplayName("Title")] //data annotation with the tab helper
        [Required(ErrorMessage = "You need to give Project Title")]
        public string name { get; set; }

        [DisplayName("PIC")]
        [Required(ErrorMessage = "You need to give Person Incharge")]
        public string pic { get; set; }
        public string pic2 { get; set; } // to get the pic name rather than the email and spec in its value in the select option
        
        [DisplayName("")]
        public string pic3 { get; set; }
        public string collaborate { get; set; }
        public string spec2 { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "You need email for your Project")]
        public string email { get; set; }

        
        public string email2 { get; set; }

        [DisplayName("Group")]
        [Required(ErrorMessage = "You need Group for your Project")]
        public string spec { get; set; }

        [DisplayName("Start Date")]
        [Required(ErrorMessage = "You need Start Date for your Project")]
        public string registerdate { get; set; }

        [DisplayName("Due Date")]
        [Required(ErrorMessage = "You need Due Date for your Project")]
        public string duedate { get; set; }
        public string duedate2 { get; set; }

        [DisplayName("Remark")]
        public string remark { get; set; }

        [DisplayName("ID")]
        public string td_tms_id { get; set; }
    }
}
