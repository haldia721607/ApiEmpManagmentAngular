using EmpManagmentWebApiBOL.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagmentWebApiIBLL.AccountRepository
{
    public interface IAccountRepository
    {
        IEnumerable<Country> CounteryList();
        IEnumerable<State> StateList(int counteryId);
        IEnumerable<City> CityList(int stateId);
    }
}
