using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.UserViewModels.CustomValidation
{
    public class ValidEmailDomain : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidEmailDomain(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }
        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');
            return strings[1].ToUpper() == allowedDomain.ToUpper();
        }
    }
}
