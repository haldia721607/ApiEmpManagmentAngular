using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels;
using EmpManagmentWebApiDAL.DbContextClass;
using EmpManagmentWebApiIBLL.EmployeeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmpManagmentWebApiBLL.Employee
{
    public class EmployeeBll : EmployeeRepository
    {
        private readonly EmployeeDbContext DbContext;
        public EmployeeBll(EmployeeDbContext employeeDbContext)
        {
            DbContext = employeeDbContext;
        }
        public bool AddEmployee(EmployeeViewModel model)
        {
            bool isSavedSucess = false;
            if (model.FileAsBase64.Contains(","))
            {
                model.FileAsBase64 = model.FileAsBase64.Substring(model.FileAsBase64.IndexOf(",") + 1);
            }
            model.Data = Convert.FromBase64String(model.FileAsBase64);

            EmpManagmentWebApiBOL.Tables.Employee employee = new EmpManagmentWebApiBOL.Tables.Employee();
            employee.Name = model.Name;
            employee.Address = model.Address;
            employee.PinCode = model.PinCode;
            employee.GenderId = Convert.ToInt32(model.GenderId);
            employee.CountryId = Convert.ToInt32(model.CountryId);
            employee.StateId = Convert.ToInt32(model.StateId);
            employee.CityId = Convert.ToInt32(model.CityId);
            employee.FileName = Guid.NewGuid().ToString() + "_" + model.FileName;
            employee.ContentType = model.ContentType;
            employee.FileExtension = ".pdf";
            employee.Data = model.Data;
            DbContext.Employees.Add(employee);
            DbContext.SaveChanges();
            isSavedSucess = true;
            return isSavedSucess;
        }

        public IEnumerable<Country> CounteryList()
        {
            var allCountries = DbContext.Countries.Where(x => x.Status == true).ToList();
            if (allCountries != null)
            {
                return allCountries;
            }
            return null;
        }
        public IEnumerable<State> StateList(int countryId)
        {
            var getStateByCountry = DbContext.States.Where(x => x.CountryId == countryId && x.Status == true).ToList();
            if (getStateByCountry != null)
            {
                return getStateByCountry;
            }
            return null;
        }
        public IEnumerable<City> CityList(int stateId)
        {
            var getCityByState = DbContext.Cities.Where(x => x.StateId == stateId && x.Status == true).ToList();
            if (getCityByState != null)
            {
                return getCityByState;
            }
            return null;
        }

        public List<EmployeeListViewModel> EmployeeList()
        {
            var getEmployee = (from emp in DbContext.Employees
                               join country in DbContext.Countries on emp.CountryId equals country.CountryId
                               join state in DbContext.States on emp.StateId equals state.StateId
                               join city in DbContext.Cities on emp.CityId equals city.CityId
                               join gender in DbContext.Genders on emp.GenderId equals gender.GenderId
                               select new
                               {
                                   emp.EmpId,
                                   emp.Name,
                                   emp.Address,
                                   emp.PinCode,
                                   country.CountryName,
                                   state.StateName,
                                   city.CityName,
                                   gender.GenderName,
                                   emp.FileName,
                                   emp.FileExtension,
                                   emp.ContentType,
                                   emp.Data
                               }).ToList();
            if (getEmployee.Count>0)
            {
                List<EmployeeListViewModel> employeeListViewModels = new List<EmployeeListViewModel>();
                foreach (var item in getEmployee)
                {
                    EmployeeListViewModel employeeListViewModel = new EmployeeListViewModel();
                    employeeListViewModel.Name = item.Name;
                    employeeListViewModel.EmpId = item.EmpId;
                    employeeListViewModel.Address = item.Address;
                    employeeListViewModel.PinCode = item.PinCode;
                    employeeListViewModel.CountryName = item.CountryName;
                    employeeListViewModel.StateName = item.StateName;
                    employeeListViewModel.CityName = item.CityName;
                    employeeListViewModel.GenderName = item.GenderName;
                    employeeListViewModel.FileName = item.FileName;
                    employeeListViewModel.FileExtension = item.FileExtension;
                    employeeListViewModel.ContentType = item.ContentType;
                    employeeListViewModel.Data = item.Data;
                    employeeListViewModels.Add(employeeListViewModel);
                }
                return employeeListViewModels;
            }
            return  null;
        }
        public IEnumerable<Gender> GenderList()
        {
            return DbContext.Genders.ToList();
        }
    }
}
