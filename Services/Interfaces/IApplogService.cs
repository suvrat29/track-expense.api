using System;
using System.Threading.Tasks;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Services.Interfaces
{
    public interface IApplogService
    {
        #nullable enable
        Task addErrorLogAsync(Exception ex, string type, string page, string function, UserModelVM? _user = null);
        void addErrorLog(Exception ex, string type, string page, string function, UserModelVM? _user = null);
        #nullable disable
    }
}
