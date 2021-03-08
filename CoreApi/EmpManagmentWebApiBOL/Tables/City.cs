using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmpManagmentWebApiBOL.Tables
{
    public class City
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CityId { get; set; }
        [ForeignKey("State")]
        public int StateId { get; set; }
        [Required]
        [MaxLength(200)]
        public string CityName { get; set; }
        public bool Status { get; set; }

    }
}
