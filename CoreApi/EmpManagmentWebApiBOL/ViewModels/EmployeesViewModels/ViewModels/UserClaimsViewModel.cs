using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Cliams = new List<UserClaim>();
        }

        public string UserId { get; set; }
        public List<UserClaim> Cliams { get; set; }
    }
}
