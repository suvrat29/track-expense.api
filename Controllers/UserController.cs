using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using track_expense.api.ApiResponseModels;
using track_expense.api.Extensions;
using track_expense.api.Services.Interfaces;
using track_expense.api.ViewModels.ControllerVM;

namespace track_expense.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("User")]
    public class UserController : Controller
    {
        #region Variables
        private readonly IUserService _userService;
        private readonly IApplogService _applogService;
        #endregion

        #region Constructor
        public UserController(IUserService userService, IApplogService applogService)
        {
            _userService = userService;
            _applogService = applogService;
        }
        #endregion

        #region Routes

        #region GET Methods
        #endregion

        #region POST Methods
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(UserRegisterVM registrationData)
        {
            try
            {
                return Ok(await _userService.registerUserAsync(registrationData));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserController.cs", "RegisterUserAsync()");
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUserAsync(UserEmailVerifyVM verificationData)
        {
            try
            {
                return Ok(await _userService.verifyUserAsync(verificationData));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserController.cs", "VerifyUserAsync()");
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync(UserLoginVM _userCredentials)
        {
            try
            {
                (bool, string, UserLoginResponse) AuthenticationResult = await _userService.loginUserAsync(_userCredentials);

                if (AuthenticationResult.Item1)
                {
                    return Ok(new
                    {
                        token = AuthenticationResult.Item2,
                        user = AuthenticationResult.Item3
                    });
                }
                else
                {
                    throw new ApiErrorResponse("Invalid Username/Password");
                }
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserController.cs", "LoginUserAsync()");
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> UserForgotPasswordAsync(UserForgotPasswordVM forgotPasswordData)
        {
            try
            {
                (bool, string) ForgotPasswordResult = await _userService.userForgotPasswordAsync(forgotPasswordData);

                if (ForgotPasswordResult.Item1)
                {
                    return Ok(ForgotPasswordResult.Item2);
                }
                else
                {
                    throw new ApiErrorResponse("Failed to perform forgot password action");
                }
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserController.cs", "UserForgotPasswordAsync()");
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("resetpassword")]
        public async Task<IActionResult> UserResetPasswordAsync(UserResetPasswordVM resetPasswordData)
        {
            try
            {
                return Ok(await _userService.userResetPasswordAsync(resetPasswordData));
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserController.cs", "UserResetPasswordAsync()");
                throw;
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUserAsync(string email)
        {
            try
            {
                _userService.logoutUser(email);
                return Ok(true);
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserController.cs", "LogoutUserAsync()");
                throw;
            }
        }
        #endregion

        #endregion
    }
}
