using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the TaskManagementUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "character varying(100)")]
        public string Name { get; set; }

        [PersonalData]
        [Column(TypeName = "character varying(100)")]
        public string Department { get; set; }
    }
}
