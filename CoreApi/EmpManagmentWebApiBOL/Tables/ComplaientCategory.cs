using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace EmpManagmentWebApiBOL.Tables
{
    public class ComplaientCategory
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ComplaientCategoryId { get; set; }
        [Required]
        [MaxLength(500)]
        [Remote(action: "IsDescriptionInUse", controller: "Complaient", areaName: "User")]
        public string Description { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
