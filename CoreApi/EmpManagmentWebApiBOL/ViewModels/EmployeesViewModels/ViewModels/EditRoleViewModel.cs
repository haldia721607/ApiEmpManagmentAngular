using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<Users>();
        }

        public string Id { get; set; }

        public string RoleName { get; set; }

        public List<Users> Users { get; set; }
    }
    public class Users
    {
        public string  Name { get; set; }
    }
}
