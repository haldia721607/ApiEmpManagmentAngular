using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmpManagmentWebApiBOL.Tables
{
    public class Gender
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GenderId { get; set; }
        [Required]
        [MaxLength(200)]
        public string GenderName { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
