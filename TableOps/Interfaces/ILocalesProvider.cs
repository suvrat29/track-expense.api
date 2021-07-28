using System.Collections.Generic;
using System.Threading.Tasks;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.TableOps.Interfaces
{
    public interface ILocalesProvider
    {
        Task<List<LocaleRegionList>> GetRegionListAsync();
        Task<List<LocaleCurrencyList>> GetCurrencyListAsync();
    }
}
