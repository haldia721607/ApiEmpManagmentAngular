using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmpManagmentWebApiBOL.Tables
{
    [Table("tblEmployee")]
    public class Employee
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int EmpId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        [ForeignKey("Gender")]
        [Required]
        public int GenderId { get; set; }

        [ForeignKey("Country")]
        [Required]
        public int CountryId { get; set; }
        [ForeignKey("State")]
        [Required]
        public int StateId { get; set; }
        [ForeignKey("City")]
        [Required]
        public int CityId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public string FileExtension { get; set; }
        [DataType("varbinary(max)")]
        [Required]
        public Byte[] Data { get; set; }




        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        public virtual City City { get; set; }
        public virtual Gender Gender { get; set; }

    }
}


