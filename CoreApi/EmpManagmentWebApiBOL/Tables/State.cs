using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmpManagmentWebApiBOL.Tables
{
    public class State
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int StateId { get; set; }
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        [Required]
        [MaxLength(200)]
        public string StateName { get; set; }
        public bool Status { get; set; }
    }
}
