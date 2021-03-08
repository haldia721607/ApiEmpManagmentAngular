using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmpManagmentWebApiBOL.Tables
{
    public class Complaients
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ComplaientId { get; set; }
        [Required]
        [MaxLength(200)]
        public string ComplainantName { get; set; }
        [Required]
        [MaxLength(200)]
        public string ComplainantEmail { get; set; }
        [Required]
        public bool ComplaientStatus { get; set; }
        public DateTime? CompaientDate { get; set; }
        public virtual ComplaientDetails ComplaientDetails { get; set; }
    }
}
