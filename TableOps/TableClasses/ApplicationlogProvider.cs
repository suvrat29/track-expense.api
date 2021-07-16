using System.Threading.Tasks;
using track_expense.api.DatabaseAccess;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.ViewModels;

namespace track_expense.api.TableOps.TableClasses
{
    public class ApplicationlogProvider : IApplicationlogProvider
    {
        #region Variables
        private readonly PostgreSQLContext _dbcontext;
        #endregion

        #region Constructor
        public ApplicationlogProvider(PostgreSQLContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion

        #region CRUD Operations
        public async Task logErrorAsync(ApplicationlogVM appLog)
        {
            await _dbcontext.applog.AddAsync(appLog);
            await _dbcontext.SaveChangesAsync();
        }

        public void logError(ApplicationlogVM appLog)
        {
            _dbcontext.applog.Add(appLog);
            _dbcontext.SaveChanges();
        }
        #endregion
    }
}
