using EmpManagmentWebApiBOL.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels
{
    public class ComplainantViewModel
    {
        public ComplainantViewModel()
        {
            this.ListOfExcelFileDataSaveToDatabaseViewModel = new List<BulkDatas>();
            this.MultipleFileSaveToDatabaseViewModel = new List<FileViewModel>();
            this.MultipleFileSaveToDatabase = new List<IFormFile>();
            this.MultipleFileSaveToFolderViewModel = new List<FileViewModel>();
            this.MultipleFileSaveToFolder = new List<IFormFile>();
            this.MultipleImageSaveToDatabaseViewModel = new List<FileViewModel>();
            this.MultipleImageSaveToDatabase = new List<IFormFile>();
            this.MultipleImageSaveToFolderViewModel = new List<FileViewModel>();
            this.MultipleImageSaveToFolder = new List<IFormFile>();
            this.BikeCategoriesSelectList = new List<SelectListItem>();
            this.BikeCategories = new List<BikeCategory>();
            this.ComplaientCategory = new List<SelectListItem>();
            this.Gender = new List<SelectListItem>();
            this.Countries = new List<SelectListItem>();
            this.States = new List<SelectListItem>();
            this.Cities = new List<SelectListItem>();
        }
        /// <summary>
        /// Complaient tbl
        /// </summary>
        public int ComplaientId { get; set; }
        public string ComplaientEncryptedId { get; set; }
        public string ComplainantName { get; set; }
        public string ComplainantEmail { get; set; }
        public DateTime? CompaientDate { get; set; }

        /// <summary>
        /// Complaient Detail tbl
        /// </summary>
        public int ComplaientDetailsId { get; set; }
        public string ComplaientDetailsEncryptedId { get; set; }
        public int ComplaientCategoryId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Description { get; set; }
        public DateTime? ComplaientDate { get; set; }

        /// <summary>
        /// PermamentAddress Tbl
        /// </summary>
        public int ComplaientPermamentAddressId { get; set; }
        public string ComplaientPermamentAddressEncryptedId { get; set; }
        public string PermamentAddress { get; set; }
        public string PermamentAddressPostalCode { get; set; }

        /// <summary>
        /// TempAddress Tbl
        /// </summary>
        public int ComplaientTempAddressId { get; set; }
        public string ComplaientTempAddressEncryptedId { get; set; }
        public string TempAddress { get; set; }
        public string TempAddressPostalCode { get; set; }
        public bool TermsAndConditions { get; set; }
        public List<SelectListItem> ComplaientCategory { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Cities { get; set; }
        //[Required(AllowEmptyStrings =false,ErrorMessage ="Please select categories option")]
        //public CategoriesOptions? CategoriesOptions { get; set; }
        public List<BikeCategory> BikeCategories { get; set; }
        public List<SelectListItem> BikeCategoriesSelectList { get; set; }
        public List<int> BikeCategoriesSelectListId { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please select gender option")]
        //public GenderOptions? GenderOptions { get; set; }
        public List<SelectListItem> Gender { get; set; }
        public int GenderId { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }

        public IFormFile SingleImageSaveToFolder { get; set; }
        public List<IFormFile> MultipleImageSaveToFolder { get; set; }
        public FileViewModel SingleImageSaveToFolderViewModel { get; set; }
        public List<FileViewModel> MultipleImageSaveToFolderViewModel { get; set; }

        public IFormFile SingleImageSaveToDatabase { get; set; }
        public List<IFormFile> MultipleImageSaveToDatabase { get; set; }
        public FileViewModel SingleImageSaveToDatabaseViewModel { get; set; }
        public List<FileViewModel> MultipleImageSaveToDatabaseViewModel { get; set; }

        public IFormFile SingleFileSaveToFolder { get; set; }
        public List<IFormFile> MultipleFileSaveToFolder { get; set; }
        public FileViewModel SingleFileSaveToFolderViewModel { get; set; }
        public List<FileViewModel> MultipleFileSaveToFolderViewModel { get; set; }

        public IFormFile SingleFileSaveToDatabase { get; set; }
        public List<IFormFile> MultipleFileSaveToDatabase { get; set; }
        public FileViewModel SingleFileSaveToDatabaseViewModel { get; set; }
        public List<FileViewModel> MultipleFileSaveToDatabaseViewModel { get; set; }

        public IFormFile ExcelFileDataSaveToDatabase { get; set; }
        public FileViewModel ExcelFileDataSaveToDatabaseViewModel { get; set; }
        public List<BulkDatas> ListOfExcelFileDataSaveToDatabaseViewModel { get; set; }
    }
}
