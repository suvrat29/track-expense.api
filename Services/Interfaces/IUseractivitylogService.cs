using System.Collections.Generic;
using System.Threading.Tasks;
using track_expense.api.Enums;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Services.Interfaces
{
    public interface IUseractivitylogService
    {
        Task addUserActivityAsync(ActivityTypeEnum activityType, ActivityActionTypeEnum actionType, string username, string action1, string action2);
        void addUserActivity(ActivityTypeEnum activityType, ActivityActionTypeEnum actionType, string username, string action1, string action2);
        Task<List<UseractivitylogVM>> getUseractivityAsync(string username);
    }
}
