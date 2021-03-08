using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmpManagmentWebApiBOL.Tables
{
    public class BikeCollection
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BikeCollectionId { get; set; }

        [ForeignKey("ComplaientDetails")]
        public int ComplaientDetailsId { get; set; }
        [Required]
        public virtual ComplaientDetails ComplaientDetails { get; set; }
        [ForeignKey("BikeCategory")]
        public int BikeCategoryId { get; set; }
        [Required]
        public virtual BikeCategory BikeCategory { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
