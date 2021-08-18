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
        private readonly IMemCacheService _memCacheService;
        #endregion

        #region Constructor
        public SetupService(IApplogService applogService, ICategorydataProvider categoryProvider, IMemCacheService memCacheService)
        {
            _applogService = applogService;
            _categoryProvider = categoryProvider;
            _memCacheService = memCacheService;
        }
        #endregion

        #region Public Methods
        public async Task<List<CategorydataVM>> getAllCategoriesAsync(string username)
        {
            try
            {
                List<CategorydataVM> _categoriesList = await _categoryProvider.GetCategoryListAsync(_memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id);

                return _categoriesList.OrderBy(cat => cat.id).ToList();
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupService.cs", "getAllCategoriesAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        public async Task<bool> addNewCategoryAsync(AddNewCategoryVM newCategoryData, string username)
        {
            try
            {
                CategorydataVM _newCategory = new CategorydataVM();

                _newCategory.name = newCategoryData.name;
                _newCategory.type = newCategoryData.type == 0 ? CategoryDataEnum.TYPE_EXPENSE : newCategoryData.type == 1 ? CategoryDataEnum.TYPE_INCOME : throw new ApiErrorResponse("Unrecognized expense type");
                _newCategory.icon = string.IsNullOrWhiteSpace(newCategoryData.icon) ? "" : newCategoryData.icon;
                _newCategory.description = string.IsNullOrWhiteSpace(newCategoryData.description) ? "" : newCategoryData.description;
                _newCategory.createdby = _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id;
                _newCategory.datecreated = DateTime.Now.ToUniversalTime();

                await _categoryProvider.CreateNewCategoryAsync(_newCategory);

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
                await _categoryProvider.DeleteCategoryAsync(categoryId, _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id);

                return true;
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupService.cs", "deleteCategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }
        #endregion
    }
}
