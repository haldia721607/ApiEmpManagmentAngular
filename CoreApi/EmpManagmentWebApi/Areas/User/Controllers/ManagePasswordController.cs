using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmpManagmentWebApi.Areas.User.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Area("User")]
    //[EnableCors("AllowOrigin")]
    public class ManagePasswordController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public ManagePasswordController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        [Route("userArea/administrationController/changePasswordGet")]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User);

            var userHasPassword = await userManager.HasPasswordAsync(user);

            if (!userHasPassword)
            {
                //return RedirectToAction("AddPassword");
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("userArea/administrationController/changePasswordPost")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    //return RedirectToAction("Login");
                    return BadRequest();
                }

                // ChangePasswordAsync changes the user password
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    //foreach (var error in result.Errors)
                    //{
                    //    ModelState.AddModelError(string.Empty, error.Description);
                    //}
                    return BadRequest();
                }

                // Upon successfully changing the password refresh sign-in cookie
                await signInManager.RefreshSignInAsync(user);
                //return View("ChangePasswordConfirmation");
                return Ok();
            }

            return View(model);
        }

        [HttpGet]
        [Route("userArea/administrationController/addPasswordGet")]
        public async Task<IActionResult> AddPassword()
        {
            var user = await userManager.GetUserAsync(User);

            var userHasPassword = await userManager.HasPasswordAsync(user);

            if (userHasPassword)
            {
                //return RedirectToAction("ChangePassword");
                return Ok();
            }

            return Ok();
        }

        [HttpPost]
        [Route("userArea/administrationController/addPasswordPost")]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                var result = await userManager.AddPasswordAsync(user, model.NewPassword);

                if (!result.Succeeded)
                {
                    //foreach (var error in result.Errors)
                    //{
                    //    ModelState.AddModelError(string.Empty, error.Description);
                    //}
                    return BadRequest();
                }
                await signInManager.RefreshSignInAsync(user);
                //return View("AddPasswordConfirmation");
                return Ok();
            }
            return BadRequest(model);
        }
    }
}
