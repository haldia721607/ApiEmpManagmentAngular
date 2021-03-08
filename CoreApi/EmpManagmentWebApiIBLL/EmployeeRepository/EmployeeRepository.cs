using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiIBLL.EmployeeRepository
{
    public interface EmployeeRepository
    {
        List<EmployeeListViewModel> EmployeeList();
        bool AddEmployee(EmployeeViewModel model);
        IEnumerable<Country> CounteryList();
        IEnumerable<State> StateList(int counteryId);
        IEnumerable<City> CityList(int stateId);
        IEnumerable<Gender> GenderList();
    }
}
