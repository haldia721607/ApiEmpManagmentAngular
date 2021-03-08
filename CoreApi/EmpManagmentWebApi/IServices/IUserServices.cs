using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiBOL.ViewModels.UserViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManagmentWebApi.IServices
{
    public interface IUserServices
    {
        Task<ApplicationUser> authenticate(LoginViewModel loginViewModel);
        Task<ApplicationUser> Register(RegisterViewModel signUpViewModel);
    }
}
