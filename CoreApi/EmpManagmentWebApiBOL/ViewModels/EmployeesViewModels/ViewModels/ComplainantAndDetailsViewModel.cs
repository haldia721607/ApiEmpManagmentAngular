using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels
{
    public class ComplainantAndDetailsViewModel
    {
        public int ComplaientId { get; set; }
        public string ComplaientEncryptedId { get; set; }
        public string ComplainantName { get; set; }
        public string ComplainantEmail { get; set; }
        public DateTime? CompaientDate { get; set; }
        public string ComplaientCategoriesDescription { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string ComplaientDescription { get; set; }
        public DateTime? ComplaientDate { get; set; }
        public string ShowComplaientDate { get; set; }
    }
}
