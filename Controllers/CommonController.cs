using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using track_expense.api.ApiResponseModels;
using track_expense.api.Enums;
using track_expense.api.Services.Interfaces;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Common")]
    public class CommonController : BaseController
    {
        #region Variables
        private readonly IHttpContextAccessor _context;
        private readonly IApplogService _applogService;
        private readonly IMemCacheService _memCacheService;
        #endregion

        #region Constructor
        public CommonController(IApplogService applogService, IMemCacheService memCacheService, IHttpContextAccessor context) : base(context)
        {
            _applogService = applogService;
            _memCacheService = memCacheService;
            _context = context;
        }
        #endregion

        #region Routes

        #region GET Methods
        [HttpGet("data")]
        public async Task<IActionResult> getLoggedInUserDetails()
        {
            try
            {
                return Ok(_memCacheService.GetValueFromCache<UserLoginResponse>(base._userName, CacheKeyConstants.USER_LOGIN_CACHE_STORE));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "CommonController.cs", "getLoggedInUserDetails()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER__CACHE_STORE));
                throw;
            }
        }
        #endregion
        
        #endregion
    }
}
