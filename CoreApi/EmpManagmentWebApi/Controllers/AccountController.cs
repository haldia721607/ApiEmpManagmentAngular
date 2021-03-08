using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmpManagmentWebApi.IServices;
using EmpManagmentWebApi.Models;
using EmpManagmentWebApi.Services;
using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiBOL.ViewModels.UserViewModels.ViewModels;
using EmpManagmentWebApiIBLL.AccountRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmpManagmentWebApi.Controllers
{
    //[EnableCors("AllowOrigin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserServices _iUserServices;
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger, IAccountRepository accountRepository, IUserServices userServices, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _accountRepository = accountRepository;
            _configuration = configuration;
            this._iUserServices = userServices;
        }

        [HttpPost("get", Name = "get")]
        public IActionResult Get()
        {
            return null;
        }

        [HttpPost("authenticate", Name = "authenticate")]
        public async Task<IActionResult> Authenticate(LoginViewModel loginViewModel)
        {
            if (loginViewModel.UserName != null && loginViewModel.Password != null)
            {
                var user = await _iUserServices.authenticate(loginViewModel);
                if (user == null)
                {
                    return BadRequest(new { message = "username or password is incorrect" });
                }
                else
                {
                    return Ok(user);
                }
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel signUpViewModel)
        {
            var user = await _iUserServices.Register(signUpViewModel);
            if (user == null)
                return BadRequest(new { message = "Invalid Data" });
            return Ok(user);
        }
  
        [HttpGet]
        [Route("country")]
        public IActionResult GetCountrylist()
        {
            var countryList = _accountRepository.CounteryList().ToList();
            if (countryList.Count >= 0)
            {
                return Ok(countryList);
            }
            return null;
        }

        [HttpGet]
        [Route("state")]
        public IActionResult GetStatelist(int countryId)
        {
            var states = _accountRepository.StateList(countryId);
            if (states != null)
            {

                return Ok(states);
            }
            return BadRequest(null);
        }

        [HttpGet]
        [Route("city")]
        public IActionResult GetCitylist(int stateId)
        {
            var cities = _accountRepository.CityList(stateId);
            if (cities != null)
            {
                return Ok(cities);
            }
            return BadRequest(null);
        }

        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> IsEmailInUse(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json($"Email {email} is already in use.");
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> ConfirmEmail(string userId, string token)
        //{
        //    if (userId == null && token == null)
        //    {
        //        return RedirectToAction("Register", "Account", new { area = "Comman" });
        //    }
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
        //        return View("NotFound");
        //    }
        //    var result = await _userManager.ConfirmEmailAsync(user, token);
        //    if (result.Succeeded)
        //    {
        //        return View();
        //    }
        //    ViewBag.ErrorTitle = "Email cannot be confirmed";
        //    return View("Error");
        //}
        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Login", "Account", new { Area = "Comman" });
        //}
        //[HttpGet]
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Find the user by email
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        // If the user is found AND Email is confirmed
        //        if (user != null && await _userManager.IsEmailConfirmedAsync(user))
        //        {
        //            // Generate the reset password token
        //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //            // Build the password reset link
        //            var passwordResetLink = Url.Action("ResetPassword", "Account", new { area = "Comman", email = model.Email, token = token }, Request.Scheme);
        //            string Ref = "PasswordResetLink";
        //            int isSendSuccess = CommanFunction.SendConfirmationLink(passwordResetLink, user.UserName, model.Email, Ref);
        //            if (isSendSuccess == 1)
        //            {
        //                return View("ForgotPasswordConfirmation");
        //            }
        //            //Before you can Login, please reset your password, by clicking on the reset password link we have emailed you on your email-{model.Email} id.
        //            // Log the password reset link
        //            _logger.Log(LogLevel.Warning, passwordResetLink);

        //            // Send the user to Forgot Password Confirmation view
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        // To avoid account enumeration and brute force attacks, don't
        //        // reveal that the user does not exist or is not confirmed
        //        return View("ForgotPasswordConfirmation");
        //    }

        //    return View(model);
        //}
        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResetPassword(string token, string email)
        //{
        //    // If password reset token or email is null, most likely the
        //    // user tried to tamper the password reset link
        //    if (token == null || email == null)
        //    {
        //        ModelState.AddModelError("", "Invalid password reset token");
        //    }
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Find the user by email
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user != null)
        //        {
        //            // reset the user password
        //            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        //            if (result.Succeeded)
        //            {
        //                // Upon successful password reset and if the account is lockedout, set
        //                // the account lockout end date to current UTC date time, so the user
        //                // can login with the new password
        //                if (await _userManager.IsLockedOutAsync(user))
        //                {
        //                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
        //                }
        //                ViewBag.Success = "Reset password successful";
        //                ViewBag.Message = $"You have successfully reset your passowd.Please loging with email-{model.Email} with your new password.";
        //                return View("PasswordResetSuccessful");
        //            }
        //            // Display validation errors. For example, password reset token already
        //            // used to change the password or password complexity rules not met
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }
        //            return View(model);
        //        }

        //        // To avoid account enumeration and brute force attacks, don't
        //        // reveal that the user does not exist
        //        ViewBag.Success = "Reset password successful";
        //        ViewBag.Message = $"You have successfully reset your passowd.Please loging with registered email id with your new password.";
        //        return View("PasswordResetSuccessful");
        //    }
        //    // Display validation errors if model state is not valid
        //    return View(model);
        //}
        //[HttpGet]
        //public IActionResult AccessDenied()
        //{
        //    return View();
        //}
    }
}
