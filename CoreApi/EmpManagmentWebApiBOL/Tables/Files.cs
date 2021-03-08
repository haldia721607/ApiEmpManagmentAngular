using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmpManagmentWebApiBOL.Tables
{
    public class Files
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("ComplaientDetails")]
        public int ComplaientDetailsId { get; set; }
        [Required]
        public virtual ComplaientDetails ComplaientDetails { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string FileEncodingTypes { get; set; }
        public string FileStoreMode { get; set; }
        public string Path { get; set; }
        [DataType("varbinary(max)")]
        public Byte[] Data { get; set; }
    }
}
