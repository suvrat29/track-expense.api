using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using track_expense.api.Enums;
using track_expense.api.Services.Interfaces;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Setup")]
    public class SetupController : BaseController
    {
        #region Variables
        private readonly IHttpContextAccessor _context;
        private readonly ISetupService _setupService;
        private readonly IApplogService _applogService;
        private readonly IMemCacheService _memCacheService;
        #endregion

        #region Constructor
        public SetupController(IHttpContextAccessor context, ISetupService setupService, IApplogService applogService, IMemCacheService memCacheService) : base(context)
        {
            _context = context;
            _setupService = setupService;
            _applogService = applogService;
            _memCacheService = memCacheService;
        }
        #endregion

        #region Routes

        #region GET Methods
        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategoriesAsync([FromQuery] bool fetchSubcategoryCount)
        {
            try
            {
                if (fetchSubcategoryCount)
                {
                    return Ok(await _setupService.getAllCategoriesWithSubcategoriesAsync(base._userName));
                }
                else
                {
                    return Ok(await _setupService.getAllCategoriesNoSubcategoryAsync(base._userName));
                }
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupController.cs", "GetAllCategoriesAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        [HttpGet("get-all-subcategories")]
        public async Task<IActionResult> GetAllSubcategoriesByCategoryAsync([FromQuery] long categoryId)
        {
            try
            {
                return Ok(await _setupService.getAllSubcategoriesByCategoryAsync(categoryId, base._userName));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupController.cs", "GetAllSubcategoriesByCategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }
        #endregion

        #region POST Methods
        [HttpPost("add-new-category")]
        public async Task<IActionResult> AddNewCategoryAsync(AddNewCategoryVM newCategoryData)
        {
            try
            {
                return Ok(await _setupService.addNewCategoryAsync(newCategoryData, base._userName));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupController.cs", "AddNewCategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        [HttpPost("update-category")]
        public async Task<IActionResult> UpdateCategoryDetailsAsync(UpdateCategoryVM categoryData)
        {
            try
            {
                return Ok(await _setupService.updateCategoryDetailsAsync(categoryData, base._userName));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupController.cs", "UpdateCategoryDetailsAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        [HttpPost("delete-category")]
        public async Task<IActionResult> DeleteCategoryAsync(long categoryId)
        {
            try
            {
                return Ok(await _setupService.deleteCategoryAsync(categoryId, base._userName));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupController.cs", "DeleteCategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        [HttpPost("add-new-subcategory")]
        public async Task<IActionResult> AddNewSubcategoryAsync(AddNewSubcategoryVM newSubcategoryData)
        {
            try
            {
                return Ok(await _setupService.addNewSubcategoryAsync(newSubcategoryData, base._userName));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupController.cs", "AddNewSubcategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        [HttpPost("update-subcategory")]
        public async Task<IActionResult> UpdateSubcategoryAsync(UpdateSubcategoryVM subcategoryData)
        {
            try
            {
                return Ok(await _setupService.updateSubcategoryDetailsAsync(subcategoryData, base._userName));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupController.cs", "UpdateSubcategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }

        [HttpPost("delete-subcategory")]
        public async Task<IActionResult> DeleteSubcategoryAsync(long subcategoryId)
        {
            try
            {
                return Ok(await _setupService.deleteSubcategoryAsync(subcategoryId, base._userName));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "SetupController.cs", "DeleteSubcategoryAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }
        #endregion

        #endregion
    }
}
