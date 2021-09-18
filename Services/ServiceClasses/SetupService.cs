using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using track_expense.api.Enums;
using track_expense.api.Extensions;
using track_expense.api.Services.Interfaces;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Services.ServiceClasses
{
    public class SetupService : ISetupService
    {
        #region Variables
        private readonly IApplogService _applogService;
        private readonly ICategorydataProvider _categoryProvider;
        private readonly ISubcategorydataProvider _subcategoryProvider;
        private readonly IMemCacheService _memCacheService;
        #endregion

        #region Constructor
        public SetupService(IApplogService applogService, ICategorydataProvider categoryProvider, ISubcategorydataProvider subcategoryProvider, IMemCacheService memCacheService)
        {
            _applogService = applogService;
            _categoryProvider = categoryProvider;
            _subcategoryProvider = subcategoryProvider;
            _memCacheService = memCacheService;
        }
        #endregion

        #region Public Methods
        public async Task<List<CategorydataVM>> getAllCategoriesWithSubcategoriesAsync(string username)
        {
            try
            {
                List<CategorydataVM> _categoriesList = await _categoryProvider.GetCategoryListAsync(_memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id, true);

                return _categoriesList.OrderBy(cat => cat.id).ToList();
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupService.cs", "getAllCategoriesWithSubcategoriesAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        public async Task<List<CategorydataVM>> getAllCategoriesNoSubcategoryAsync(string username)
        {
            try
            {
                List<CategorydataVM> _categoriesList = await _categoryProvider.GetCategoryListAsync(_memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id, false);

                return _categoriesList.OrderBy(cat => cat.id).ToList();
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupService.cs", "getAllCategoriesNoSubcategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        public async Task<List<SubcategorydataVM>> getAllSubcategoriesByCategoryAsync(long categoryId, string username)
        {
            try
            {
                List<SubcategorydataVM> _subcategoriesList = await _subcategoryProvider.GetSubcategoryListAsync(categoryId, _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id);

                return _subcategoriesList.OrderBy(subcat => subcat.id).ToList();
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupService.cs", "getAllSubcategoriesByCategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        public async Task<bool> addNewCategoryAsync(AddNewCategoryVM newCategoryData, string username)
        {
            try
            {
                CategorydataVM _newCategory = new CategorydataVM();

                _newCategory.name = string.IsNullOrWhiteSpace(newCategoryData.name) ? throw new ApplicationException("Category name cannot be blank") : newCategoryData.name;
                _newCategory.type = newCategoryData.type == 0 ? CategoryDataEnum.TYPE_EXPENSE : newCategoryData.type == 1 ? CategoryDataEnum.TYPE_INCOME : throw new ApiErrorResponse("Unrecognized expense type");
                _newCategory.icon = string.IsNullOrWhiteSpace(newCategoryData.icon) ? "" : newCategoryData.icon;
                _newCategory.description = string.IsNullOrWhiteSpace(newCategoryData.description) ? "" : newCategoryData.description;
                _newCategory.createdby = _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id;
                _newCategory.datecreated = DateTime.Now.ToUniversalTime();

                await _categoryProvider.CreateNewCategoryAsync(_newCategory);

                BackgroundJob.Enqueue<IUseractivitylogService>(activity => activity.addUserActivityAsync(ActivityTypeEnum.Category, ActivityActionTypeEnum.Create, username, _newCategory.name, ""));

                return true;
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupService.cs", "addNewCategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        public async Task<bool> updateCategoryDetailsAsync(UpdateCategoryVM categoryData, string username)
        {
            try
            {
                if (await _categoryProvider.UpdateCategoryDetailsAsync(categoryData, _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id))
                {
                    //TODO: Find updated fields
                    BackgroundJob.Enqueue<IUseractivitylogService>(activity => activity.addUserActivityAsync(ActivityTypeEnum.Category, ActivityActionTypeEnum.Modify, username, "", ""));

                    return true;
                }
                else
                {
                    throw new ApiErrorResponse("Failed to update category");
                }
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupService.cs", "updateCategoryDetailsAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        public async Task<bool> deleteCategoryAsync(long categoryId, string username)
        {
            try
            {
                //TODO: Find category name that was deleted
                await _categoryProvider.DeleteCategoryAsync(categoryId, _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id);

                BackgroundJob.Enqueue<IUseractivitylogService>(activity => activity.addUserActivityAsync(ActivityTypeEnum.Category, ActivityActionTypeEnum.Delete, username, "", ""));

                return true;
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupService.cs", "deleteCategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        public async Task<bool> addNewSubcategoryAsync(AddNewSubcategoryVM newSubcategoryData, string username)
        {
            try
            {
                SubcategorydataVM _newSubcategory = new SubcategorydataVM();

                _newSubcategory.name = string.IsNullOrWhiteSpace(newSubcategoryData.name) ? throw new ApplicationException("Name of subcategory cannot be null") : newSubcategoryData.name;
                _newSubcategory.categoryid = newSubcategoryData.categoryId > 0 ? newSubcategoryData.categoryId : throw new ApplicationException("A category must be selected before adding a subcategory");
                _newSubcategory.icon = string.IsNullOrWhiteSpace(newSubcategoryData.icon) ? "" : newSubcategoryData.icon;
                _newSubcategory.createdby = _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id;
                _newSubcategory.datecreated = DateTime.Now.ToUniversalTime();

                await _subcategoryProvider.CreateNewSubcategoryAsync(_newSubcategory);

                BackgroundJob.Enqueue<IUseractivitylogService>(activity => activity.addUserActivityAsync(ActivityTypeEnum.Subcategory, ActivityActionTypeEnum.Create, username, _newSubcategory.name, ""));

                return true;
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupService.cs", "addNewSubcategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        public async Task<bool> updateSubcategoryDetailsAsync(UpdateSubcategoryVM subcategoryData, string username)
        {
            try
            {
                if (await _subcategoryProvider.UpdateSubcategoryDetailsAsync(subcategoryData, _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id))
                {
                    //TODO: Find updated fields
                    BackgroundJob.Enqueue<IUseractivitylogService>(activity => activity.addUserActivityAsync(ActivityTypeEnum.Subcategory, ActivityActionTypeEnum.Modify, username, "", ""));

                    return true;
                }
                else
                {
                    throw new ApiErrorResponse("Failed to update subcategory");
                }
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupService.cs", "updateSubcategoryDetailsAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        public async Task<bool> deleteSubcategoryAsync(long subcategoryId, string username)
        {
            try
            {
                //TODO: Find subcategory name that was deleted
                await _subcategoryProvider.DeleteSubcategoryAsync(subcategoryId, _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id);

                BackgroundJob.Enqueue<IUseractivitylogService>(activity => activity.addUserActivityAsync(ActivityTypeEnum.Subcategory, ActivityActionTypeEnum.Delete, username, "", ""));

                return true;
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupService.cs", "deleteSubcategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }
        #endregion
    }
}
