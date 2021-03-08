using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmpManagmentWebApiBOL.Tables
{
    public class ComplaientPermamentAddress
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ComplaientPermamentAddressId { get; set; }
        [ForeignKey("ComplaientDetails")]
        public int ComplaientDetailsId { get; set; }
        [Required]
        public virtual ComplaientDetails ComplaientDetails { get; set; }
        [Required]
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(6)]
        public int PostalCode { get; set; }
    }
}
