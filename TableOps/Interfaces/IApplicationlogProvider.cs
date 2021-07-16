using System.Threading.Tasks;
using track_expense.api.ViewModels;

namespace track_expense.api.TableOps.Interfaces
{
    public interface IApplicationlogProvider
    {
        Task logErrorAsync(ApplicationlogVM appLog);
        void logError(ApplicationlogVM appLog);
    }
}
