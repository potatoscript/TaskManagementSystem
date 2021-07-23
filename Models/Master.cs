using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class Master
    {

        public List<Master> MasterModel = new List<Master>();

        public List<SelectListItem> list=
            new List<SelectListItem>
            {
                new SelectListItem{Value = "#aae2e9",Text = "Blue"},
                new SelectListItem{Value = "#e3ae5b",Text = "Coco"},
                new SelectListItem{Value = "#d7d7d7",Text = "Grey"},
                new SelectListItem{Value = "#ccffcc",Text = "Green"},
                new SelectListItem{Value = "#ffd800",Text = "Orange"},
                new SelectListItem{Value = "#f6cffb",Text = "Pink"},
                new SelectListItem{Value = "#d7abff",Text = "Purple"},
                new SelectListItem{Value = "#ff9999",Text = "Red"},
                new SelectListItem{Value = "#ffffff",Text = "White"},
                new SelectListItem{Value = "#fff2a9",Text = "Yellow"}
            };

        [Key]
        public int Id { get; set; }

        [DisplayName("Name")] //data annotation with the tab helper
        [Required(ErrorMessage = "You need to give name")]
        public string name { get; set; }

        [DisplayName("Colour")]
        [Required(ErrorMessage = "You need to give colour")]
        public string colour { get; set; } 


        [DisplayName("Group")]
        [Required(ErrorMessage = "You need to give group")]
        public string spec { get; set; }


        
    }

}
