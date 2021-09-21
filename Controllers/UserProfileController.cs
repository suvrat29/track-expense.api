using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using track_expense.api.Enums;
using track_expense.api.Services.Interfaces;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Profile")]
    public class UserProfileController : BaseController
    {
        #region Variables
        private readonly IApplogService _applogService;
        private readonly IUserProfileService _userProfileService;
        private readonly IMemCacheService _memCacheService;
        #endregion

        #region Constructor
        public UserProfileController(IApplogService applogService, IHttpContextAccessor context, IUserProfileService userProfileService, IMemCacheService memCacheService) : base(context)
        {
            _applogService = applogService;
            _userProfileService = userProfileService;
            _memCacheService = memCacheService;
        }
        #endregion

        #region Routes

        #region GET Methods
        [HttpGet("get-profile-data")]
        public async Task<IActionResult> GetUserProfileDataAsync()
        {
            try
            {
                return Ok(await _userProfileService.getUserProfileDataAsync(base._userName));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserProfileController.cs", "GetUserProfileDataAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }
        #endregion

        #region POST Methods
        [HttpPost("update-user-profile")]
        public async Task<IActionResult> UpdateUserProfileAsync(UserProfileUpdateVM profileData)
        {
            try
            {
                return Ok(await _userProfileService.updateUserProfileAsync(base._userName, profileData));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserProfileController.cs", "UpdateUserProfileAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }
        #endregion

        #endregion
    }
}
