using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.Enum;
using EmpManagmentWebApiBOL.ViewModels.EmployeesViewModels.ViewModels;
using EmpManagmentWebApiDAL.DbContextClass;
using EmpManagmentWebApiIBLL.ComplaientRepository;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmpManagmentWebApiBLL.ComplaientBs
{
    public class SqlComplaientBs : IComplaientRepositery
    {
        private readonly EmployeeDbContext DbContext;
        public SqlComplaientBs(EmployeeDbContext employeeDbContext)
        {
            DbContext = employeeDbContext;
        }
        public ComplaientCategory AddCategory(ComplaientCategory category)
        {
            DbContext.ComplaientCategories.Add(category);
            DbContext.SaveChanges();
            return category;
        }
        public bool CategoryExisits(string description)
        {
            bool isDescriptionRxisits = false;
            var checkIfExisits = DbContext.ComplaientCategories.Where(x => x.Description == description).FirstOrDefault();
            if (checkIfExisits != null)
            {
                isDescriptionRxisits = true;
            }
            return isDescriptionRxisits;
        }
        public ComplaientCategory Delete(int complaientCategoryId)
        {
            ComplaientCategory result = DbContext.ComplaientCategories.Find(complaientCategoryId);
            if (result != null)
            {
                DbContext.ComplaientCategories.Remove(result);
                DbContext.SaveChanges();
            }
            return result;
        }
        public IEnumerable<ComplaientCategoryViewModel> GetAllComplaientCategory()
        {
            //Query with where condition
            //var getAllCategory = DbContext.ComplaientCategories.Where(x => x.Status == true).Select(
            //   a => new ComplaientCategoryViewModel
            //   {
            //       ComplaientCategoryId = a.ComplaientCategoryId,
            //       Description = a.Description,
            //       Status = ((a.Status == true) ? "Active" : "No Active")
            //   });

            var getAllCategory = DbContext.ComplaientCategories.Select(a => new ComplaientCategoryViewModel
            {
                ComplaientCategoryId = a.ComplaientCategoryId,
                Description = a.Description,
                Status = a.Status,
                UserStatus = ((a.Status == true) ? "Active" : "Not Active")
            });
            if (getAllCategory != null)
            {
                return getAllCategory;
            }
            return null;
        }
        public ComplaientCategory GetCategoryById(int complaientCategoryId)
        {
            var result = DbContext.ComplaientCategories.Where(x => x.ComplaientCategoryId == complaientCategoryId).FirstOrDefault();
            if (result != null)
            {
                return result;
            }
            return null;
        }
        public bool MultipleCategoryDelete(List<int> ids)
        {
            bool deletedSuccessfully = false;
            if (ids!= null)
            {
                foreach (var item in ids)
                {
                    ComplaientCategory result = DbContext.ComplaientCategories.Find(item);
                    if (result != null)
                    {
                        DbContext.ComplaientCategories.Remove(result);
                        DbContext.SaveChanges();
                    }
                }
                deletedSuccessfully = true;
            }
            return deletedSuccessfully;
        }
        public ComplaientCategory UpdateCategoryById(ComplaientCategory category)
        {
            var updateCategory = DbContext.ComplaientCategories.Attach(category);
            updateCategory.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            DbContext.SaveChanges();
            return category;
        }
        public IEnumerable<Country> CounteryList()
        {
            var allCountries = DbContext.Countries.Where(x => x.Status == true).ToList();
            if (allCountries!= null)
            {
                return allCountries;
            }
            return null;
        }
        public IEnumerable<State> StateList(int countryId)
        {
            var getStateByCountry = DbContext.States.Where(x => x.CountryId == countryId && x.Status == true).ToList();
            if (getStateByCountry!= null)
            {
                return getStateByCountry;
            }
            return null;
        }
        public IEnumerable<City> CityList(int stateId)
        {
            var getCityByState = DbContext.Cities.Where(x => x.StateId == stateId && x.Status == true).ToList();
            if (getCityByState!= null)
            {
                return getCityByState;
            }
            return null;
        }
        public IEnumerable<ComplaientCategory> ComplaientCategoryList()
        {
            var allComplaientCategory = DbContext.ComplaientCategories.Where(x => x.Status == true).ToList();
            if (allComplaientCategory!= null)
            {
                return allComplaientCategory;
            }
            return null;
        }
        public IEnumerable<Gender> Genders()
        {
            var gender = DbContext.Genders.Where(x => x.Status == true).ToList();
            if (gender!= null)
            {
                return gender;
            }
            return null;
        }
        public IEnumerable<BikeCategory> BikeCategory()
        {
            var result = DbContext.BikeCategories.ToList();
            if (result!= null)
            {
                return result;
            }
            return null;
        }
        public bool AddComplaient(ComplainantViewModel model)
        {
            bool isSavedSucess = false;
            if (model.TermsAndConditions == true)
            {
                int iComplaientId = 0;
                int iComplaientDetailsId = 0;

                //Save Complainet Table values
                Complaients complaients = new Complaients();
                complaients.ComplainantName = model.ComplainantName;
                complaients.ComplainantEmail = model.ComplainantEmail;
                complaients.ComplaientStatus = true;
                complaients.CompaientDate = model.CompaientDate;
                DbContext.Complaients.Add(complaients);
                DbContext.SaveChanges();
                iComplaientId = complaients.ComplaientId;

                //Save ComplaientDetails Table values
                ComplaientDetails complaientDetails = new ComplaientDetails();
                complaientDetails.ComplaientId = iComplaientId;
                complaientDetails.ComplaientCategoryId = model.ComplaientCategoryId;
                complaientDetails.GenderId = model.GenderId;
                complaientDetails.CountryId = model.CountryId;
                complaientDetails.StateId = model.StateId;
                complaientDetails.CityId = model.CityId;
                complaientDetails.Description = model.Description;
                complaientDetails.ComplaientDate = model.ComplaientDate;
                DbContext.ComplaientDetails.Add(complaientDetails);
                DbContext.SaveChanges();
                iComplaientDetailsId = complaientDetails.ComplaientDetailsId;

                //Save ComplaientPermamentAddress Table values
                ComplaientPermamentAddress complaientPermamentAddress = new ComplaientPermamentAddress();
                complaientPermamentAddress.ComplaientDetailsId = iComplaientDetailsId;
                complaientPermamentAddress.Address = model.PermamentAddress;
                complaientPermamentAddress.PostalCode = Convert.ToInt32(model.PermamentAddressPostalCode);
                DbContext.ComplaientPermamentAddresses.Add(complaientPermamentAddress);
                DbContext.SaveChanges();

                //Save ComplaientTempAddress Table values
                ComplaientTempAddress complaientTempAddress = new ComplaientTempAddress();
                complaientTempAddress.ComplaientDetailsId = iComplaientDetailsId;
                complaientTempAddress.Address = model.TempAddress;
                complaientTempAddress.PostalCode = Convert.ToInt32(model.TempAddressPostalCode);
                DbContext.ComplaientTempAddresses.Add(complaientTempAddress);
                DbContext.SaveChanges();

                //Save BikeCategories Table values
                if (model.BikeCategories!= null)
                {
                    foreach (var item in model.BikeCategories)
                    {
                        if (item.Status == true)
                        {
                            BikeCollection bikeCollection = new BikeCollection();
                            bikeCollection.ComplaientDetailsId = iComplaientDetailsId;
                            bikeCollection.BikeCategoryId = item.BikeCategoryId;
                            bikeCollection.Status = item.Status;
                            bikeCollection.CreatedDate = DateTime.Now;
                            DbContext.BikeCollections.Add(bikeCollection);
                            DbContext.SaveChanges();
                        }
                    }
                }

                //Save SingleImageSaveToFolder
                if (model.SingleImageSaveToFolderViewModel != null)
                {
                    Files singleImageSaveToFolder = new Files();
                    singleImageSaveToFolder.ComplaientDetailsId = iComplaientDetailsId;
                    singleImageSaveToFolder.Name = model.SingleImageSaveToFolderViewModel.Name;
                    singleImageSaveToFolder.ContentType = model.SingleImageSaveToFolderViewModel.ContentType;
                    singleImageSaveToFolder.FileEncodingTypes = model.SingleImageSaveToFolderViewModel.FileEncodingTypes;
                    singleImageSaveToFolder.FileStoreMode = model.SingleImageSaveToFolderViewModel.FileStoreMode;
                    singleImageSaveToFolder.Path = model.SingleImageSaveToFolderViewModel.Path;
                    singleImageSaveToFolder.Data = model.SingleImageSaveToFolderViewModel.Data;
                    DbContext.Files.Add(singleImageSaveToFolder);
                    DbContext.SaveChanges();
                }

                //Save MultipleImageSaveToFolder
                if (model.MultipleImageSaveToFolderViewModel!= null)
                {
                    foreach (var item in model.MultipleImageSaveToFolderViewModel)
                    {
                        Files listMultipleImageSaveToFolder = new Files();
                        listMultipleImageSaveToFolder.ComplaientDetailsId = iComplaientDetailsId;
                        listMultipleImageSaveToFolder.Name = item.Name;
                        listMultipleImageSaveToFolder.ContentType = item.ContentType;
                        listMultipleImageSaveToFolder.FileEncodingTypes = item.FileEncodingTypes;
                        listMultipleImageSaveToFolder.FileStoreMode = item.FileStoreMode;
                        listMultipleImageSaveToFolder.Path = item.Path;
                        listMultipleImageSaveToFolder.Data = item.Data;
                        DbContext.Files.Add(listMultipleImageSaveToFolder);
                        DbContext.SaveChanges();
                    }
                }


                //Save SingleImageSaveToDatabase
                if (model.SingleImageSaveToDatabaseViewModel != null)
                {
                    Files singleImageSaveToDatabaseViewModel = new Files();
                    singleImageSaveToDatabaseViewModel.ComplaientDetailsId = iComplaientDetailsId;
                    singleImageSaveToDatabaseViewModel.Name = model.SingleImageSaveToDatabaseViewModel.Name;
                    singleImageSaveToDatabaseViewModel.ContentType = model.SingleImageSaveToDatabaseViewModel.ContentType;
                    singleImageSaveToDatabaseViewModel.FileEncodingTypes = model.SingleImageSaveToDatabaseViewModel.FileEncodingTypes;
                    singleImageSaveToDatabaseViewModel.FileStoreMode = model.SingleImageSaveToDatabaseViewModel.FileStoreMode;
                    singleImageSaveToDatabaseViewModel.Path = model.SingleImageSaveToDatabaseViewModel.Path;
                    singleImageSaveToDatabaseViewModel.Data = model.SingleImageSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(singleImageSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }

                //Save MultipleImageSaveToDatabase
                if (model.MultipleImageSaveToDatabaseViewModel!= null)
                {
                    foreach (var item in model.MultipleImageSaveToDatabaseViewModel)
                    {
                        Files multipleImageSaveToDatabaseViewModel = new Files();
                        multipleImageSaveToDatabaseViewModel.ComplaientDetailsId = iComplaientDetailsId;
                        multipleImageSaveToDatabaseViewModel.Name = item.Name;
                        multipleImageSaveToDatabaseViewModel.ContentType = item.ContentType;
                        multipleImageSaveToDatabaseViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        multipleImageSaveToDatabaseViewModel.FileStoreMode = item.FileStoreMode;
                        multipleImageSaveToDatabaseViewModel.Path = item.Path;
                        multipleImageSaveToDatabaseViewModel.Data = item.Data;
                        DbContext.Files.Add(multipleImageSaveToDatabaseViewModel);
                        DbContext.SaveChanges();
                    }
                }

                //Save SingleFileSaveToFolder
                if (model.SingleFileSaveToFolderViewModel != null)
                {
                    Files singleFileSaveToFolderViewModel = new Files();
                    singleFileSaveToFolderViewModel.ComplaientDetailsId = iComplaientDetailsId;
                    singleFileSaveToFolderViewModel.Name = model.SingleFileSaveToFolderViewModel.Name;
                    singleFileSaveToFolderViewModel.ContentType = model.SingleFileSaveToFolderViewModel.ContentType;
                    singleFileSaveToFolderViewModel.FileEncodingTypes = model.SingleFileSaveToFolderViewModel.FileEncodingTypes;
                    singleFileSaveToFolderViewModel.FileStoreMode = model.SingleFileSaveToFolderViewModel.FileStoreMode;
                    singleFileSaveToFolderViewModel.Path = model.SingleFileSaveToFolderViewModel.Path;
                    singleFileSaveToFolderViewModel.Data = model.SingleFileSaveToFolderViewModel.Data;
                    DbContext.Files.Add(singleFileSaveToFolderViewModel);
                    DbContext.SaveChanges();
                }

                //Save MultipleFileSaveToFolder
                if (model.MultipleFileSaveToFolderViewModel!= null)
                {
                    foreach (var item in model.MultipleFileSaveToFolderViewModel)
                    {
                        Files listMultipleFileSaveToFolderViewModel = new Files();
                        listMultipleFileSaveToFolderViewModel.ComplaientDetailsId = iComplaientDetailsId;
                        listMultipleFileSaveToFolderViewModel.Name = item.Name;
                        listMultipleFileSaveToFolderViewModel.ContentType = item.ContentType;
                        listMultipleFileSaveToFolderViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        listMultipleFileSaveToFolderViewModel.FileStoreMode = item.FileStoreMode;
                        listMultipleFileSaveToFolderViewModel.Path = item.Path;
                        listMultipleFileSaveToFolderViewModel.Data = item.Data;
                        DbContext.Files.Add(listMultipleFileSaveToFolderViewModel);
                        DbContext.SaveChanges();
                    }
                }

                //Save SingleFileSaveToDatabase
                if (model.SingleFileSaveToDatabaseViewModel != null)
                {
                    Files singleFileSaveToDatabaseViewModel = new Files();
                    singleFileSaveToDatabaseViewModel.ComplaientDetailsId = iComplaientDetailsId;
                    singleFileSaveToDatabaseViewModel.Name = model.SingleFileSaveToDatabaseViewModel.Name;
                    singleFileSaveToDatabaseViewModel.ContentType = model.SingleFileSaveToDatabaseViewModel.ContentType;
                    singleFileSaveToDatabaseViewModel.FileEncodingTypes = model.SingleFileSaveToDatabaseViewModel.FileEncodingTypes;
                    singleFileSaveToDatabaseViewModel.FileStoreMode = model.SingleFileSaveToDatabaseViewModel.FileStoreMode;
                    singleFileSaveToDatabaseViewModel.Path = model.SingleFileSaveToDatabaseViewModel.Path;
                    singleFileSaveToDatabaseViewModel.Data = model.SingleFileSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(singleFileSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }

                //Save MultipleFileSaveToDatabase
                if (model.MultipleFileSaveToDatabaseViewModel!= null)
                {
                    foreach (var item in model.MultipleFileSaveToDatabaseViewModel)
                    {
                        Files multipleFileSaveToDatabaseViewModel = new Files();
                        multipleFileSaveToDatabaseViewModel.ComplaientDetailsId = iComplaientDetailsId;
                        multipleFileSaveToDatabaseViewModel.Name = item.Name;
                        multipleFileSaveToDatabaseViewModel.ContentType = item.ContentType;
                        multipleFileSaveToDatabaseViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        multipleFileSaveToDatabaseViewModel.FileStoreMode = item.FileStoreMode;
                        multipleFileSaveToDatabaseViewModel.Path = item.Path;
                        multipleFileSaveToDatabaseViewModel.Data = item.Data;
                        DbContext.Files.Add(multipleFileSaveToDatabaseViewModel);
                        DbContext.SaveChanges();
                    }
                }

                //Save Bulk and BulkData Table
                Bulk bulk = new Bulk();
                bulk.ComplaientDetailsId = iComplaientDetailsId;
                bulk.FileStoreMode = Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase);
                bulk.CreatedDate = DateTime.Now;
                DbContext.Bulk.Add(bulk);
                DbContext.SaveChanges();
                if (bulk.BulkId > 0)
                {
                    if (model.ListOfExcelFileDataSaveToDatabaseViewModel!= null)
                    {
                        foreach (var item in model.ListOfExcelFileDataSaveToDatabaseViewModel)
                        {
                            BulkDatas BulkData = new BulkDatas();
                            BulkData.BulkId = bulk.BulkId;
                            BulkData.Name = item.Name;
                            BulkData.Des = item.Des;
                            BulkData.Email = item.Email;
                            BulkData.MobileNumber = item.MobileNumber;
                            DbContext.BulkDatas.Add(BulkData);
                            DbContext.SaveChanges();
                        }
                    }
                }
                if (model.ExcelFileDataSaveToDatabaseViewModel != null)
                {
                    Files excelFileDataSaveToDatabaseViewModel = new Files();
                    excelFileDataSaveToDatabaseViewModel.ComplaientDetailsId = iComplaientDetailsId;
                    excelFileDataSaveToDatabaseViewModel.Name = model.ExcelFileDataSaveToDatabaseViewModel.Name;
                    excelFileDataSaveToDatabaseViewModel.ContentType = model.ExcelFileDataSaveToDatabaseViewModel.ContentType;
                    excelFileDataSaveToDatabaseViewModel.FileEncodingTypes = model.ExcelFileDataSaveToDatabaseViewModel.FileEncodingTypes;
                    excelFileDataSaveToDatabaseViewModel.FileStoreMode = model.ExcelFileDataSaveToDatabaseViewModel.FileStoreMode;
                    excelFileDataSaveToDatabaseViewModel.Path = model.ExcelFileDataSaveToDatabaseViewModel.Path;
                    excelFileDataSaveToDatabaseViewModel.Data = model.ExcelFileDataSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(excelFileDataSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }

                isSavedSucess = true;
            }

            return isSavedSucess;
        }
        public IEnumerable<ComplainantAndDetailsViewModel> GetAllComplaient(IDataProtector protector)
        {
            var categorizedProducts = (from complaients in DbContext.Complaients
                                       join complaientDetails in DbContext.ComplaientDetails on complaients.ComplaientId equals complaientDetails.ComplaientId
                                       join complaientCategories in DbContext.ComplaientCategories on complaientDetails.ComplaientCategoryId equals complaientCategories.ComplaientCategoryId
                                       join country in DbContext.Countries on complaientDetails.CountryId equals country.CountryId
                                       join state in DbContext.States on complaientDetails.StateId equals state.StateId
                                       join city in DbContext.Cities on complaientDetails.CityId equals city.CityId
                                       select new
                                       {
                                           complaients.ComplaientId,
                                           complaients.ComplainantName,
                                           complaients.ComplainantEmail,
                                           complaients.CompaientDate,
                                           complaientCategories.Description,
                                           country.CountryName,
                                           state.StateName,
                                           city.CityName,
                                           ComplaientDescription = complaientDetails.Description,
                                           complaientDetails.ComplaientDate
                                       }).OrderByDescending(x => x.ComplaientId).ToList();
            if (categorizedProducts != null)
            {
                List<ComplainantAndDetailsViewModel> complainantListViewModel = new List<ComplainantAndDetailsViewModel>();
                foreach (var item in categorizedProducts)
                {
                    ComplainantAndDetailsViewModel complainantListView = new ComplainantAndDetailsViewModel();
                    complainantListView.ComplaientId = item.ComplaientId;
                    // Encrypt the ComplaientId value and store in ComplaientEncryptedId property
                    complainantListView.ComplaientEncryptedId = protector.Protect(item.ComplaientId.ToString());
                    complainantListView.ComplainantName = item.ComplainantName;
                    complainantListView.ComplainantEmail = item.ComplainantEmail;
                    complainantListView.CompaientDate = item.CompaientDate;
                    complainantListView.ComplaientCategoriesDescription = item.Description;
                    complainantListView.CountryName = item.CountryName;
                    complainantListView.StateName = item.StateName;
                    complainantListView.CityName = item.CityName;
                    complainantListView.ComplaientDescription = item.ComplaientDescription;
                    complainantListView.ShowComplaientDate = String.Format("{0:dd-MM-yyyy}", item.ComplaientDate);
                    complainantListViewModel.Add(complainantListView);
                }
                return complainantListViewModel;
            }
            return null;
        }
        public IEnumerable<BikeCategory> GetAllBikeCategory()
        {
            var result = (from obj in DbContext.BikeCategories select obj).ToList();
            if (result !=null)
            {
                return result;
            }
            return null;
        }
        public ComplainantViewModel GetComplainantBiId(int id)
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            var complaientById = (from obj in DbContext.Complaients
                                  join cd in DbContext.ComplaientDetails on obj.ComplaientId equals cd.ComplaientId
                                  join pa in DbContext.ComplaientPermamentAddresses on cd.ComplaientDetailsId equals pa.ComplaientDetailsId
                                  join ta in DbContext.ComplaientTempAddresses on cd.ComplaientDetailsId equals ta.ComplaientDetailsId
                                  where obj.ComplaientId == id
                                  select new
                                  {
                                      obj.ComplaientId,
                                      obj.ComplainantName,
                                      obj.ComplainantEmail,
                                      obj.CompaientDate,
                                      cd.ComplaientDetailsId,
                                      cd.ComplaientCategoryId,
                                      cd.CountryId,
                                      cd.StateId,
                                      cd.CityId,
                                      cd.Description,
                                      cd.ComplaientDate,
                                      cd.GenderId,
                                      pa.ComplaientPermamentAddressId,
                                      PermamentAddress = pa.Address,
                                      PermamentAddressPostalCode = pa.PostalCode,
                                      ta.ComplaientTempAddressId,
                                      TempAddress = ta.Address,
                                      TempAddressPostalCode = ta.PostalCode
                                  }).FirstOrDefault();
            //Get Saved bike category list
            complainantViewModel.ComplaientId = complaientById.ComplaientId;
            complainantViewModel.ComplainantName = complaientById.ComplainantName;
            complainantViewModel.ComplainantEmail = complaientById.ComplainantEmail;
            complainantViewModel.CompaientDate = complaientById.CompaientDate;
            complainantViewModel.ComplaientDetailsId = complaientById.ComplaientDetailsId;
            complainantViewModel.ComplaientCategoryId = complaientById.ComplaientCategoryId;
            complainantViewModel.CountryId = complaientById.CountryId;
            complainantViewModel.StateId = complaientById.StateId;
            complainantViewModel.CityId = complaientById.CityId;
            complainantViewModel.Description = complaientById.Description;
            complainantViewModel.ComplaientDate = complaientById.ComplaientDate;
            complainantViewModel.ComplaientPermamentAddressId = complaientById.ComplaientPermamentAddressId;
            complainantViewModel.PermamentAddress = complaientById.PermamentAddress;
            complainantViewModel.PermamentAddressPostalCode = Convert.ToString(complaientById.PermamentAddressPostalCode);
            complainantViewModel.ComplaientTempAddressId = complaientById.ComplaientTempAddressId;
            complainantViewModel.TempAddress = complaientById.TempAddress;
            complainantViewModel.TempAddressPostalCode = Convert.ToString(complaientById.TempAddressPostalCode);
            complainantViewModel.ComplaientId = complaientById.ComplaientId;
            complainantViewModel.GenderId = complaientById.GenderId;
            var updateBikeCategories = (from obj in DbContext.BikeCollections where obj.ComplaientDetailsId == complaientById.ComplaientDetailsId select obj).ToList();
            complainantViewModel.BikeCategoriesSelectListId = updateBikeCategories.Select(x => x.BikeCategoryId).ToList();
            //Bind country
            var countries = CounteryList().ToList();
            if (countries!= null)
            {
                foreach (var item in countries)
                {
                    complainantViewModel.Countries.Add(new SelectListItem { Text = item.CountryName, Value = item.CountryId.ToString(), Selected = true });
                }
            }
            //Bind state by country id
            var states = StateList(complaientById.CountryId);
            if (states != null)
            {
                foreach (var item in states)
                {
                    complainantViewModel.States.Add(new SelectListItem
                    {
                        Text = item.StateName,
                        Value = Convert.ToString(item.StateId),
                        Selected = true
                    });
                }
            }
            //Bind city by state id
            var cities = CityList(complaientById.StateId);
            if (cities != null)
            {
                foreach (var item in cities)
                {
                    complainantViewModel.Cities.Add(new SelectListItem
                    {
                        Text = item.CityName,
                        Value = Convert.ToString(item.CityId),
                        Selected = true
                    });
                }
            }
            //Bind complaient category
            var complaientCategory = ComplaientCategoryList().ToList();
            if (complaientCategory!= null)
            {
                foreach (var item in complaientCategory)
                {
                    complainantViewModel.ComplaientCategory.Add(new SelectListItem { Text = item.Description, Value = item.ComplaientCategoryId.ToString(), Selected = true });
                }
            }
            //Bind gender
            var gender = Genders().ToList();
            if (gender!= null)
            {
                foreach (var item in gender)
                {
                    complainantViewModel.Gender.Add(new SelectListItem { Text = item.GenderName, Value = item.GenderId.ToString(), Selected = true });
                }
            }
            //Bind Bike Category for check boxes
            var bikeCategory = BikeCategory().ToList();
            if (bikeCategory!= null)
            {
                complainantViewModel.BikeCategories = bikeCategory.Select(m => new BikeCategory()
                {
                    BikeCategoryId = m.BikeCategoryId,
                    Name = m.Name,
                    Description = m.Description,
                    Status = updateBikeCategories.Any(x => x.BikeCategoryId == m.BikeCategoryId) ? true : false
                }).ToList();
            }
            //Bind Bike Category for multiple dropdown select list
            var bikeCategoriesSelectList = BikeCategory().ToList();
            if (bikeCategoriesSelectList!= null)
            {
                foreach (var item in bikeCategoriesSelectList)
                {
                    complainantViewModel.BikeCategoriesSelectList.Add(new SelectListItem { Text = item.Name, Value = item.BikeCategoryId.ToString(), Selected = true });
                }
            }

            //Get SingleImageSaveToFolder
            var resultSingleImageSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.SingleImageSaveToFolder) select o).FirstOrDefault();
            if (resultSingleImageSaveToFolder != null)
            {
                FileViewModel singleImageSaveToFolder = new FileViewModel();
                singleImageSaveToFolder.Id = resultSingleImageSaveToFolder.Id;
                singleImageSaveToFolder.Name = resultSingleImageSaveToFolder.Name;
                singleImageSaveToFolder.Path = resultSingleImageSaveToFolder.Path;
                complainantViewModel.SingleImageSaveToFolderViewModel = singleImageSaveToFolder;
            }

            //Get MultipleImageSaveToFolder
            var resultMultipleImageSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.MultipleImageSaveToFolder) select o).ToList();
            if (resultMultipleImageSaveToFolder != null)
            {
                List<FileViewModel> listMultipleImageSaveToFolder = new List<FileViewModel>();
                foreach (var item in resultMultipleImageSaveToFolder)
                {
                    FileViewModel fileMultipleImageSaveToFolder = new FileViewModel();
                    fileMultipleImageSaveToFolder.Id = item.Id;
                    fileMultipleImageSaveToFolder.Name = item.Name;
                    fileMultipleImageSaveToFolder.Path = item.Path;
                    listMultipleImageSaveToFolder.Add(fileMultipleImageSaveToFolder);
                }
                complainantViewModel.MultipleImageSaveToFolderViewModel = listMultipleImageSaveToFolder;
            }

            //Get SingleImageSaveToDatabase
            var resultSingleImageSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.SingleImageSaveToDatabase) select o).FirstOrDefault();
            if (resultSingleImageSaveToDatabase != null)
            {
                FileViewModel fileSingleImageSaveToDatabase = new FileViewModel();
                fileSingleImageSaveToDatabase.Id = resultSingleImageSaveToDatabase.Id;
                string[] name = resultSingleImageSaveToDatabase.Name.Split("_");
                fileSingleImageSaveToDatabase.Name = name[1];
                fileSingleImageSaveToDatabase.ContentType = resultSingleImageSaveToDatabase.ContentType;
                fileSingleImageSaveToDatabase.Data = resultSingleImageSaveToDatabase.Data;
                complainantViewModel.SingleImageSaveToDatabaseViewModel = fileSingleImageSaveToDatabase;
            }

            //Get MultipleImageSaveToDatabase
            var resultMultipleImageSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.MultipleImageSaveToDatabase) select o).ToList();
            if (resultMultipleImageSaveToDatabase!= null)
            {
                List<FileViewModel> listMultipleImageSaveToDatabase = new List<FileViewModel>();
                foreach (var item in resultMultipleImageSaveToDatabase)
                {
                    FileViewModel fileMultipleImageSaveToDatabase = new FileViewModel();
                    fileMultipleImageSaveToDatabase.Id = item.Id;
                    string[] name = item.Name.Split("_");
                    fileMultipleImageSaveToDatabase.Name = name[1];
                    fileMultipleImageSaveToDatabase.ContentType = item.ContentType;
                    fileMultipleImageSaveToDatabase.Data = item.Data;
                    listMultipleImageSaveToDatabase.Add(fileMultipleImageSaveToDatabase);
                }
                complainantViewModel.MultipleImageSaveToDatabaseViewModel = listMultipleImageSaveToDatabase;
            }

            //Get SingleFileSaveToFolder
            var resultSingleFileSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.SingleFileSaveToFolder) select o).FirstOrDefault();
            if (resultSingleFileSaveToFolder != null)
            {
                FileViewModel singleFileSaveToFolder = new FileViewModel();
                singleFileSaveToFolder.Id = resultSingleFileSaveToFolder.Id;
                string[] splitSingleFileToFolderName = resultSingleFileSaveToFolder.Name.Split("_");
                singleFileSaveToFolder.Name = splitSingleFileToFolderName[1];
                complainantViewModel.SingleFileSaveToFolderViewModel = singleFileSaveToFolder;
            }

            //Get MultipleFileSaveToFolder
            var resultMultipleFileSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.MultipleFileSaveToFolder) select o).ToList();
            if (resultMultipleFileSaveToFolder!= null)
            {
                List<FileViewModel> listMultipleFileSaveToFolder = new List<FileViewModel>();
                foreach (var item in resultMultipleFileSaveToFolder)
                {
                    FileViewModel multipleFileSaveToFolder = new FileViewModel();
                    multipleFileSaveToFolder.Id = item.Id;
                    string[] multipleFileSaveToFolderName = item.Name.Split("_");
                    multipleFileSaveToFolder.Name = multipleFileSaveToFolderName[1];
                    listMultipleFileSaveToFolder.Add(multipleFileSaveToFolder);
                }
                complainantViewModel.MultipleFileSaveToFolderViewModel = listMultipleFileSaveToFolder;
            }

            //Get SingleFileSaveToDatabase
            var resultSingleFileSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.SingleFileSaveToDatabase) select o).FirstOrDefault();
            if (resultSingleFileSaveToDatabase != null)
            {
                FileViewModel singleFileSaveToDatabase = new FileViewModel();
                singleFileSaveToDatabase.Id = resultSingleFileSaveToDatabase.Id;
                string[] name = resultSingleFileSaveToDatabase.Name.Split("_");
                singleFileSaveToDatabase.Name = name[1];
                complainantViewModel.SingleFileSaveToDatabaseViewModel = singleFileSaveToDatabase;
            }

            //Get MultipleFileSaveToDatabase
            var resultMultipleFileSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.MultipleFileSaveToDatabase) select o).ToList();
            if (resultMultipleFileSaveToDatabase!= null)
            {
                List<FileViewModel> listMultipleFileSaveToDatabase = new List<FileViewModel>();
                foreach (var item in resultMultipleFileSaveToDatabase)
                {
                    FileViewModel multipleFileSaveToDatabase = new FileViewModel();
                    multipleFileSaveToDatabase.Id = item.Id;
                    string[] name = item.Name.Split("_");
                    multipleFileSaveToDatabase.Name = name[1];
                    listMultipleFileSaveToDatabase.Add(multipleFileSaveToDatabase);
                }
                complainantViewModel.MultipleFileSaveToDatabaseViewModel = listMultipleFileSaveToDatabase;
            }

            //Get Bulk and BulkData table 
            var resultExcelFileDataSaveToDatabase = (from o in DbContext.Bulk where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase) select o).FirstOrDefault();
            if (resultExcelFileDataSaveToDatabase != null)
            {
                var resultExcelFileDataSaveToDatabaseViewModel = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == resultExcelFileDataSaveToDatabase.FileStoreMode select o).FirstOrDefault();
                if (resultExcelFileDataSaveToDatabaseViewModel != null)
                {
                    FileViewModel excelFileDataSaveToDatabaseViewModel = new FileViewModel();
                    excelFileDataSaveToDatabaseViewModel.Id = resultExcelFileDataSaveToDatabaseViewModel.Id;
                    string[] name = resultExcelFileDataSaveToDatabaseViewModel.Name.Split("_");
                    excelFileDataSaveToDatabaseViewModel.Name = name[1];
                    complainantViewModel.ExcelFileDataSaveToDatabaseViewModel = excelFileDataSaveToDatabaseViewModel;
                }
            }

            return complainantViewModel;
        }
        public FileViewModel DownloadFile(int id)
        {
            FileViewModel fileViewModel = new FileViewModel();
            var files = (from o in DbContext.Files
                         where o.Id == id
                         select o).FirstOrDefault();
            if (files != null)
            {
                fileViewModel.Id = files.Id;
                fileViewModel.Name = files.Name;
                fileViewModel.ContentType = files.ContentType;
                fileViewModel.FileEncodingTypes = files.FileEncodingTypes;
                fileViewModel.FileStoreMode = files.FileStoreMode;
                fileViewModel.Path = files.Path;
                fileViewModel.Data = files.Data;
            }
            else
            {
                fileViewModel = null;
            }
            return fileViewModel;
        }
        public List<BulkDatas> GetExcelData(string fileStoreMode)
        {
            var getBulkId = (from o in DbContext.Bulk where o.FileStoreMode == fileStoreMode select o).FirstOrDefault();
            if (getBulkId != null)
            {
                var getBulkData = (from o in DbContext.BulkDatas where o.BulkId == getBulkId.BulkId orderby o.Id descending select o).ToList();
                if (getBulkData!= null)
                {
                    List<BulkDatas> bulkDatas = new List<BulkDatas>();
                    foreach (var item in getBulkData)
                    {
                        BulkDatas bulk = new BulkDatas();
                        bulk.Id = item.Id;
                        bulk.BulkId = item.BulkId;
                        bulk.Name = item.Name;
                        bulk.Email = item.Email;
                        bulk.MobileNumber = item.MobileNumber;
                        bulk.Des = item.Des;
                        bulkDatas.Add(bulk);
                    }
                    return bulkDatas;
                }
            }
            return null;
        }
        public bool UpdateComplaient(ComplainantViewModel model)
        {
            bool isUpdateSucess = false;
            //Update Complaient
            var updateComplaient = (from o in DbContext.Complaients where o.ComplaientId == model.ComplaientId select o).FirstOrDefault();
            if (updateComplaient != null)
            {
                updateComplaient.ComplainantName = model.ComplainantName;
                updateComplaient.ComplainantEmail = model.ComplainantEmail;
                updateComplaient.ComplaientStatus = true;
                updateComplaient.CompaientDate = model.CompaientDate;
                DbContext.SaveChanges();
            }

            //Update ComplaientDatail
            var updateComplaientDetail = (from o in DbContext.ComplaientDetails where o.ComplaientDetailsId == model.ComplaientDetailsId select o).FirstOrDefault();
            if (updateComplaientDetail != null)
            {
                updateComplaientDetail.ComplaientId = model.ComplaientId;
                updateComplaientDetail.ComplaientCategoryId = model.ComplaientCategoryId;
                updateComplaientDetail.GenderId = model.GenderId;
                updateComplaientDetail.CountryId = model.CountryId;
                updateComplaientDetail.StateId = model.StateId;
                updateComplaientDetail.CityId = model.CityId;
                updateComplaientDetail.Description = model.Description;
                updateComplaientDetail.ComplaientDate = model.ComplaientDate;
                DbContext.SaveChanges();
            }

            //Update ComplaientPermamentAddress Table values
            var updatePermanetAddress = (from o in DbContext.ComplaientPermamentAddresses where o.ComplaientDetailsId == model.ComplaientDetailsId select o).FirstOrDefault();
            if (updatePermanetAddress != null)
            {
                updatePermanetAddress.ComplaientDetailsId = model.ComplaientDetailsId;
                updatePermanetAddress.Address = model.PermamentAddress;
                updatePermanetAddress.PostalCode = Convert.ToInt32(model.PermamentAddressPostalCode);
                DbContext.SaveChanges();
            }

            //Update ComplaientTempAddress Table values
            var updateTempAddress = (from o in DbContext.ComplaientTempAddresses where o.ComplaientDetailsId == model.ComplaientDetailsId select o).FirstOrDefault();
            if (updateTempAddress != null)
            {
                updateTempAddress.ComplaientDetailsId = model.ComplaientDetailsId;
                updateTempAddress.Address = model.TempAddress;
                updateTempAddress.PostalCode = Convert.ToInt32(model.TempAddressPostalCode);
                DbContext.SaveChanges();
            }

            //Delete Bike Collections Data then re-insert
            var deleteBikeCategories = (from obj in DbContext.BikeCollections where obj.ComplaientDetailsId == model.ComplaientDetailsId select obj).ToList();
            if (deleteBikeCategories!= null)
            {
                foreach (var item in deleteBikeCategories)
                {
                    DbContext.BikeCollections.Remove(item);
                    DbContext.SaveChanges();
                }
            }

            //Save BikeCategories Table values
            if (model.BikeCategories!= null)
            {
                foreach (var item in model.BikeCategories)
                {
                    if (item.Status == true)
                    {
                        BikeCollection bikeCollection = new BikeCollection();
                        bikeCollection.ComplaientDetailsId = model.ComplaientDetailsId;
                        bikeCollection.BikeCategoryId = item.BikeCategoryId;
                        bikeCollection.Status = item.Status;
                        bikeCollection.CreatedDate = DateTime.Now;
                        DbContext.BikeCollections.Add(bikeCollection);
                        DbContext.SaveChanges();
                    }
                }
            }

            //Update SingleImageSaveToFolder values
            if (model.SingleImageSaveToFolderViewModel != null)
            {
                var updateSingleImageSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.SingleImageSaveToFolder) select o).FirstOrDefault();
                if (updateSingleImageSaveToFolder != null)
                {
                    DbContext.Files.Remove(updateSingleImageSaveToFolder);
                    DbContext.SaveChanges();

                    Files singleImageSaveToFolder = new Files();
                    singleImageSaveToFolder.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleImageSaveToFolder.Name = model.SingleImageSaveToFolderViewModel.Name;
                    singleImageSaveToFolder.ContentType = model.SingleImageSaveToFolderViewModel.ContentType;
                    singleImageSaveToFolder.FileEncodingTypes = model.SingleImageSaveToFolderViewModel.FileEncodingTypes;
                    singleImageSaveToFolder.FileStoreMode = model.SingleImageSaveToFolderViewModel.FileStoreMode;
                    singleImageSaveToFolder.Path = model.SingleImageSaveToFolderViewModel.Path;
                    singleImageSaveToFolder.Data = model.SingleImageSaveToFolderViewModel.Data;
                    DbContext.Files.Add(singleImageSaveToFolder);
                    DbContext.SaveChanges();
                }
            }

            //Update MultipleImageSaveToFolder values
            if (model.MultipleImageSaveToFolderViewModel != null)
            {
                var updateMultipleImageSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.MultipleImageSaveToFolder) select o).ToList();
                if (updateMultipleImageSaveToFolder!= null)
                {
                    foreach (var item in updateMultipleImageSaveToFolder)
                    {
                        DbContext.Files.Remove(item);
                        DbContext.SaveChanges();
                    }
                    foreach (var item in model.MultipleImageSaveToFolderViewModel)
                    {
                        Files listMultipleImageSaveToFolder = new Files();
                        listMultipleImageSaveToFolder.ComplaientDetailsId = model.ComplaientDetailsId;
                        listMultipleImageSaveToFolder.Name = item.Name;
                        listMultipleImageSaveToFolder.ContentType = item.ContentType;
                        listMultipleImageSaveToFolder.FileEncodingTypes = item.FileEncodingTypes;
                        listMultipleImageSaveToFolder.FileStoreMode = item.FileStoreMode;
                        listMultipleImageSaveToFolder.Path = item.Path;
                        listMultipleImageSaveToFolder.Data = item.Data;
                        DbContext.Files.Add(listMultipleImageSaveToFolder);
                        DbContext.SaveChanges();
                    }
                }
            }

            //Update SingleImageSaveToDatabaseViewModel values
            if (model.SingleImageSaveToDatabaseViewModel != null)
            {
                var updateSingleImageSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.SingleImageSaveToDatabase) select o).FirstOrDefault();
                if (updateSingleImageSaveToDatabase != null)
                {
                    DbContext.Files.Remove(updateSingleImageSaveToDatabase);
                    DbContext.SaveChanges();

                    Files singleImageSaveToDatabaseViewModel = new Files();
                    singleImageSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleImageSaveToDatabaseViewModel.Name = model.SingleImageSaveToDatabaseViewModel.Name;
                    singleImageSaveToDatabaseViewModel.ContentType = model.SingleImageSaveToDatabaseViewModel.ContentType;
                    singleImageSaveToDatabaseViewModel.FileEncodingTypes = model.SingleImageSaveToDatabaseViewModel.FileEncodingTypes;
                    singleImageSaveToDatabaseViewModel.FileStoreMode = model.SingleImageSaveToDatabaseViewModel.FileStoreMode;
                    singleImageSaveToDatabaseViewModel.Path = model.SingleImageSaveToDatabaseViewModel.Path;
                    singleImageSaveToDatabaseViewModel.Data = model.SingleImageSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(singleImageSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }
            }

            //Update MultipleImageSaveToDatabase values
            if (model.MultipleImageSaveToDatabaseViewModel!= null)
            {
                var updateMultipleImageSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.MultipleImageSaveToDatabase) select o).ToList();
                if (updateMultipleImageSaveToDatabase!= null)
                {
                    foreach (var item in updateMultipleImageSaveToDatabase)
                    {
                        DbContext.Files.Remove(item);
                        DbContext.SaveChanges();
                    }
                    foreach (var item in model.MultipleImageSaveToDatabaseViewModel)
                    {
                        Files multipleImageSaveToDatabaseViewModel = new Files();
                        multipleImageSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                        multipleImageSaveToDatabaseViewModel.Name = item.Name;
                        multipleImageSaveToDatabaseViewModel.ContentType = item.ContentType;
                        multipleImageSaveToDatabaseViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        multipleImageSaveToDatabaseViewModel.FileStoreMode = item.FileStoreMode;
                        multipleImageSaveToDatabaseViewModel.Path = item.Path;
                        multipleImageSaveToDatabaseViewModel.Data = item.Data;
                        DbContext.Files.Add(multipleImageSaveToDatabaseViewModel);
                        DbContext.SaveChanges();
                    }
                }
            }

            //Update SingleFileSaveToFolder
            if (model.SingleFileSaveToFolderViewModel != null)
            {
                var updateSingleFileSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.SingleFileSaveToFolder) select o).FirstOrDefault();
                if (updateSingleFileSaveToFolder != null)
                {
                    DbContext.Files.Remove(updateSingleFileSaveToFolder);
                    DbContext.SaveChanges();

                    Files singleFileSaveToFolderViewModel = new Files();
                    singleFileSaveToFolderViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleFileSaveToFolderViewModel.Name = model.SingleFileSaveToFolderViewModel.Name;
                    singleFileSaveToFolderViewModel.ContentType = model.SingleFileSaveToFolderViewModel.ContentType;
                    singleFileSaveToFolderViewModel.FileEncodingTypes = model.SingleFileSaveToFolderViewModel.FileEncodingTypes;
                    singleFileSaveToFolderViewModel.FileStoreMode = model.SingleFileSaveToFolderViewModel.FileStoreMode;
                    singleFileSaveToFolderViewModel.Path = model.SingleFileSaveToFolderViewModel.Path;
                    singleFileSaveToFolderViewModel.Data = model.SingleFileSaveToFolderViewModel.Data;
                    DbContext.Files.Add(singleFileSaveToFolderViewModel);
                    DbContext.SaveChanges();
                }
            }

            //Update MultipleFileSaveToFolder
            if (model.MultipleFileSaveToFolderViewModel != null)
            {
                var updateMultipleFileSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.MultipleFileSaveToFolder) select o).ToList();
                if (updateMultipleFileSaveToFolder!= null)
                {
                    foreach (var item in updateMultipleFileSaveToFolder)
                    {
                        DbContext.Files.Remove(item);
                        DbContext.SaveChanges();
                    }

                    foreach (var item in model.MultipleFileSaveToFolderViewModel)
                    {
                        Files listMultipleFileSaveToFolderViewModel = new Files();
                        listMultipleFileSaveToFolderViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                        listMultipleFileSaveToFolderViewModel.Name = item.Name;
                        listMultipleFileSaveToFolderViewModel.ContentType = item.ContentType;
                        listMultipleFileSaveToFolderViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        listMultipleFileSaveToFolderViewModel.FileStoreMode = item.FileStoreMode;
                        listMultipleFileSaveToFolderViewModel.Path = item.Path;
                        listMultipleFileSaveToFolderViewModel.Data = item.Data;
                        DbContext.Files.Add(listMultipleFileSaveToFolderViewModel);
                        DbContext.SaveChanges();
                    }
                }
            }

            //Update SingleFileSaveToDatabase
            if (model.SingleFileSaveToDatabaseViewModel != null)
            {
                var updateSingleFileSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.SingleFileSaveToDatabase) select o).FirstOrDefault();
                if (updateSingleFileSaveToDatabase != null)
                {
                    DbContext.Files.Remove(updateSingleFileSaveToDatabase);
                    DbContext.SaveChanges();

                    Files singleFileSaveToDatabaseViewModel = new Files();
                    singleFileSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleFileSaveToDatabaseViewModel.Name = model.SingleFileSaveToDatabaseViewModel.Name;
                    singleFileSaveToDatabaseViewModel.ContentType = model.SingleFileSaveToDatabaseViewModel.ContentType;
                    singleFileSaveToDatabaseViewModel.FileEncodingTypes = model.SingleFileSaveToDatabaseViewModel.FileEncodingTypes;
                    singleFileSaveToDatabaseViewModel.FileStoreMode = model.SingleFileSaveToDatabaseViewModel.FileStoreMode;
                    singleFileSaveToDatabaseViewModel.Path = model.SingleFileSaveToDatabaseViewModel.Path;
                    singleFileSaveToDatabaseViewModel.Data = model.SingleFileSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(singleFileSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }
            }

            //Update MultipleFileSaveToDatabase
            if (model.MultipleFileSaveToDatabaseViewModel!= null)
            {
                var updateMultipleFileSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.MultipleFileSaveToDatabase) select o).ToList();
                if (updateMultipleFileSaveToDatabase!= null)
                {
                    foreach (var item in updateMultipleFileSaveToDatabase)
                    {
                        DbContext.Files.Remove(item);
                        DbContext.SaveChanges();
                    }
                    foreach (var item in model.MultipleFileSaveToDatabaseViewModel)
                    {
                        Files multipleFileSaveToDatabaseViewModel = new Files();
                        multipleFileSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                        multipleFileSaveToDatabaseViewModel.Name = item.Name;
                        multipleFileSaveToDatabaseViewModel.ContentType = item.ContentType;
                        multipleFileSaveToDatabaseViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        multipleFileSaveToDatabaseViewModel.FileStoreMode = item.FileStoreMode;
                        multipleFileSaveToDatabaseViewModel.Path = item.Path;
                        multipleFileSaveToDatabaseViewModel.Data = item.Data;
                        DbContext.Files.Add(multipleFileSaveToDatabaseViewModel);
                        DbContext.SaveChanges();
                    }
                }
            }

            //Update Bulk and BulkData Table
            if (model.ListOfExcelFileDataSaveToDatabaseViewModel != null)
            {
                var updateBulk = (from o in DbContext.Bulk where o.ComplaientDetailsId == model.ComplaientDetailsId select o).FirstOrDefault();
                if (updateBulk != null)
                {
                    updateBulk.ComplaientDetailsId = model.ComplaientDetailsId;
                    updateBulk.FileStoreMode = Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase);
                    updateBulk.ModifyDate = DateTime.Now;
                    DbContext.SaveChanges();

                    if (updateBulk.BulkId > 0)
                    {
                        var updateBulkDatas = (from o in DbContext.BulkDatas where o.BulkId == updateBulk.BulkId select o).ToList();
                        if (updateBulkDatas!= null)
                        {
                            foreach (var item in updateBulkDatas)
                            {
                                DbContext.BulkDatas.Remove(item);
                                DbContext.SaveChanges();
                            }
                            foreach (var item in model.ListOfExcelFileDataSaveToDatabaseViewModel)
                            {
                                BulkDatas BulkData = new BulkDatas();
                                BulkData.BulkId = updateBulk.BulkId;
                                BulkData.Name = item.Name;
                                BulkData.Des = item.Des;
                                BulkData.Email = item.Email;
                                BulkData.MobileNumber = item.MobileNumber;
                                DbContext.BulkDatas.Add(BulkData);
                                DbContext.SaveChanges();
                            }
                        }
                    }
                }

                //Update ExcelFileDataSaveToDatabaseViewModel
                var updateExcelFileDataSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase) select o).FirstOrDefault();
                if (updateExcelFileDataSaveToDatabase != null)
                {
                    DbContext.Files.Remove(updateExcelFileDataSaveToDatabase);
                    DbContext.SaveChanges();

                    Files excelFileDataSaveToDatabaseViewModel = new Files();
                    excelFileDataSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    excelFileDataSaveToDatabaseViewModel.Name = model.ExcelFileDataSaveToDatabaseViewModel.Name;
                    excelFileDataSaveToDatabaseViewModel.ContentType = model.ExcelFileDataSaveToDatabaseViewModel.ContentType;
                    excelFileDataSaveToDatabaseViewModel.FileEncodingTypes = model.ExcelFileDataSaveToDatabaseViewModel.FileEncodingTypes;
                    excelFileDataSaveToDatabaseViewModel.FileStoreMode = model.ExcelFileDataSaveToDatabaseViewModel.FileStoreMode;
                    excelFileDataSaveToDatabaseViewModel.Path = model.ExcelFileDataSaveToDatabaseViewModel.Path;
                    excelFileDataSaveToDatabaseViewModel.Data = model.ExcelFileDataSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(excelFileDataSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }
            }
            isUpdateSucess = true;

            return isUpdateSucess;
        }
        public string GetFileNameById(int id)
        {
            return DbContext.Files.Where(x => x.Id == id).Select(s => s.Name).FirstOrDefault();
        }
        public List<string> GetAllFiles(string fileStoreMode, int complaientDetailsId)
        {
            return DbContext.Files.Where((x => x.ComplaientDetailsId == complaientDetailsId && x.FileStoreMode == fileStoreMode)).Select(s => s.Name).ToList();
        }
    }
}
