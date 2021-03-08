using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmpManagmentWebApi.Security;
using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.Enum;
using EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels;
using EmpManagmentWebApiIBLL.ComplaientRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ExcelDataReader;
using OfficeOpenXml;
using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpManagmentWebApi.Areas.User.Controllers
{
    [ApiController]
    [Area("User")]
    [Authorize]
    public class ComplaientController : Controller
    {
        private readonly IComplaientRepositery complaientRepo;
        private readonly IWebHostEnvironment hostingEnvironment;
        // It is through IDataProtector interface Protect and Unprotect methods,
        // we encrypt and decrypt respectively
        private readonly IDataProtector protector;

        // It is the CreateProtector() method of IDataProtectionProvider interface
        // that creates an instance of IDataProtector. CreateProtector() requires
        // a purpose string. So both IDataProtectionProvider and the class that
        // contains our purpose strings are injected using the contructor
        public ComplaientController(IComplaientRepositery complaientRepo, IWebHostEnvironment hostingEnvironment, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            this.complaientRepo = complaientRepo;
            this.hostingEnvironment = hostingEnvironment;
            // Pass the purpose string as a parameter
            this.protector = dataProtectionProvider.CreateProtector(
                dataProtectionPurposeStrings.ComplaientIdRouteValue, dataProtectionPurposeStrings.ComplaientDetailIdRouteValue, dataProtectionPurposeStrings.ComplaientCategoryIdRouteValue);
        }

        //Get All Category
        [HttpGet]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/listCategory")]
        public IActionResult ListCategory()
        {
            var getAllComplaientCategory = complaientRepo.GetAllComplaientCategory();
            return Ok(getAllComplaientCategory);
        }

        //Add Category Ajax Post Request
        [HttpPost]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/addCategory")]
        public IActionResult AddCategory(ComplaientCategory category)
        {
            if (ModelState.IsValid)
            {
                if (!complaientRepo.CategoryExisits(category.Description))
                {
                    ComplaientCategory complaientCategory = complaientRepo.AddCategory(category);
                    ViewBag.message = "Added";
                    //Clear the Model.
                    ModelState.Clear();
                }
                else
                {
                    ModelState.AddModelError("", "Category already exisits.");
                }
            }
            return Ok(category);
        }

        //Edit Category Ajax Request
        [HttpGet]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/editCategoryByIdGet")]
        public IActionResult EditCategory(int id)
        {
            ComplaientCategory complaientCategory = complaientRepo.GetCategoryById(id);
            ComplaientCategoryViewModel complaientCategoryViewModel = new ComplaientCategoryViewModel
            {
                ComplaientCategoryId = complaientCategory.ComplaientCategoryId,
                Description = complaientCategory.Description,
                Status = complaientCategory.Status,
                UserStatus = complaientCategory.Status == true ? "Active" : "Not Active"
            };
            return Ok(complaientCategoryViewModel);

        }

        //Edit Category Ajax Post Request
        [HttpPost]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/editCategoryByIdPost")]
        public IActionResult EditCategory(ComplaientCategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                ComplaientCategory complaientCategory = complaientRepo.GetCategoryById(category.ComplaientCategoryId);
                if (complaientCategory != null)
                {
                    complaientCategory.Description = category.Description;
                    complaientCategory.Status = category.Status;
                }
                ComplaientCategory updateCategory = complaientRepo.UpdateCategoryById(complaientCategory);
                ViewBag.message = "Update";
            }
            return Ok(category);
        }

        //Delete Category Ajax Post Request
        [HttpPost]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/deleteCategoryById")]
        public IActionResult Delete(int id)
        {
            ComplaientCategory result = complaientRepo.Delete(id);
            if (result!=null)
            {
                return Ok(result);
            }
            return null;
        }

        //Delete Multiple Category Using Ajax Post Request
        [HttpPost]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/multipleCategoryDelete")]
        public IActionResult MultipleCategoryDelete(List<int> ids)
        {
            bool result = complaientRepo.MultipleCategoryDelete(ids);
            if (result == true)
            {
                return Ok(new { message = "Selected category's deleted successfully." });
            }
            else
            {
                return BadRequest(new { message = "Selected category's can not delete !" });
            }
        }

        //Custom Validation for check duplicate Description Using remote validation 
        [AcceptVerbs("Get", "Post")]
        [Route("userArea/complaientController/isDescriptionInUse")]
        [AllowAnonymous]
        //[EnableCors("AllowOrigin")]
        public IActionResult IsDescriptionInUse(string description)
        {
            bool user = complaientRepo.CategoryExisits(description);
            if (user != true)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest($"{description} is already in use.");
            }
        }

        //Delete category function not implimented but id value getting form view to controller using ajax request 
        [HttpGet]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/deleteCategorie")]
        public IActionResult DeleteCategorie(int id)
        {
            bool result = true;
            if (result)
            {
                return Ok(new { success = true, message = "Deleted Successfully" });
            }
            else
            {
                return BadRequest(new { success = true, message = "Can not delete.Something went wrong" });
            }
        }

        //Delete multiple complaints function not implimented but id value getting form view to controller using ajax request 
        [HttpPost]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/multipleCategoriesDelete")]
        public IActionResult MultipleCategoriesDelete(List<int> ids)
        {
            bool result = true;
            if (result)
            {
                return Ok(new { success = true, message = "Selected categories's deleted successfully." });
            }
            else
            {
                return BadRequest(new { success = true, message = "Selected categories's can not delete !" });
            }
        }

        //Edit Bike Categories Using ajax request function not implimented
        [HttpGet]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/editBikeCategoriesByIdGet")]
        public IActionResult EditBikeCategories(int id)
        {
            BikeCategoriesViewModel bikeCategoriesViewModel = new BikeCategoriesViewModel()
            {
                Name = "Ravi",
                Description = "Ravi Shanker Pandey",
                Status = true
            };
            return Ok(bikeCategoriesViewModel);
        }

        //Create Bike Categories Get Request
        [HttpPost]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/createBikeCategoriesPost")]
        public IActionResult CreateBikeCategories(BikeCategoriesViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Ok(new { success = true, message = "Bike category create successfully." });
            }
            else
            {
                List<ErrorDetails> errorsList = new List<ErrorDetails>();
                var errorsResult = (from o in ModelState
                                    select new
                                    {
                                        Id = o.Key,
                                        Message = o.Value.Errors
                                    }).ToList();

                foreach (var item in errorsResult)
                {
                    ErrorDetails error = new ErrorDetails();
                    error.Key = item.Id.ToString();
                    error.ErrorMessage = item.Message[0].ErrorMessage;
                    errorsList.Add(error);
                }
                return BadRequest(new { success = false, message = "Please fill the form !", errorsList });
            }
        }


        //Get All Complaintes and bike category using ajax request
        [HttpGet]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/getAllComplaient")]
        public IActionResult GetAllComplaient()
        {
            ComplainantListViewModel complainantListViewModels = new ComplainantListViewModel();
            IEnumerable<ComplainantAndDetailsViewModel> getAllComplaient = complaientRepo.GetAllComplaient(protector);
            complainantListViewModels.complainantAndDetailsViewModels = getAllComplaient;
            IEnumerable<BikeCategory> bikeCategory = complaientRepo.GetAllBikeCategory();
            complainantListViewModels.BikeCategory = bikeCategory;
            return Ok(complainantListViewModels);
        }


        //Bind All Complaintes dropdown using ajax request
        [HttpGet]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/bindMasterData")]
        public ComplainantViewModel BindMasterData()
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            var countries = complaientRepo.CounteryList().ToList();
            var complaientCategory = complaientRepo.ComplaientCategoryList().ToList();
            var gender = complaientRepo.Genders().ToList();
            var bikeCategory = complaientRepo.BikeCategory().ToList();
            var bikeCategoriesSelectList = complaientRepo.BikeCategory().ToList();

            if (countries.Count > 0)
            {
                foreach (var item in countries)
                {
                    complainantViewModel.Countries.Add(new SelectListItem { Text = item.CountryName, Value = item.CountryId.ToString(), Selected = true });
                }
            }
            if (complaientCategory.Count > 0)
            {
                foreach (var item in complaientCategory)
                {
                    complainantViewModel.ComplaientCategory.Add(new SelectListItem { Text = item.Description, Value = item.ComplaientCategoryId.ToString(), Selected = true });
                }
            }
            if (gender.Count > 0)
            {
                foreach (var item in gender)
                {
                    complainantViewModel.Gender.Add(new SelectListItem { Text = item.GenderName, Value = item.GenderId.ToString(), Selected = true });
                }
            }
            if (bikeCategory.Count > 0)
            {
                complainantViewModel.BikeCategories = bikeCategory.Select(m => new BikeCategory()
                {
                    BikeCategoryId = m.BikeCategoryId,
                    Name = m.Name,
                    Description = m.Description,
                    Status = m.Status
                }).ToList();
            }
            if (bikeCategoriesSelectList.Count > 0)
            {
                foreach (var item in bikeCategoriesSelectList)
                {
                    complainantViewModel.BikeCategoriesSelectList.Add(new SelectListItem { Text = item.Name, Value = item.BikeCategoryId.ToString(), Selected = true });
                }
            }
            return complainantViewModel;
        }

        //Create Complaintes Get method
        [HttpGet]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/createcomplaientGet")]
        public IActionResult createcomplaient()
        {
            ComplainantViewModel complainantViewModel = BindMasterData();
            return Ok(complainantViewModel);
        }

        //Create Complaintes Post method
        [HttpPost]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/createcomplaientPost")]
        public IActionResult createcomplaient(ComplainantViewModel model)
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            complainantViewModel = BindMasterData();
            if (model.TermsAndConditions == false)
            {
                ModelState.AddModelError("", "Please check Terms & Conditions check box!");
                complainantViewModel.Error = "Please check Terms & Conditions check box!";
                return View(complainantViewModel);
            }
            if (ModelState.IsValid)
            {
                FileOpertaion(model);
                bool isSaved = complaientRepo.AddComplaient(model);
                if (isSaved == true)
                {
                    complainantViewModel.Message = "Added";
                    ModelState.Clear();
                }
            }
            return Ok(complainantViewModel);
        }

        //To save image folder 
        [Route("userArea/complaientController/fileOpertaion")]
        public ComplainantViewModel FileOpertaion(ComplainantViewModel model)
        {

            if (model.SingleImageSaveToFolder != null)
            {
                FileViewModel singleImageSaveToFolderViewModel = new FileViewModel();
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, Startup.StaticConfig["uploadImageFolder"]);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                singleImageSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleImageSaveToFolder.FileName;
                singleImageSaveToFolderViewModel.Path = Path.Combine(uploadFolder, singleImageSaveToFolderViewModel.Name);
                singleImageSaveToFolderViewModel.ContentType = model.SingleImageSaveToFolder.ContentType;
                singleImageSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleImageSaveToFolder);
                singleImageSaveToFolderViewModel.FileEncodingTypes = null;
                singleImageSaveToFolderViewModel.Data = null;
                model.SingleImageSaveToFolder.CopyTo(new FileStream(singleImageSaveToFolderViewModel.Path, FileMode.Create));
                model.SingleImageSaveToFolderViewModel = singleImageSaveToFolderViewModel;
            }

            if (model.MultipleImageSaveToFolder.Count > 0)
            {
                List<FileViewModel> listMultipleImageSaveToFolder = new List<FileViewModel>();
                foreach (IFormFile photo in model.MultipleImageSaveToFolder)
                {
                    FileViewModel listMultipleImageSaveToFolderViewModel = new FileViewModel();
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, Startup.StaticConfig["uploadImageFolder"]);
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    listMultipleImageSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    listMultipleImageSaveToFolderViewModel.Path = Path.Combine(uploadFolder, listMultipleImageSaveToFolderViewModel.Name);
                    listMultipleImageSaveToFolderViewModel.ContentType = photo.ContentType;
                    listMultipleImageSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToFolder);
                    listMultipleImageSaveToFolderViewModel.FileEncodingTypes = null;
                    listMultipleImageSaveToFolderViewModel.Data = null;
                    photo.CopyTo(new FileStream(listMultipleImageSaveToFolderViewModel.Path, FileMode.Create));
                    listMultipleImageSaveToFolder.Add(listMultipleImageSaveToFolderViewModel);
                }
                model.MultipleImageSaveToFolderViewModel = listMultipleImageSaveToFolder;
            }

            if (model.SingleImageSaveToDatabase != null)
            {
                FileViewModel fileSingleImageSaveToDatabaseViewModel = new FileViewModel();
                fileSingleImageSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleImageSaveToDatabase.FileName;
                fileSingleImageSaveToDatabaseViewModel.Path = null;
                fileSingleImageSaveToDatabaseViewModel.ContentType = model.SingleImageSaveToDatabase.ContentType;
                fileSingleImageSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleImageSaveToDatabase);
                fileSingleImageSaveToDatabaseViewModel.FileEncodingTypes = null;

                MemoryStream ms = new MemoryStream();
                model.SingleImageSaveToDatabase.CopyTo(ms);
                fileSingleImageSaveToDatabaseViewModel.Data = ms.ToArray();
                ms.Close();
                ms.Dispose();
                model.SingleImageSaveToDatabaseViewModel = fileSingleImageSaveToDatabaseViewModel;
            }

            if (model.MultipleImageSaveToDatabase.Count > 0)
            {
                List<FileViewModel> listMultipleImageSaveToDatabase = new List<FileViewModel>();
                foreach (IFormFile photo in model.MultipleImageSaveToDatabase)
                {
                    FileViewModel multipleImageSaveToDatabaseViewModel = new FileViewModel();
                    multipleImageSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleImageSaveToDatabaseViewModel.Path = null;
                    multipleImageSaveToDatabaseViewModel.ContentType = photo.ContentType;
                    multipleImageSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToDatabase);
                    multipleImageSaveToDatabaseViewModel.FileEncodingTypes = null;

                    MemoryStream ms = new MemoryStream();
                    photo.CopyTo(ms);
                    multipleImageSaveToDatabaseViewModel.Data = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                    listMultipleImageSaveToDatabase.Add(multipleImageSaveToDatabaseViewModel);
                }
                model.MultipleImageSaveToDatabaseViewModel = listMultipleImageSaveToDatabase;
            }

            if (model.SingleFileSaveToFolder != null)
            {
                FileViewModel singleFileSaveToFolderViewModel = new FileViewModel();
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, Startup.StaticConfig["uploadFileFolder"]);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                singleFileSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleFileSaveToFolder.FileName;
                singleFileSaveToFolderViewModel.Path = Path.Combine(uploadFolder, singleFileSaveToFolderViewModel.Name);
                singleFileSaveToFolderViewModel.ContentType = model.SingleFileSaveToFolder.ContentType;
                singleFileSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleFileSaveToFolder);
                singleFileSaveToFolderViewModel.FileEncodingTypes = null;
                singleFileSaveToFolderViewModel.Data = null;
                model.SingleFileSaveToFolder.CopyTo(new FileStream(singleFileSaveToFolderViewModel.Path, FileMode.Create));
                model.SingleFileSaveToFolderViewModel = singleFileSaveToFolderViewModel;
            }

            if (model.MultipleFileSaveToFolder.Count > 0)
            {
                List<FileViewModel> listMultipleFileSaveToFolderViewModel = new List<FileViewModel>();
                foreach (IFormFile photo in model.MultipleFileSaveToFolder)
                {
                    FileViewModel multipleFileSaveToFolderViewModel = new FileViewModel();
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, Startup.StaticConfig["uploadFileFolder"]);
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    multipleFileSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleFileSaveToFolderViewModel.Path = Path.Combine(uploadFolder, multipleFileSaveToFolderViewModel.Name);
                    multipleFileSaveToFolderViewModel.ContentType = photo.ContentType;
                    multipleFileSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToFolder);
                    multipleFileSaveToFolderViewModel.Data = null;
                    multipleFileSaveToFolderViewModel.FileEncodingTypes = null;
                    photo.CopyTo(new FileStream(multipleFileSaveToFolderViewModel.Path, FileMode.Create));
                    listMultipleFileSaveToFolderViewModel.Add(multipleFileSaveToFolderViewModel);
                }
                model.MultipleFileSaveToFolderViewModel = listMultipleFileSaveToFolderViewModel;
            }

            if (model.SingleFileSaveToDatabase != null)
            {
                FileViewModel singleFileSaveToDatabaseViewModel = new FileViewModel();
                singleFileSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleFileSaveToDatabase.FileName;
                singleFileSaveToDatabaseViewModel.Path = null;
                singleFileSaveToDatabaseViewModel.ContentType = model.SingleFileSaveToDatabase.ContentType;
                singleFileSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleFileSaveToDatabase);
                singleFileSaveToDatabaseViewModel.FileEncodingTypes = null;

                MemoryStream ms = new MemoryStream();
                model.SingleFileSaveToDatabase.CopyTo(ms);
                singleFileSaveToDatabaseViewModel.Data = ms.ToArray();
                ms.Close();
                ms.Dispose();
                model.SingleFileSaveToDatabaseViewModel = singleFileSaveToDatabaseViewModel;
            }

            if (model.MultipleFileSaveToDatabase.Count > 0)
            {
                List<FileViewModel> listMultipleFileSaveToDatabaseViewModel = new List<FileViewModel>();
                foreach (IFormFile photo in model.MultipleFileSaveToDatabase)
                {
                    FileViewModel multipleFileSaveToDatabaseViewModel = new FileViewModel();
                    multipleFileSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleFileSaveToDatabaseViewModel.Path = null;
                    multipleFileSaveToDatabaseViewModel.ContentType = photo.ContentType;
                    multipleFileSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToDatabase);
                    multipleFileSaveToDatabaseViewModel.FileEncodingTypes = null;

                    MemoryStream ms = new MemoryStream();
                    photo.CopyTo(ms);
                    multipleFileSaveToDatabaseViewModel.Data = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                    listMultipleFileSaveToDatabaseViewModel.Add(multipleFileSaveToDatabaseViewModel);
                }
                model.MultipleFileSaveToDatabaseViewModel = listMultipleFileSaveToDatabaseViewModel;
            }

            if (model.ExcelFileDataSaveToDatabase != null)
            {
                FileViewModel excelFileDataSaveToDatabaseViewModel = new FileViewModel();
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, Startup.StaticConfig["uploadFileFolder"]);
                excelFileDataSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.ExcelFileDataSaveToDatabase.FileName;
                excelFileDataSaveToDatabaseViewModel.Path = null;
                excelFileDataSaveToDatabaseViewModel.ContentType = model.ExcelFileDataSaveToDatabase.ContentType;
                excelFileDataSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase);
                excelFileDataSaveToDatabaseViewModel.FileEncodingTypes = null;
                excelFileDataSaveToDatabaseViewModel.Data = null;
                model.ExcelFileDataSaveToDatabaseViewModel = excelFileDataSaveToDatabaseViewModel;

                string path = string.Empty;
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                path = Path.Combine(uploadFolder, excelFileDataSaveToDatabaseViewModel.Name);
                using (FileStream fileStream = System.IO.File.Create(path))
                {
                    model.ExcelFileDataSaveToDatabase.CopyTo(fileStream);
                    fileStream.Flush();
                }
                var result = Getdata(path);
                //if (result.Count > 0)
                //{
                //    foreach (var item in result)
                //    {
                //        if (string.IsNullOrEmpty(item.Email) || string.IsNullOrWhiteSpace(item.Email) || !CommanFunction.ValidateEmail(item.Email))
                //        {
                //            ModelState.AddModelError("", "" + item.Email + " Id not valid!");
                //            complainantViewModel.Error = $"{item.Email} Id not valid!";
                //            return View(complainantViewModel);
                //        }
                //    }
                //}
                model.ListOfExcelFileDataSaveToDatabaseViewModel = result;
            }
            return model;
        }

        //To Get all excel data 
        [Route("userArea/complaientController/getdata")]
        private List<BulkDatas> Getdata(string name)
        {
            List<BulkDatas> bulk = new List<BulkDatas>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(name, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        BulkDatas bulkData = new BulkDatas();
                        bulkData.Name = reader.GetValue(0).ToString();
                        bulkData.Des = reader.GetValue(1).ToString();
                        bulkData.Email = reader.GetValue(2).ToString();
                        bulkData.MobileNumber = reader.GetValue(3).ToString();
                        bulk.Add(bulkData);
                        //bulk.Add(new BulkData()
                        //{
                        //    Name = reader.GetValue(0).ToString(),
                        //    Des = reader.GetValue(1).ToString()
                        //});
                    }
                }
            }
            return bulk;
        }

        //Edit Get request 
        [HttpGet]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/editComplaientByIdGet")]
        public IActionResult EditComplaient(string id)
        {
            // Decrypt the employee id using Unprotect method
            string decryptedComplaientId = protector.Unprotect(id);
            ComplainantViewModel complainantViewModel = complaientRepo.GetComplainantBiId(Convert.ToInt32(decryptedComplaientId));
            // Encrypt the ComplaientId value and store in ComplaientEncryptedId property
            complainantViewModel.ComplaientEncryptedId = protector.Protect(decryptedComplaientId);
            complainantViewModel.ComplaientDetailsEncryptedId = protector.Protect(complainantViewModel.ComplaientDetailsId.ToString());
            complainantViewModel.ComplaientPermamentAddressEncryptedId = protector.Protect(complainantViewModel.ComplaientPermamentAddressId.ToString());
            complainantViewModel.ComplaientTempAddressEncryptedId = protector.Protect(complainantViewModel.ComplaientTempAddressId.ToString());

            MakeUrl(complainantViewModel);
            return Ok(complainantViewModel);
        }

        //Edit Get request to make image and file src
        [Route("userArea/complaientController/makeUrl")]
        private static void MakeUrl(ComplainantViewModel complainantViewModel)
        {
            if (complainantViewModel.SingleImageSaveToFolderViewModel.Path != null)
            {
                //For Single Image from folder
                //Convert path Uri
                Uri uriSingleImageToFolder = new Uri(complainantViewModel.SingleImageSaveToFolderViewModel.Path);
                //Convert to Uri AbsolutePath
                string singleImageToFolderAbsolutePath = uriSingleImageToFolder.AbsolutePath;
                //Split from root floder
                string[] newSingleImageToFolderPath = singleImageToFolderAbsolutePath.Split("/wwwroot/");
                //get SolutionProjectName from app setting and make new url
                string singleImageToFolderPath = "/" + Startup.StaticConfig["SolutionProjectName"] + "/" + newSingleImageToFolderPath[1];
                //Re-Assinged path to model
                complainantViewModel.SingleImageSaveToFolderViewModel.Path = singleImageToFolderPath;

                //Remove guid number from name and get orignal name 
                string[] splitSingleImageToFolderName = complainantViewModel.SingleImageSaveToFolderViewModel.Name.Split("_");
                //Re-Assinged name
                complainantViewModel.SingleImageSaveToFolderViewModel.Name = splitSingleImageToFolderName[1];
            }

            //For Multiple Image from folder
            //List of image 
            if (complainantViewModel.MultipleImageSaveToFolderViewModel.Count > 0)
            {
                List<FileViewModel> listMultipleImageSaveToFolderViewModel = new List<FileViewModel>();
                foreach (var item in complainantViewModel.MultipleImageSaveToFolderViewModel)
                {
                    FileViewModel fileViewModel = new FileViewModel();
                    fileViewModel.Id = item.Id;
                    //Convert in Uri
                    Uri uriMultipleImageToFolder = new Uri(item.Path);
                    //Convert to Uri AbsolutePath
                    string multipleImageToFolderAbsolutePath = uriMultipleImageToFolder.AbsolutePath;
                    //Split from root floder
                    string[] newMultipleImageToFolderPath = multipleImageToFolderAbsolutePath.Split("/wwwroot/");
                    //get SolutionProjectName from app setting and make new url
                    string multipleImageToFolderPath = "/" + Startup.StaticConfig["SolutionProjectName"] + "/" + newMultipleImageToFolderPath[1];
                    //Re-Assinged path to model
                    fileViewModel.Path = multipleImageToFolderPath;
                    //Remove guid number from name and get orignal name 
                    string[] splitMultipleImageToFolderName = item.Name.Split("_");
                    //Re-Assinged name
                    fileViewModel.Name = splitMultipleImageToFolderName[1];
                    listMultipleImageSaveToFolderViewModel.Add(fileViewModel);
                }
                complainantViewModel.MultipleImageSaveToFolderViewModel = listMultipleImageSaveToFolderViewModel;
            }
        }

        //Edit Post request 
        [HttpPost]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/editComplaientByIdPost")]
        public IActionResult EditComplaient(ComplainantViewModel model)
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            complainantViewModel = model;
            if (model.TermsAndConditions == false)
            {
                ModelState.AddModelError("", "Please check Terms & Conditions check box!");
                complainantViewModel.Error = "Please check Terms & Conditions check box!";
                return View(complainantViewModel);
            }
            if (ModelState.IsValid)
            {
                model.ComplaientId = Convert.ToInt32(protector.Unprotect(model.ComplaientEncryptedId));
                model.ComplaientDetailsId = Convert.ToInt32(protector.Unprotect(model.ComplaientDetailsEncryptedId));
                model.ComplaientPermamentAddressId = Convert.ToInt32(protector.Unprotect(model.ComplaientPermamentAddressEncryptedId));
                model.ComplaientTempAddressId = Convert.ToInt32(protector.Unprotect(model.ComplaientTempAddressEncryptedId));
                FileOpertaionEditMode(model);
                bool updateComplaient = complaientRepo.UpdateComplaient(model);
                if (updateComplaient)
                {
                    return RedirectToAction("complaients");
                }
            }
            return BadRequest(complainantViewModel);
        }

        //Edit Post request to make image and file src 
        [Route("userArea/complaientController/fileOpertaionEditMode")]
        public ComplainantViewModel FileOpertaionEditMode(ComplainantViewModel model)
        {

            if (model.SingleImageSaveToFolder != null)
            {
                FileViewModel singleImageSaveToFolderViewModel = new FileViewModel();
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, Startup.StaticConfig["uploadImageFolder"]);
                if (model.SingleImageSaveToFolderViewModel.Id != 0)
                {
                    string sName = complaientRepo.GetFileNameById(model.SingleImageSaveToFolderViewModel.Id);
                    string fullPath = Path.Combine(uploadFolder, sName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                singleImageSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleImageSaveToFolder.FileName;
                singleImageSaveToFolderViewModel.Path = Path.Combine(uploadFolder, singleImageSaveToFolderViewModel.Name);
                singleImageSaveToFolderViewModel.ContentType = model.SingleImageSaveToFolder.ContentType;
                singleImageSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleImageSaveToFolder);
                singleImageSaveToFolderViewModel.FileEncodingTypes = null;
                singleImageSaveToFolderViewModel.Data = null;
                model.SingleImageSaveToFolder.CopyTo(new FileStream(singleImageSaveToFolderViewModel.Path, FileMode.Create));
                model.SingleImageSaveToFolderViewModel = singleImageSaveToFolderViewModel;
            }
            else
            {
                model.SingleImageSaveToFolderViewModel = null;
            }

            if (model.MultipleImageSaveToFolder.Count > 0)
            {
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, Startup.StaticConfig["uploadImageFolder"]);

                //Delete previous image from folder
                var getFilesId = complaientRepo.GetAllFiles(Convert.ToString(FileStoreModeOptions.MultipleImageSaveToFolder), model.ComplaientDetailsId);
                if (getFilesId.Count > 0)
                {
                    foreach (var item in getFilesId)
                    {
                        if (item != null)
                        {
                            string fullPath = Path.Combine(uploadFolder, item);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                    }
                }
                List<FileViewModel> listMultipleImageSaveToFolder = new List<FileViewModel>();
                foreach (IFormFile photo in model.MultipleImageSaveToFolder)
                {
                    FileViewModel listMultipleImageSaveToFolderViewModel = new FileViewModel();
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    listMultipleImageSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    listMultipleImageSaveToFolderViewModel.Path = Path.Combine(uploadFolder, listMultipleImageSaveToFolderViewModel.Name);
                    listMultipleImageSaveToFolderViewModel.ContentType = photo.ContentType;
                    listMultipleImageSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToFolder);
                    listMultipleImageSaveToFolderViewModel.FileEncodingTypes = null;
                    listMultipleImageSaveToFolderViewModel.Data = null;
                    photo.CopyTo(new FileStream(listMultipleImageSaveToFolderViewModel.Path, FileMode.Create));
                    listMultipleImageSaveToFolder.Add(listMultipleImageSaveToFolderViewModel);
                }
                model.MultipleImageSaveToFolderViewModel = null;
                model.MultipleImageSaveToFolderViewModel = listMultipleImageSaveToFolder;
            }
            else
            {
                model.MultipleImageSaveToFolderViewModel = null;
            }

            if (model.SingleImageSaveToDatabase != null)
            {
                FileViewModel fileSingleImageSaveToDatabaseViewModel = new FileViewModel();
                fileSingleImageSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleImageSaveToDatabase.FileName;
                fileSingleImageSaveToDatabaseViewModel.Path = null;
                fileSingleImageSaveToDatabaseViewModel.ContentType = model.SingleImageSaveToDatabase.ContentType;
                fileSingleImageSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleImageSaveToDatabase);
                fileSingleImageSaveToDatabaseViewModel.FileEncodingTypes = null;

                MemoryStream ms = new MemoryStream();
                model.SingleImageSaveToDatabase.CopyTo(ms);
                fileSingleImageSaveToDatabaseViewModel.Data = ms.ToArray();
                ms.Close();
                ms.Dispose();
                model.SingleImageSaveToDatabaseViewModel = fileSingleImageSaveToDatabaseViewModel;
            }

            if (model.MultipleImageSaveToDatabase.Count > 0)
            {
                List<FileViewModel> listMultipleImageSaveToDatabase = new List<FileViewModel>();
                foreach (IFormFile photo in model.MultipleImageSaveToDatabase)
                {
                    FileViewModel multipleImageSaveToDatabaseViewModel = new FileViewModel();
                    multipleImageSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleImageSaveToDatabaseViewModel.Path = null;
                    multipleImageSaveToDatabaseViewModel.ContentType = photo.ContentType;
                    multipleImageSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToDatabase);
                    multipleImageSaveToDatabaseViewModel.FileEncodingTypes = null;

                    MemoryStream ms = new MemoryStream();
                    photo.CopyTo(ms);
                    multipleImageSaveToDatabaseViewModel.Data = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                    listMultipleImageSaveToDatabase.Add(multipleImageSaveToDatabaseViewModel);
                }
                model.MultipleImageSaveToDatabaseViewModel = listMultipleImageSaveToDatabase;
            }

            if (model.SingleFileSaveToFolder != null)
            {
                FileViewModel singleFileSaveToFolderViewModel = new FileViewModel();
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, Startup.StaticConfig["uploadFileFolder"]);
                if (model.SingleFileSaveToFolderViewModel.Id != 0)
                {
                    string sName = complaientRepo.GetFileNameById(model.SingleFileSaveToFolderViewModel.Id);
                    string fullPath = Path.Combine(uploadFolder, sName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                singleFileSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleFileSaveToFolder.FileName;
                singleFileSaveToFolderViewModel.Path = Path.Combine(uploadFolder, singleFileSaveToFolderViewModel.Name);
                singleFileSaveToFolderViewModel.ContentType = model.SingleFileSaveToFolder.ContentType;
                singleFileSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleFileSaveToFolder);
                singleFileSaveToFolderViewModel.FileEncodingTypes = null;
                singleFileSaveToFolderViewModel.Data = null;
                model.SingleFileSaveToFolder.CopyTo(new FileStream(singleFileSaveToFolderViewModel.Path, FileMode.Create));
                model.SingleFileSaveToFolderViewModel = singleFileSaveToFolderViewModel;
            }
            else
            {
                model.SingleFileSaveToFolderViewModel = null;
            }

            if (model.MultipleFileSaveToFolder.Count > 0)
            {
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, Startup.StaticConfig["uploadFileFolder"]);
                //Delete previous file from folder
                var getFilesName = complaientRepo.GetAllFiles(Convert.ToString(FileStoreModeOptions.MultipleFileSaveToFolder), model.ComplaientDetailsId);
                if (getFilesName.Count > 0)
                {
                    foreach (var item in getFilesName)
                    {
                        if (item != null)
                        {
                            string fullPath = Path.Combine(uploadFolder, item);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                    }
                }
                List<FileViewModel> listMultipleFileSaveToFolderViewModel = new List<FileViewModel>();
                foreach (IFormFile photo in model.MultipleFileSaveToFolder)
                {
                    FileViewModel multipleFileSaveToFolderViewModel = new FileViewModel();
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    multipleFileSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleFileSaveToFolderViewModel.Path = Path.Combine(uploadFolder, multipleFileSaveToFolderViewModel.Name);
                    multipleFileSaveToFolderViewModel.ContentType = photo.ContentType;
                    multipleFileSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToFolder);
                    multipleFileSaveToFolderViewModel.Data = null;
                    multipleFileSaveToFolderViewModel.FileEncodingTypes = null;
                    photo.CopyTo(new FileStream(multipleFileSaveToFolderViewModel.Path, FileMode.Create));
                    listMultipleFileSaveToFolderViewModel.Add(multipleFileSaveToFolderViewModel);
                }
                model.MultipleFileSaveToFolderViewModel = listMultipleFileSaveToFolderViewModel;
            }
            else
            {
                model.MultipleFileSaveToFolderViewModel = null;
            }

            if (model.SingleFileSaveToDatabase != null)
            {
                FileViewModel singleFileSaveToDatabaseViewModel = new FileViewModel();
                singleFileSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleFileSaveToDatabase.FileName;
                singleFileSaveToDatabaseViewModel.Path = null;
                singleFileSaveToDatabaseViewModel.ContentType = model.SingleFileSaveToDatabase.ContentType;
                singleFileSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleFileSaveToDatabase);
                singleFileSaveToDatabaseViewModel.FileEncodingTypes = null;

                MemoryStream ms = new MemoryStream();
                model.SingleFileSaveToDatabase.CopyTo(ms);
                singleFileSaveToDatabaseViewModel.Data = ms.ToArray();
                ms.Close();
                ms.Dispose();
                model.SingleFileSaveToDatabaseViewModel = singleFileSaveToDatabaseViewModel;
            }

            if (model.MultipleFileSaveToDatabase.Count > 0)
            {
                List<FileViewModel> listMultipleFileSaveToDatabaseViewModel = new List<FileViewModel>();
                foreach (IFormFile photo in model.MultipleFileSaveToDatabase)
                {
                    FileViewModel multipleFileSaveToDatabaseViewModel = new FileViewModel();
                    multipleFileSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleFileSaveToDatabaseViewModel.Path = null;
                    multipleFileSaveToDatabaseViewModel.ContentType = photo.ContentType;
                    multipleFileSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToDatabase);
                    multipleFileSaveToDatabaseViewModel.FileEncodingTypes = null;

                    MemoryStream ms = new MemoryStream();
                    photo.CopyTo(ms);
                    multipleFileSaveToDatabaseViewModel.Data = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                    listMultipleFileSaveToDatabaseViewModel.Add(multipleFileSaveToDatabaseViewModel);
                }
                model.MultipleFileSaveToDatabaseViewModel = listMultipleFileSaveToDatabaseViewModel;
            }

            if (model.ExcelFileDataSaveToDatabase != null)
            {
                FileViewModel excelFileDataSaveToDatabaseViewModel = new FileViewModel();
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, Startup.StaticConfig["uploadFileFolder"]);
                if (model.ExcelFileDataSaveToDatabaseViewModel.Id != 0)
                {
                    string sName = complaientRepo.GetFileNameById(model.ExcelFileDataSaveToDatabaseViewModel.Id);
                    string fullPath = Path.Combine(uploadFolder, sName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                excelFileDataSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.ExcelFileDataSaveToDatabase.FileName;
                excelFileDataSaveToDatabaseViewModel.Path = null;
                excelFileDataSaveToDatabaseViewModel.ContentType = model.ExcelFileDataSaveToDatabase.ContentType;
                excelFileDataSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase);
                excelFileDataSaveToDatabaseViewModel.FileEncodingTypes = null;
                excelFileDataSaveToDatabaseViewModel.Data = null;
                model.ExcelFileDataSaveToDatabaseViewModel = excelFileDataSaveToDatabaseViewModel;

                string path = string.Empty;
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                path = Path.Combine(uploadFolder, excelFileDataSaveToDatabaseViewModel.Name);
                using (FileStream fileStream = System.IO.File.Create(path))
                {
                    model.ExcelFileDataSaveToDatabase.CopyTo(fileStream);
                    fileStream.Flush();
                }
                var result = Getdata(path);
                //if (result.Count > 0)
                //{
                //    foreach (var item in result)
                //    {
                //        if (string.IsNullOrEmpty(item.Email) || string.IsNullOrWhiteSpace(item.Email) || !CommanFunction.ValidateEmail(item.Email))
                //        {
                //            ModelState.AddModelError("", "" + item.Email + " Id not valid!");
                //            complainantViewModel.Error = $"{item.Email} Id not valid!";
                //            return View(complainantViewModel);
                //        }
                //    }
                //}
                model.ListOfExcelFileDataSaveToDatabaseViewModel = result;
            }
            else
            {
                model.ListOfExcelFileDataSaveToDatabaseViewModel = null;
            }
            return model;
        }

        ////Dowonload image using id 
        //public FileResult DownloadFileFromFolder(int fileId)
        //{
        //    var getFile = complaientRepo.DownloadFile(fileId);
        //    byte[] fileBytes = null;
        //    if (getFile.Data != null)
        //    {
        //        Uri uri = new Uri(getFile.Path);
        //        string uriAbsolutePath = uri.AbsolutePath;
        //        fileBytes = GetFile(uriAbsolutePath);
        //        string[] name = getFile.Name.Split("_");
        //        return File(fileBytes, getFile.ContentType, name[1]);
        //    }
        //    else
        //    {
        //        Uri uri = new Uri(getFile.Path);
        //        string uriAbsolutePath = uri.AbsolutePath;
        //        fileBytes = GetFile(uriAbsolutePath);
        //        string[] name = getFile.Name.Split("_");
        //        return File(fileBytes, getFile.ContentType, name[1]);
        //    }
        //}

        //Get file in byte arrary 
        //public byte[] GetFile(string s)
        //{
        //    System.IO.FileStream fs = System.IO.File.OpenRead(s);
        //    byte[] data = new byte[fs.Length];
        //    int br = fs.Read(data, 0, data.Length);
        //    if (br != fs.Length)
        //        throw new System.IO.IOException(s);
        //    return data;
        //}

        ////Download file from database
        //[Route("userArea/complaientController/downloadFileFromDbById")]
        //public IActionResult DownloadFileFromDb(int fileId)
        //{
        //    //Fetch the File data from Database.
        //    var getFile = complaientRepo.DownloadFile(fileId);
        //    if (getFile.FileStoreMode == Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase))
        //    {

        //    }
        //    string[] name = getFile.Name.Split("_");
        //    //return File(getFile.Data, getFile.ContentType, name[1]);
        //    return Ok(getFile);
        //}
        //Download Excel file from database
        //public FileResult DownloadExcelFileFromDb(int fileId)
        //{
        //    //Fetch the File data from Database.
        //    var getFile = complaientRepo.DownloadFile(fileId);
        //    string[] name = getFile.Name.Split("_");
        //    if (getFile.FileStoreMode == Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase))
        //    {
        //        var bulkDatas = complaientRepo.GetExcelData(getFile.FileStoreMode);
        //        if (bulkDatas.Count > 0)
        //        {
        //            var comlumHeadrs = new string[]
        //                               {
        //                                    "Bulk Data Id",
        //                                    "Bulk Id",
        //                                    "Name",
        //                                    "Email",
        //                                    "Mobile Number",
        //                                    "Des"
        //                               };
        //            byte[] result;
        //            using (var package = new ExcelPackage())
        //            {
        //                // add a new worksheet to the empty workbook

        //                var worksheet = package.Workbook.Worksheets.Add(name[1]); //Worksheet name
        //                using (var cells = worksheet.Cells[1, 1, 1, 6]) //(1,1) (1,5)
        //                {
        //                    cells.Style.Font.Bold = true;
        //                }

        //                //First add the headers
        //                for (var i = 0; i < comlumHeadrs.Count(); i++)
        //                {
        //                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
        //                }
        //                //Add values
        //                var j = bulkDatas.Count + 1;
        //                foreach (var employee in bulkDatas)
        //                {
        //                    if (j > 0)
        //                    {
        //                        worksheet.Cells["A" + j].Value = employee.Id;
        //                        worksheet.Cells["B" + j].Value = employee.BulkId;
        //                        worksheet.Cells["C" + j].Value = employee.Name;
        //                        worksheet.Cells["D" + j].Value = employee.Email;
        //                        worksheet.Cells["E" + j].Value = employee.MobileNumber;
        //                        worksheet.Cells["F" + j].Value = employee.Des;
        //                        j = j - 1;
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //                result = package.GetAsByteArray();
        //            }
        //            string[] splitName = name[1].Split(".");
        //            string fileName = $"{splitName[0]}-{DateTime.Now.ToShortDateString()}.{splitName[1]}";
        //            return File(result, getFile.ContentType, $"{fileName}");
        //        }
        //    }
        //    return null;
        //}
        ////Delete complaint function not implimented but id value getting form view to controller using ajax request 
        [HttpPost]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/deleteComplaientByIdGet")]
        public IActionResult DeleteComplaient(int id)
        {
            bool result = true;
            if (result)
            {
                return Ok(new { success = true, message = "Deleted Successfully" });
            }
            else
            {
                return BadRequest(new { success = true, message = "Selected complaints's can not delete !" });
            }
        }

        //Delete Multiple complaints function not implimented but id value getting form view to controller using ajax request 
        [HttpPost]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/multipleComplaientDeletePost")]
        public IActionResult MultipleComplaientDelete(List<int> ids)
        {
            bool result = true;
            if (result)
            {
                return Ok(new { success = true, message = "Selected complaints's deleted successfully." });
            }
            else
            {
                return BadRequest(new { success = true, message = "Selected complaints's can not delete !" });
            }
        }

        //Bind State using ajax request 
        [HttpGet]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/getStatelist")]
        public IActionResult GetStatelist(int countryId)
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            var states = complaientRepo.StateList(countryId);
            if (states != null)
            {
                foreach (var item in states)
                {
                    complainantViewModel.States.Add(new SelectListItem
                    {
                        Text = item.StateName,
                        Value = Convert.ToString(item.StateId)
                    });
                }
                return Ok(complainantViewModel);
            }
            return null;
        }

        //Bind City using ajax request 
        [HttpGet]
        //[EnableCors("AllowOrigin")]
        [Route("userArea/complaientController/getCitylist")]
        public IActionResult GetCitylist(int stateId)
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            var cities = complaientRepo.CityList(stateId);
            if (cities != null)
            {
                foreach (var item in cities)
                {
                    complainantViewModel.Cities.Add(new SelectListItem
                    {
                        Text = item.CityName,
                        Value = Convert.ToString(item.CityId)
                    });
                }
                return Ok(complainantViewModel);
            }
            return null;
        }
    }
}
