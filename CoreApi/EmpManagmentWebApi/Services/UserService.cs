using EmpManagmentWebApi.IServices;
using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiBOL.ViewModels.UserViewModels.ViewModels;
using EmpManagmentWebApiDAL.DbContextClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmpManagmentWebApi.Services
{
    public class UserService : IUserServices
    {
        private AppSettings _appSettings;
        public UserManager<ApplicationUser> userManager;
        public SignInManager<ApplicationUser> signInManager;
        private ILogger<UserService> _logger;
        private IConfiguration _configuration;
        private EmployeeDbContext _db;
        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<UserService> logger, IConfiguration configuration, IOptions<AppSettings> appSettings, EmployeeDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _appSettings = appSettings.Value; 
            this._db = db;
        }

        public async Task<ApplicationUser> authenticate(LoginViewModel loginViewModel)
        {
            var user = await userManager.FindByNameAsync(loginViewModel.UserName);
            if (user!=null)
            {
                var result = await signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    var applicationUser = await userManager.FindByNameAsync(loginViewModel.UserName);
                    applicationUser.PasswordHash = null;
                    if (await this.userManager.IsInRoleAsync(applicationUser, "Admin"))
                    {
                        applicationUser.Role = "Admin";
                    }
                    else if (await this.userManager.IsInRoleAsync(applicationUser, "Employee"))
                    {
                        applicationUser.Role = "Employee";
                    }

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, applicationUser.Id),
                        new Claim(ClaimTypes.Email, applicationUser.Email),
                        new Claim(ClaimTypes.Role, applicationUser.Role)
                    }),
                        Expires = DateTime.UtcNow.AddHours(8),
                        SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    applicationUser.Token = tokenHandler.WriteToken(token);

                    return applicationUser;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<ApplicationUser> Register(RegisterViewModel signUpViewModel)
        {
            var applicationUser = new ApplicationUser();
            applicationUser.UserName = signUpViewModel.Name;
            applicationUser.Email = signUpViewModel.Email;
            applicationUser.CountryId = signUpViewModel.CountryId;
            applicationUser.StateId = signUpViewModel.StateId;
            applicationUser.CityId = signUpViewModel.CityId;
            applicationUser.Role = "Employee";

            var result = await userManager.CreateAsync(applicationUser, signUpViewModel.Password);
            if (result.Succeeded)
            {
                if ((await userManager.AddToRoleAsync(await userManager.FindByNameAsync(signUpViewModel.Email), "Employee")).Succeeded)
                {
                    var result2 = await signInManager.PasswordSignInAsync(signUpViewModel.Email, signUpViewModel.Password, false, false);
                    if (result2.Succeeded)
                    {
                        //token
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
                        var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
                        {
                            Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, applicationUser.Id),
                        new Claim(ClaimTypes.Email, applicationUser.Email),
                        new Claim(ClaimTypes.Role, applicationUser.Role)
                    }),
                            Expires = DateTime.UtcNow.AddHours(8),
                            SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        applicationUser.Token = tokenHandler.WriteToken(token);

                        return applicationUser;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

    }
}
