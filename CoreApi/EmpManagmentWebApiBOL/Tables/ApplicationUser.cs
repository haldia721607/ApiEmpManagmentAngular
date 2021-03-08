using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmpManagmentWebApiBOL.Tables
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        [Required]
        public virtual Country Country { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        [Required]
        public virtual State State { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        [Required]
        public virtual City City { get; set; }
        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}
