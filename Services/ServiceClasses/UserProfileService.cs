using System;
using System.Threading.Tasks;
using track_expense.api.Enums;
using track_expense.api.Services.Interfaces;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Services.ServiceClasses
{
    public class UserProfileService : IUserProfileService
    {
        #region Variables
        private readonly ILocalesProvider _locales;
        private readonly IApplogService _applogService;
        private readonly IUserModelProvider _userModel;
        private readonly IMemCacheService _memCacheService;
        #endregion

        #region Constructor
        public UserProfileService(ILocalesProvider locales, IApplogService applogService, IUserModelProvider userModel, IMemCacheService memCacheService)
        {
            _locales = locales;
            _applogService = applogService;
            _userModel = userModel;
            _memCacheService = memCacheService;
        }
        #endregion

        #region Public Methods
        public async Task<UserProfileDataVM> getUserProfileDataAsync(string username)
        {
            try
            {
                UserProfileDataVM _profileData = new UserProfileDataVM();
                UserModelVM _user = await _userModel.GetUserAccountByEmailAsync(username);

                _profileData.regionlist = await _locales.GetRegionListAsync();
                _profileData.currencylist = await _locales.GetCurrencyListAsync();

                if (_user != null)
                {
                    _profileData.id = _user.id;
                    _profileData.firstname = _user.firstname;
                    _profileData.lastname = _user.lastname;
                    _profileData.email = _user.email;
                    _profileData.avatar = _user.avatar;
                    _profileData.region = _user.region != null ? (long)_user.region : 0;
                    _profileData.currency = _user.currency != null ? (long)_user.currency : 0;
                }

                return _profileData;
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UserProfileService.cs", "getUserProfileDataAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER__CACHE_STORE));
                throw;
            }
        }
        #endregion
    }
}
