using System.Threading.Tasks;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.TableOps.Interfaces
{
    public interface IEmaildataProvider
    {
        Task<EmaildataVM> getEmailTemplateAsync(string templateName);
        EmaildataVM getEmailTemplate(string templateName);
    }
}
