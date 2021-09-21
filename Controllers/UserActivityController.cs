using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using track_expense.api.Enums;
using track_expense.api.Services.Interfaces;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Activity")]
    public class UserActivityController : BaseController
    {
        #region Variables
        private readonly IUseractivitylogService _activityService;
        private readonly IApplogService _applogService;
        private readonly IMemCacheService _memCacheService;
        #endregion

        #region Constructor
        public UserActivityController(IUseractivitylogService activityService, IApplogService applogService, IMemCacheService memCacheService, IHttpContextAccessor context) : base(context)
        {
            _activityService = activityService;
            _applogService = applogService;
            _memCacheService = memCacheService;
        }
        #endregion

        #region Routes

        #region GET Methods
        [HttpGet("get-user-activity")]
        public async Task<IActionResult> GetUserActivityAsync()
        {
            try
            {
                return Ok(await _activityService.getUserActivityAsync(base._userName));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "ActivityController.cs", "GetUserActivityAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }
        #endregion

        #endregion
    }
}
