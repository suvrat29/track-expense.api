using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using track_expense.api.DatabaseAccess;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.TableOps.TableClasses
{
    public class LocalesProvider : ILocalesProvider
    {
        #region Variables
        private readonly PostgreSQLContext _dbcontext;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public LocalesProvider(PostgreSQLContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }
        #endregion

        #region Fetch Operations
        public async Task<List<LocaleRegionList>> GetRegionListAsync()
        {
            List<LocalesVM> _locales = await _dbcontext.locale.ToListAsync();

            List<LocaleRegionList> _regions = _mapper.Map<List<LocaleRegionList>>(_locales);

            return _regions;
        }

        public async Task<List<LocaleCurrencyList>> GetCurrencyListAsync()
        {
            List<LocalesVM> _locales = await _dbcontext.locale.ToListAsync();

            List<LocaleCurrencyList> _currencies = _mapper.Map<List<LocaleCurrencyList>>(_locales);

            return _currencies;
        }
        #endregion
    }
}
