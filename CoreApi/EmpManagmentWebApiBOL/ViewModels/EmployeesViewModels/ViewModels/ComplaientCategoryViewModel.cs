using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels
{
    public class ComplaientCategoryViewModel
    {
        public int ComplaientCategoryId { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string UserStatus { get; set; }
    }
}
