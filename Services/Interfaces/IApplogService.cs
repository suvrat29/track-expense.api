using System;
using System.Threading.Tasks;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Services.Interfaces
{
    public interface IApplogService
    {
        Task addErrorLogAsync(Exception ex, string type, string page, string function, UserModelVM _user);
        Task addErrorLogAsync(Exception ex, string type, string page, string function);
        void addErrorLog(Exception ex, string type, string page, string function, UserModelVM _user);
        void addErrorLog(Exception ex, string type, string page, string function);
    }
}
