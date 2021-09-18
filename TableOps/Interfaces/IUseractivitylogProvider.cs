using System.Collections.Generic;
using System.Threading.Tasks;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.TableOps.Interfaces
{
    public interface IUseractivitylogProvider
    {
        Task logUseractivityAsync(UseractivitylogVM userActivity);
        void logUseractivity(UseractivitylogVM userActivity);
        Task<List<UseractivitylogVM>> getUseractivityAsync(long userId);
        List<UseractivitylogVM> getUseractivity(long userId);
    }
}
