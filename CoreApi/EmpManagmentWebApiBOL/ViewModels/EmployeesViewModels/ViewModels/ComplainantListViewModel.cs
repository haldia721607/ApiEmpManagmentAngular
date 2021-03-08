using EmpManagmentWebApiBOL.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels
{
    public class ComplainantListViewModel
    {
        public IEnumerable<ComplainantAndDetailsViewModel> complainantAndDetailsViewModels { get; set; }
        public IEnumerable<BikeCategory> BikeCategory { get; set; }
    }
}
