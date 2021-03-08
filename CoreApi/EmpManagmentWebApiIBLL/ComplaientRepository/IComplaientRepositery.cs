using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiIBLL.ComplaientRepository
{
    public interface IComplaientRepositery
    {
        IEnumerable<ComplaientCategoryViewModel> GetAllComplaientCategory();
        IEnumerable<ComplainantAndDetailsViewModel> GetAllComplaient(Microsoft.AspNetCore.DataProtection.IDataProtector protector);
        ComplaientCategory GetCategoryById(int complaientCategoryId);
        ComplaientCategory Delete(int complaientCategoryId);
        ComplaientCategory UpdateCategoryById(ComplaientCategory category);
        ComplaientCategory AddCategory(ComplaientCategory category);
        bool CategoryExisits(string description);
        bool MultipleCategoryDelete(List<int> ids);
        IEnumerable<BikeCategory> BikeCategory();
        IEnumerable<Gender> Genders();
        IEnumerable<ComplaientCategory> ComplaientCategoryList();
        IEnumerable<Country> CounteryList();
        IEnumerable<State> StateList(int counteryId);
        IEnumerable<City> CityList(int stateId);
        bool AddComplaient(ComplainantViewModel model);
        IEnumerable<BikeCategory> GetAllBikeCategory();
        ComplainantViewModel GetComplainantBiId(int id);
        FileViewModel DownloadFile(int id);
        List<BulkDatas> GetExcelData(string fileStoreMode);
        bool UpdateComplaient(ComplainantViewModel model);
        string GetFileNameById(int id);
        List<string> GetAllFiles(string fileStoreMode, int complaientDetailsId);
    }
}
