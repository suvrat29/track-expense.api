using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using track_expense.api.DatabaseAccess;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.TableOps.TableClasses
{
    public class UseractivitylogProvider : IUseractivitylogProvider
    {
        #region Variables
        private readonly PostgreSQLContext _dbcontext;
        #endregion

        #region Constructor
        public UseractivitylogProvider(PostgreSQLContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion

        #region CRUD Operations
        public async Task logUseractivityAsync(UseractivitylogVM userActivity)
        {
            await _dbcontext.useractivitylog.AddAsync(userActivity);
            await _dbcontext.SaveChangesAsync();
        }

        public void logUseractivity(UseractivitylogVM userActivity)
        {
            _dbcontext.useractivitylog.Add(userActivity);
            _dbcontext.SaveChanges();
        }

        public async Task<List<UseractivitylogVM>> getUseractivityAsync(long userId)
        {
            return await _dbcontext.useractivitylog.FromSqlRaw(GetUserActivitySQL(userId)).ToListAsync();
        }

        public List<UseractivitylogVM> getUseractivity(long userId)
        {
            return _dbcontext.useractivitylog.FromSqlRaw(GetUserActivitySQL(userId)).ToList();
        }
        #endregion

        #region SQL Queries
        public string GetUserActivitySQL(long userId)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.Append("SELECT ");
            _sql.Append("id, ");
            _sql.Append("activitytype, ");
            _sql.Append("actiontype, ");
            _sql.Append("userid, ");
            _sql.Append("actiondate, ");
            _sql.Append("actionfield1, ");
            _sql.Append("actionfield2 ");
            _sql.Append("FROM useractivitylog ");
            _sql.Append($"WHERE userid = {userId} ");

            return _sql.ToString();
        }
        #endregion
    }
}
