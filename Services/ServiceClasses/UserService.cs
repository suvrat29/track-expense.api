using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using track_expense.api.ApiResponseModels;
using track_expense.api.Enums;
using track_expense.api.Extensions;
using track_expense.api.Services.Interfaces;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.Utils;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Services.ServiceClasses
{
    public class UserService : IUserService
    {
        #region Variables
        private readonly IUserModelProvider _userModel;
        private readonly IApplogService _applogService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMemCacheService _memCacheService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public UserService(IUserModelProvider userModel, IApplogService applogService, IEmailService emailService, IConfiguration configuration, IMemCacheService memCacheService, IMapper mapper)
        {
            _userModel = userModel;
            _applogService = applogService;
            _emailService = emailService;
            _configuration = configuration;
            _memCacheService = memCacheService;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods
        public async Task<bool> registerUserAsync(UserRegisterVM registrationData)
        {
            try
            {
                if (!await _userModel.UserAlreadyExistsAsync(registrationData.email))
                {
                    string _emailTemplate = "";

                    using (StreamReader sr = new StreamReader(Path.GetFullPath(EmailTemplateKeys.TEMPLATES_BASE_PATH + EmailTemplateKeys.USER_VERIFY_EMAIL)))
                    {
                        _emailTemplate = await sr.ReadToEndAsync().ConfigureAwait(false);
                    }

                    UserModelVM _userData = new UserModelVM();
                    string resetkey = "";
                    (byte[], byte[]) passwordData;
                    _userData.avatar = registrationData.avatar;
                    _userData.email = registrationData.email;
                    _userData.firstname = registrationData.firstname;
                    _userData.lastname = registrationData.lastname;
                    passwordData = CommonUtils.GenerateUserPassword(registrationData.password);
                    _userData.passwordHash = passwordData.Item1;
                    _userData.passwordSalt = passwordData.Item2;
                    _userData.invited = true;
                    _userData.verified = false;
                    resetkey = CommonUtils.generateResetKey();
                    while (await _userModel.ResetKeyExistsAsync(resetkey))
                    {
                        resetkey = CommonUtils.generateResetKey();
                    }
                    _userData.resetkey = resetkey;
                    _userData.disabled = false;
                    _userData.deleted = false;
                    _userData.dateinvited = DateTime.Now.ToUniversalTime();
                    _userData.datecreated = DateTime.Now.ToUniversalTime();

                    await _userModel.CreateUserAccountAsync(_userData);

                    await _emailService.SendEmailAsync(_configuration["EmailSettings:SenderEmail"], registrationData.email, "TrackExpense Account Verification", _emailTemplate.Replace("^email^", Convert.ToBase64String(Encoding.UTF8.GetBytes(registrationData.email))).Replace("^resetkey^", Convert.ToBase64String(Encoding.UTF8.GetBytes(_userData.resetkey))));

                    return true;
                }
                else
                {
                    throw new ApiErrorResponse("The user already exists");
                }
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserService.cs", "registerUserAsync()");
                throw;
            }
        }

        public async Task<bool> verifyUserAsync(UserEmailVerifyVM verificationData)
        {
            try
            {
                UserModelVM _user = await _userModel.GetUserAccountByEmailAsync(Encoding.UTF8.GetString(Convert.FromBase64String(verificationData.email)));

                if (_user != null)
                {
                    if (_user.verified)
                    {
                        throw new ApiErrorResponse("Your account is already verified");
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(_user.resetkey) && _user.resetkey == Encoding.UTF8.GetString(Convert.FromBase64String(verificationData.resetkey)))
                        {
                            string _emailTemplate = "";

                            using (StreamReader sr = new StreamReader(Path.GetFullPath(EmailTemplateKeys.TEMPLATES_BASE_PATH + EmailTemplateKeys.USER_VERIFY_EMAIL_SUCCESS)))
                            {
                                _emailTemplate = await sr.ReadToEndAsync().ConfigureAwait(false);
                            }

                            _user.invited = false;
                            _user.resetkey = "";
                            _user.verified = true;
                            _user.dateverified = DateTime.Now.ToUniversalTime();
                            _user.modifiedby = _user.id;
                            _user.datemodified = DateTime.Now.ToUniversalTime();

                            await _userModel.UpdateUserDetailsAsync(_user);

                            await _emailService.SendEmailAsync(_configuration["EmailSettings:SenderEmail"], _user.email, "TrackExpense Account Verified", _emailTemplate);

                            return true;
                        }
                        else
                        {
                            throw new ApiErrorResponse("Invalid reset key");
                        }
                    }
                }
                else
                {
                    throw new ApiErrorResponse("User not found");
                }
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserService.cs", "verifyUserAsync()");
                throw;
            }
        }

        public async Task<(bool, string, UserLoginResponse)> loginUserAsync(UserLoginVM userCredentials)
        {
            try
            {
                UserModelVM _user = await _userModel.GetUserAccountByEmailAsync(userCredentials.email);

                if (_user != null)
                {
                    if (!_user.verified)
                    {
                        return (false, "User not verified", null);
                    }
                    else
                    {
                        if (_user.disabled)
                        {
                            return (false, "User disabled", null);
                        }
                        else
                        {
                            if (_user.deleted)
                            {
                                return (false, "User deleted", null);
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(_user.resetkey))
                                {
                                    if (CommonUtils.VerifyPasswordHash(userCredentials.password, _user.passwordHash, _user.passwordSalt))
                                    {
                                        //Set logged in user's detail in cache
                                        _memCacheService.SetValueInCache<UserLoginResponse>(_user.email, CacheKeyConstants.USER_LOGIN_CACHE_STORE, _mapper.Map<UserLoginResponse>(_user));

                                        _memCacheService.SetValueInCache<UserModelVM>(_user.email, CacheKeyConstants.USER_CACHE_STORE, _user);

                                        return (true, BuildUserIdentity(_user), _mapper.Map<UserLoginResponse>(_user));
                                    }
                                    else
                                    {
                                        return (false, "Incorrect password", null);
                                    }
                                }
                                else
                                {
                                    return (false, "User forgot password", null);
                                }
                            }
                        }
                    }
                }
                else
                {
                    return (false, "User not found", null);
                }
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserService.cs", "loginUserAsync()");
                throw;
            }
        }

        public async Task<(bool, string)> userForgotPasswordAsync(UserForgotPasswordVM forgotPasswordData)
        {
            try
            {
                UserModelVM _user = await _userModel.GetUserAccountByEmailAsync(forgotPasswordData.email);
                string resetkey = "";

                if (_user != null)
                {
                    if (!_user.verified)
                    {
                        return (false, "User not verified");
                    }
                    else
                    {
                        if (_user.disabled)
                        {
                            return (false, "User disabled");
                        }
                        else
                        {
                            if (_user.deleted)
                            {
                                return (false, "User deleted");
                            }
                            else
                            {
                                string _emailTemplate = "";

                                using (StreamReader sr = new StreamReader(Path.GetFullPath(EmailTemplateKeys.TEMPLATES_BASE_PATH + EmailTemplateKeys.USER_FORGOT_PASSWORD)))
                                {
                                    _emailTemplate = await sr.ReadToEndAsync().ConfigureAwait(false);
                                }

                                resetkey = CommonUtils.generateResetKey();
                                while (await _userModel.ResetKeyExistsAsync(resetkey))
                                {
                                    resetkey = CommonUtils.generateResetKey();
                                }
                                _user.resetkey = resetkey;
                                _user.passwordHash = new byte[0];
                                _user.passwordSalt = new byte[0];
                                _user.modifiedby = _user.id;
                                _user.datemodified = DateTime.Now.ToUniversalTime();

                                await _userModel.UpdateUserDetailsAsync(_user);

                                await _emailService.SendEmailAsync(_configuration["EmailSettings:SenderEmail"], forgotPasswordData.email, "TrackExpense Forgot Password", _emailTemplate.Replace("^email^", Convert.ToBase64String(Encoding.UTF8.GetBytes(forgotPasswordData.email))).Replace("^resetkey^", Convert.ToBase64String(Encoding.UTF8.GetBytes(_user.resetkey))));

                                return (true, "Email sent");
                            }
                        }
                    }
                }
                else
                {
                    return (false, "User not found");
                }
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserService.cs", "userForgotPasswordAsync()");
                throw;
            }
        }

        public async Task<bool> userResetPasswordAsync(UserResetPasswordVM resetPasswordData)
        {
            try
            {
                UserModelVM _user = await _userModel.GetUserAccountByEmailAsync(Encoding.UTF8.GetString(Convert.FromBase64String(resetPasswordData.email)));

                if (_user != null)
                {
                    if (!_user.verified)
                    {
                        return false;
                    }
                    else
                    {
                        if (_user.disabled)
                        {
                            return false;
                        }
                        else
                        {
                            if (_user.deleted)
                            {
                                return false;
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(_user.resetkey) && _user.resetkey == Encoding.UTF8.GetString(Convert.FromBase64String(resetPasswordData.resetkey)))
                                {
                                    (byte[], byte[]) passwordData;
                                    string _emailTemplate = "";

                                    using (StreamReader sr = new StreamReader(Path.GetFullPath(EmailTemplateKeys.TEMPLATES_BASE_PATH + EmailTemplateKeys.USER_RESET_PASSWORD_SUCCESS)))
                                    {
                                        _emailTemplate = await sr.ReadToEndAsync().ConfigureAwait(false);
                                    }

                                    passwordData = CommonUtils.GenerateUserPassword(resetPasswordData.password);
                                    _user.passwordHash = passwordData.Item1;
                                    _user.passwordSalt = passwordData.Item2;
                                    _user.resetkey = "";
                                    _user.modifiedby = _user.id;
                                    _user.datemodified = DateTime.Now.ToUniversalTime();

                                    await _userModel.UpdateUserDetailsAsync(_user);

                                    await _emailService.SendEmailAsync(_configuration["EmailSettings:SenderEmail"], _user.email, "TrackExpense Password Updated", _emailTemplate);

                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserService.cs", "userResetPasswordAsync()");
                throw;
            }
        }

        public void logoutUser(string email)
        {
            try
            {
                //Remove logged in user's cache
                _memCacheService.ClearAllUserSpecificCache(email);
            }
            catch (Exception ex)
            {
                _applogService.addErrorLog(ex, "Exception", "UserService.cs", "logoutUser()");
                throw;
            }
        }
        #endregion

        #region Private Methods
        private string BuildUserIdentity(UserModelVM _user)
        {
            try
            {
                Claim[] claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, _user.id.ToString()),
                    new Claim(ClaimTypes.Name, _user.email)
                };

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Token"]));

                SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(7),
                    SigningCredentials = creds
                };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _applogService.addErrorLog(ex, "Exception", "UserService.cs", "BuildUserIdentity()");
                throw;
            }
        }
        #endregion
    }
}
