using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using track_expense.api.DatabaseAccess;
using track_expense.api.Extensions;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.TableOps.TableClasses
{
    public class EmaildataProvider : IEmaildataProvider
    {
        #region Variables
        private readonly PostgreSQLContext _dbcontext;
        #endregion

        #region Constructor
        public EmaildataProvider(PostgreSQLContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion

        #region Fetch Operations
        public EmaildataVM getEmailTemplate(string templateName)
        {
            EmaildataVM template = _dbcontext.emaildata.FromSqlRaw($"SELECT * FROM emaildata WHERE name = '{templateName}'").OrderBy(x => x.id).LastOrDefault();

            if (template == null)
            {
                throw new ApiErrorResponse("Email template not found");
            }
            else
            {
                return template;
            }
        }

        public async Task<EmaildataVM> getEmailTemplateAsync(string templateName)
        {
            EmaildataVM template = await _dbcontext.emaildata.FromSqlRaw($"SELECT * FROM emaildata WHERE name = '{templateName}'").OrderBy(x => x.id).LastOrDefaultAsync();

            if (template == null)
            {
                throw new ApiErrorResponse("Email template not found");
            }
            else
            {
                return template;
            }
        }
        #endregion
    }
}
