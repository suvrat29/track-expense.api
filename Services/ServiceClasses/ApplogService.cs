using System;
using System.Threading.Tasks;
using track_expense.api.Services.Interfaces;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Services.ServiceClasses
{
    public class ApplogService : IApplogService
    {
        #region Variables
        private readonly IApplicationlogProvider _appLog;
        #endregion

        #region Constructor
        public ApplogService(IApplicationlogProvider appLog)
        {
            _appLog = appLog;
        }
        #endregion

        #region Public Methods
        public async Task addErrorLogAsync(Exception ex, string type, string page, string function, UserModelVM _user)
        {
            ApplicationlogVM _errorLog = new ApplicationlogVM();
            _errorLog.type = type;
            _errorLog.page = page;
            _errorLog.function = function;
            _errorLog.message = ex.InnerException == null ? ex.Message : ex.Message + " Inner exception message: " + ex.InnerException.Message;
            _errorLog.stacktrace = ex.InnerException == null ? ex.StackTrace : ex.StackTrace + " Inner exception stacktrace: " + ex.InnerException.StackTrace;
            _errorLog.userid = _user.id;
            _errorLog.logdate = DateTime.Now.ToUniversalTime();

            await _appLog.logErrorAsync(_errorLog);
        }

        public async Task addErrorLogAsync(Exception ex, string type, string page, string function)
        {
            ApplicationlogVM _errorLog = new ApplicationlogVM();
            _errorLog.type = type;
            _errorLog.page = page;
            _errorLog.function = function;
            _errorLog.message = ex.InnerException == null ? ex.Message : ex.Message + " Inner exception message: " + ex.InnerException.Message;
            _errorLog.stacktrace = ex.InnerException == null ? ex.StackTrace : ex.StackTrace + " Inner exception stacktrace: " + ex.InnerException.StackTrace;
            _errorLog.userid = 0;
            _errorLog.logdate = DateTime.Now.ToUniversalTime();

            await _appLog.logErrorAsync(_errorLog);
        }

        public void addErrorLog(Exception ex, string type, string page, string function, UserModelVM _user)
        {
            ApplicationlogVM _errorLog = new ApplicationlogVM();
            _errorLog.type = type;
            _errorLog.page = page;
            _errorLog.function = function;
            _errorLog.message = ex.InnerException == null ? ex.Message : ex.Message + " Inner exception message: " + ex.InnerException.Message;
            _errorLog.stacktrace = ex.InnerException == null ? ex.StackTrace : ex.StackTrace + " Inner exception stacktrace: " + ex.InnerException.StackTrace;
            _errorLog.userid = _user.id;
            _errorLog.logdate = DateTime.Now.ToUniversalTime();

            _appLog.logError(_errorLog);
        }

        public void addErrorLog(Exception ex, string type, string page, string function)
        {
            ApplicationlogVM _errorLog = new ApplicationlogVM();
            _errorLog.type = type;
            _errorLog.page = page;
            _errorLog.function = function;
            _errorLog.message = ex.InnerException == null ? ex.Message : ex.Message + " Inner exception message: " + ex.InnerException.Message;
            _errorLog.stacktrace = ex.InnerException == null ? ex.StackTrace : ex.StackTrace + " Inner exception stacktrace: " + ex.InnerException.StackTrace;
            _errorLog.userid = 0;
            _errorLog.logdate = DateTime.Now.ToUniversalTime();

            _appLog.logError(_errorLog);
        }
        #endregion
    }
}
