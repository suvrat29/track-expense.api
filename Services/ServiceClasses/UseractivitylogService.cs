using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using track_expense.api.Enums;
using track_expense.api.Services.Interfaces;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Services.ServiceClasses
{
    public class UseractivitylogService : IUseractivitylogService
    {
        #region Variables
        private readonly IApplogService _applogService;
        private readonly IUseractivitylogProvider _activityLog;
        private readonly IMemCacheService _memCacheService;
        #endregion

        #region Constructor
        public UseractivitylogService(IApplogService applogService, IUseractivitylogProvider activityLog, IMemCacheService memCacheService)
        {
            _applogService = applogService;
            _activityLog = activityLog;
            _memCacheService = memCacheService;
        }
        #endregion

        #region Public Methods
        public async Task addUserActivityAsync(ActivityTypeEnum activityType, ActivityActionTypeEnum actionType, string username, string action1, string action2)
        {
            try
            {
                UseractivitylogVM userActivity = new UseractivitylogVM();
                userActivity.activitytype = (int)activityType;
                userActivity.actiontype = (int)actionType;
                userActivity.userid = _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id;
                userActivity.actiondate = DateTime.Now.ToUniversalTime();
                userActivity.actionfield1 = action1;
                userActivity.actionfield2 = action2;

                await _activityLog.logUseractivityAsync(userActivity);
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UseractivitylogService.cs", "addUserActivityAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
            }
        }

        public void addUserActivity(ActivityTypeEnum activityType, ActivityActionTypeEnum actionType, string username, string action1, string action2)
        {
            try
            {
                UseractivitylogVM userActivity = new UseractivitylogVM();
                userActivity.activitytype = (int)activityType;
                userActivity.actiontype = (int)actionType;
                userActivity.userid = _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id;
                userActivity.actiondate = DateTime.Now.ToUniversalTime();
                userActivity.actionfield1 = action1;
                userActivity.actionfield2 = action2;

                _activityLog.logUseractivity(userActivity);
            }
            catch (Exception ex)
            {
                _applogService.addErrorLog(ex, "Exception", "UseractivitylogService.cs", "addUserActivity()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
            }
        }

        public async Task<List<UseractivitylogVM>> getUserActivityAsync(string username)
        {
            try
            {
                List<UseractivitylogVM> _activityLogData = await _activityLog.getUseractivityAsync(_memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE).id);

                return _activityLogData.OrderByDescending(x => x.id).ToList();
            }
            catch (Exception ex)
            {
                await _applogService.addErrorLogAsync(ex, "Exception", "UseractivitylogService.cs", "getUserActivityAsync()", _memCacheService.GetValueFromCache<UserModelVM>(username, CacheKeyConstants.USER_CACHE_STORE));
                throw;
            }
        }
        #endregion
    }
}
