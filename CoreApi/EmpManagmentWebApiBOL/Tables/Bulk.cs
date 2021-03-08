using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmpManagmentWebApiBOL.Tables
{
    public class Bulk
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BulkId { get; set; }
        [ForeignKey("ComplaientDetails")]
        public int ComplaientDetailsId { get; set; }
        [Required]
        public virtual ComplaientDetails ComplaientDetails { get; set; }
        public string FileStoreMode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifyDate { get; set; }

    }
}
