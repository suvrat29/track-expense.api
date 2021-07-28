using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using track_expense.api.Services.Interfaces;

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
        #endregion

        #region Constructor
        public UserProfileController(IApplogService applogService, IHttpContextAccessor context, IUserProfileService userProfileService) : base(context)
        {
            _applogService = applogService;
            _context = context;
            _userProfileService = userProfileService;
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
                await _applogService.addErrorLogAsync(ex, "Exception", "UserProfileController.cs", "GetUserProfileDataAsync()");
                throw;
            }
        }
        #endregion

        #region POST Methods
        #endregion

        #endregion
    }
}
