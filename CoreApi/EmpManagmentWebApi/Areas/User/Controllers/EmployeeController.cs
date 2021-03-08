using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels;
using EmpManagmentWebApiIBLL.EmployeeRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmpManagmentWebApi.Areas.User.Controllers
{
    [ApiController]
    [Route("api/employee/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;
        public EmployeeController(EmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }
        [HttpGet]
        [Route("employeeList")]
        public IActionResult EmployeeList()
        {
            List<EmployeeListViewModel> employeeList = _employeeRepository.EmployeeList();
            if (employeeList.Count > 0)
            {
                return Ok(employeeList);
            }
            else
            {
                return NotFound();
            }
        }

        // public IActionResult Country
        [HttpGet]
        [Route("country")]
        public IActionResult GetCountry()
        {
            IEnumerable<Country> country = _employeeRepository.CounteryList();
            return Ok(country);
        }
        [HttpGet]
        [Route("state/{countryId}")]
        public IActionResult GetStatelist(int countryId)
        {
            IEnumerable<State> state = _employeeRepository.StateList(countryId);
            return Ok(state);
        }
        [HttpGet]
        [Route("city/{stateId}")]
        public IActionResult GetCitylist(int stateId)
        {
            IEnumerable<City> city = _employeeRepository.CityList(stateId);
            return Ok(city);
        }
        [HttpGet]
        [Route("gender")]
        public IActionResult GetGenderlist()
        {
            IEnumerable<Gender> gender = _employeeRepository.GenderList();
            return Ok(gender);
        }


        [HttpPost]
        [Route("addemployee")]
        public IActionResult AddEmployee(EmployeeViewModel employee)
        {
            //employee.
            bool iInserted = _employeeRepository.AddEmployee(employee);
            if (iInserted)
            {
                return Ok(iInserted);
            }
            else
            {
                return BadRequest(iInserted);
            }
        }
    }
}
