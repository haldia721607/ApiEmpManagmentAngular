using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.UserViewModels.ViewModels
{
    public class UserList
    {
        public UserList()
        {
            this.UserListViewModes = new List<UserListViewModes>();
        }
        public string UserId { get; set; }
        public List<UserListViewModes> UserListViewModes { get; set; }
    }
}
