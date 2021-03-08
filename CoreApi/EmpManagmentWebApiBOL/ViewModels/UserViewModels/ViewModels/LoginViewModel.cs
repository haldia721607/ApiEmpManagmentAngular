using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.UserViewModels.ViewModels
{
    public class LoginViewModel
    {
        //[Required]
        //[EmailAddress]
        public string UserName { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = "Remember me")]
        //public bool RememberMe { get; set; }

        //public string ReturnUrl { get; set; }

        //// AuthenticationScheme is in Microsoft.AspNetCore.Authentication namespace
        //public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
