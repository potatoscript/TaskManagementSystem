using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class DataModel
    {
        public List<DataModel> ListModel = new List<DataModel>();

        public List<SelectListItem> ListEmail = new List<SelectListItem>();


        public string process_td_tms_id { get; set; }
        public string process_date { get; set; }
        public string process_content { get; set; }
        public string process_colour { get; set; }


        [DisplayName("Start")]
        public string StartDate { get; set; }
        [DisplayName("End")]
        public string EndDate { get; set; }
        public int nDay { get; set; }

        public string status { get; set; }
        public string status_done { get; set; }


        [Key]
        public int Id { get; set; }


        [DisplayName("Title")] //data annotation with the tab helper
        [Required(ErrorMessage = "You need to give Project Title")]
        public string name { get; set; }
        
        [DisplayName("PIC")]
        [Required(ErrorMessage = "You need to give Person Incharge")]
        public string pic { get; set; }

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
