﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using track_expense.api.Enums;
using track_expense.api.Services.Interfaces;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Profile")]
    public class UserProfileController : BaseController
    {
        #region Variables
        private readonly IHttpContextAccessor _context;
        private readonly IApplogService _applogService;
        private readonly IUserProfileService _userProfileService;
        private readonly IMemCacheService _memCacheService;
        #endregion

        #region Constructor
        public UserProfileController(IApplogService applogService, IHttpContextAccessor context, IUserProfileService userProfileService, IMemCacheService memCacheService) : base(context)
        {
            _applogService = applogService;
            _context = context;
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
                await _applogService.addErrorLogAsync(ex, "Exception", "UserProfileController.cs", "GetUserProfileDataAsync()", _memCacheService.GetValueFromCache<UserModelVM>(base._userName, CacheKeyConstants.USER__CACHE_STORE));
                throw;
            }
        }
        #endregion

        #region POST Methods
        #endregion

        #endregion
    }
}
