using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmpManagmentWebApi.IServices;
using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels;
using EmpManagmentWebApiBOL.ViewModels.UserViewModels.ViewModels;
using EmpManagmentWebApiIBLL.AccountRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmpManagmentWebApi.Areas.User.Controllers
{
    //[Authorize(Policy = "AdminRolePolicy")]
    //[EnableCors("AllowOrigin")]
    [ApiController]
    [Route("api/administration/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AdministrationController> logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserServices _iUserServices;
        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAccountRepository accountRepository, IUserServices userServices, ILogger<AdministrationController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._accountRepository = accountRepository;
            this._iUserServices = userServices;
            this.logger = logger;
        }

        [HttpPost]
        [Route("createroll")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            bool bSucceed = false;
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RollName
                };
                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    bSucceed = true;
                    return Ok(bSucceed);
                }
            }
            return BadRequest(bSucceed);
        }

        [HttpGet]
        [Route("rolls")]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles.OrderBy(x => x.Name);
            List<CreateRoleViewModel> createRoleViewModels = new List<CreateRoleViewModel>();
            foreach (var item in roles)
            {
                CreateRoleViewModel createRoleViewModel = new CreateRoleViewModel();
                createRoleViewModel.Id = item.Id;
                createRoleViewModel.RollName = item.Name;
                createRoleViewModels.Add(createRoleViewModel);
            }
            return Ok(createRoleViewModels);
        }

        [HttpGet]
        [Route("editUserById/{RollID}")]
        public async Task<IActionResult> EditRole(string RollID)
        {
            //Find role by id
            var role = await roleManager.FindByIdAsync(RollID);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id ={RollID} cannot be found";
                return View();
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            //List<Users> users = new List<Users>();
            // Retrieve all the Users
            foreach (var user in userManager.Users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    Users userInRoles = new Users();
                    userInRoles.Name = user.UserName;
                    model.Users.Add(userInRoles);
                }
            }

            return Ok(model);
        }

        // This action responds to HttpPost and receives EditRoleViewModel
        [HttpPost]
        //[Route("userArea/administrationController/editRolePost")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                // Update the Role using UpdateAsync
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration", new { area = "User" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        ////[Authorize(Policy = "DeleteRolePolicy")]
        //[Route("userArea/administrationController/deleteRole")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                // Wrap the code in a try/catch block
                try
                {
                    //throw new Exception("Test Exception");

                    var result = await roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("ListRoles");
                }
                // If the exception is DbUpdateException, we know we are not able to
                // delete the role as there are users in the role being deleted
                catch (DbUpdateException ex)
                {
                    //Log the exception to a file. We discussed logging to a file
                    // using Nlog in Part 63 of ASP.NET Core tutorial
                    logger.LogError($"Exception Occured : {ex}");
                    // Pass the ErrorTitle and ErrorMessage that you want to show to
                    // the user using ViewBag. The Error view retrieves this data
                    // from the ViewBag and displays to the user.
                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users from the role and then try to delete";
                    return View("Error");
                }
            }
        }


        ////[Authorize(Policy = "EditRolePolicy")]
        [HttpGet]
        [Route("editUsersInRoleByrollId/{roleId}")]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return BadRequest("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return Ok(model);
        }

        [HttpPost]
        [Route("editUsersInRoleByrollId/{userInRoleViewModel}/{roleId}")]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> userInRoleViewModel, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return BadRequest("NotFound");
            }
            for (int i = 0; i < userInRoleViewModel.Count; i++)
            {
                var user = await userManager.FindByIdAsync(userInRoleViewModel[i].UserId);
                IdentityResult result = null;
                if (userInRoleViewModel[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userInRoleViewModel[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (userInRoleViewModel.Count - 1))
                        continue;
                    else
                        break;
                }
            }
            return Ok(true);
        }

        [HttpGet]
        [Route("users/{userId}")]
        public async Task<IActionResult> Users(string userId)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var user = await userManager.FindByIdAsync(userId);
            List<UserListViewModes> userListViewModes = new List<UserListViewModes>();
            bool result = await userManager.IsInRoleAsync(user, "Super Admin");
            if (result)
            {
                var res = userManager.Users.ToList();
                if (res.Count > 0)
                {
                    foreach (var item in res)
                    {
                        UserListViewModes userListViewMode = new UserListViewModes();
                        userListViewMode.UserId = item.Id;
                        userListViewMode.Name = item.UserName;
                        userListViewModes.Add(userListViewMode);
                    }
                }
            }
            else
            {
                //Login user not see own data also super admin data
                //var hshs=userManager.Users.Where(u => u.Id != userId && u.Id != "8d7e126e-738b-4a9a-9de4-86b6aa26f2d1");
                var res = userManager.Users.ToList();
                if (res.Count > 0)
                {
                    foreach (var item in res)
                    {
                        UserListViewModes userListViewMode = new UserListViewModes();
                        userListViewMode.UserId = item.Id;
                        userListViewMode.Name = item.UserName;
                        userListViewModes.Add(userListViewMode);
                    }
                }

            }
            return Ok(userListViewModes);
        }

        [HttpGet]
        //[Authorize(Policy = "EditRolePolicy")]
        [Route("editUser/{userId}")]
        public async Task<IActionResult> EditUser(string userId)
        {
            EditUserViewModel editUserViewModel = new EditUserViewModel();
            //Find user by id
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return BadRequest("NotFound");
            }
            BindMasterData(editUserViewModel, user);
            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await userManager.GetRolesAsync(user);
            editUserViewModel.Id = user.Id;
            editUserViewModel.Email = user.Email;
            editUserViewModel.UserName = user.UserName;
            editUserViewModel.City = user.City;
            editUserViewModel.CountryId = user.CountryId;
            editUserViewModel.StateId = user.StateId;
            editUserViewModel.CityId = user.CityId;
            editUserViewModel.Claims = userClaims.Select(c => c.Value).ToList();
            editUserViewModel.Roles = userRoles;
            return Ok(editUserViewModel);
        }

        private void BindMasterData(EditUserViewModel editUserViewModel, ApplicationUser user)
        {
            //Bind Country
            var countryList = _accountRepository.CounteryList().ToList();
            if (countryList != null)
            {
                foreach (var item in countryList)
                {
                    Country country = new Country();
                    country.CountryId = item.CountryId;
                    country.CountryName = item.CountryName;
                    country.Status = item.Status;
                    editUserViewModel.Countries.Add(country);
                }
            }
            //Bind State
            var states = _accountRepository.StateList(user.CountryId);
            if (states != null)
            {
                foreach (var item in states)
                {
                    State state = new State();
                    state.StateId = item.StateId;
                    state.StateName = item.StateName;
                    state.Status = item.Status;
                    editUserViewModel.States.Add(state);
                }
            }
            //Bind City
            var cities = _accountRepository.CityList(user.StateId);
            if (cities != null)
            {
                foreach (var item in cities)
                {
                    City city = new City();
                    city.StateId = item.StateId;
                    city.CityId = item.CityId;
                    city.CityName = item.CityName;
                    city.Status = item.Status;
                    editUserViewModel.Cities.Add(city);
                }
            }
        }

        [HttpGet]

        //[Route("userArea/administrationController/getStateList")]
        public IActionResult GetStatelist(int countryId)
        {
            EditUserViewModel editUserViewModel = new EditUserViewModel();
            var states = _accountRepository.StateList(countryId);
            if (states != null)
            {
                foreach (var item in states)
                {
                    State state = new State();
                    state.StateId = item.StateId;
                    state.StateName = item.StateName;
                    state.Status = item.Status;

                    editUserViewModel.States.Add(state);
                }
                return Ok(editUserViewModel);
            }
            return null;
        }

        [HttpGet]

        //[Route("userArea/administrationController/getCityList")]
        public IActionResult GetCitylist(int stateId)
        {
            EditUserViewModel editUserViewModel = new EditUserViewModel();
            var cities = _accountRepository.CityList(stateId);
            if (cities != null)
            {
                foreach (var item in cities)
                {
                    City city = new City();
                    city.StateId = item.StateId;
                    city.CityId = item.CityId;
                    city.CityName = item.CityName;
                    city.Status = item.Status;
                    editUserViewModel.Cities.Add(city);
                }
                return Ok(editUserViewModel);
            }
            return null;
        }

        [HttpPost]
        //[Route("userArea/administrationController/editUserPost")]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.CountryId = model.CountryId;
                user.StateId = model.StateId;
                user.CityId = model.CityId;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                BindMasterData(model, user);
                return View(model);
            }
        }

        [HttpPost]

        //[Route("userArea/administrationController/deleteUser")]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Users");
            }
        }

        [HttpGet]
        [Route("manageUserRoles/{userId}")]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return BadRequest(ViewBag.ErrorMessage);
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return Ok(model);
        }

        [HttpPost]
        [Route("manageUserRoles/{model}/{userId}")]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return BadRequest(ViewBag.ErrorMessage);
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return BadRequest("Cannot remove user existing roles");
            }

            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View("Cannot add selected roles to user");
            }

            return Ok(userId);
        }

        [HttpGet]

        //[Route("userArea/administrationController/manageUserClaimsGet")]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            // UserManager service GetClaimsAsync method gets all the current claims of the user
            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            // Loop through each claim we have in our application
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                model.Cliams.Add(userClaim);
            }

            return View(model);

        }

        [HttpPost]

        //[Route("userArea/administrationController/manageUserClaimsPost")]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            // Get all the user existing claims and delete them
            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }

            // Add all the claims that are selected on the UI
            result = await userManager.AddClaimsAsync(user,
                model.Cliams.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = model.UserId });

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

        [Route("getUserByEmail/{Email}")]
        public async Task<IActionResult> GetUserByEmail(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            return Ok(user);
        }

        [Route("countries")]
        public IActionResult GetCountries()
        {
            //Bind Country
            var countryList = _accountRepository.CounteryList().ToList();
            List<Country> countries = new List<Country>();
            if (countryList != null)
            {
                foreach (var item in countryList)
                {
                    Country country = new Country();
                    country.CountryId = item.CountryId;
                    country.CountryName = item.CountryName;
                    country.Status = item.Status;
                    countries.Add(country);
                }
            }
            return Ok(countries);
        }
        [Route("states/{countryId}")]
        public IActionResult Getstates(string countryId)
        {
            //Bind Country
            var stateList = _accountRepository.StateList(Convert.ToInt32(countryId)).ToList();
            List<State> states = new List<State>();
            if (stateList != null)
            {
                foreach (var item in stateList)
                {
                    State state = new State();
                    state.StateId = item.StateId;
                    state.StateName = item.StateName;
                    state.Status = item.Status;
                    state.CountryId = item.CountryId;
                    states.Add(state);
                }
            }
            return Ok(states);
        }
        [Route("cities/{stateId}")]
        public IActionResult GetCites(string stateId)
        {
            //Bind Country
            var CityList = _accountRepository.CityList(Convert.ToInt32(stateId)).ToList();
            if (CityList != null)
            {
                List<City> cites = new List<City>();
                if (CityList != null)
                {
                    foreach (var item in CityList)
                    {
                        City city = new City();
                        city.CityId = item.CityId;
                        city.CityName = item.CityName;
                        city.Status = item.Status;
                        city.StateId = item.StateId;
                        cites.Add(city);
                    }
                }
                return Ok(cites);
            }
            return Ok(null);
        }
    }
}
