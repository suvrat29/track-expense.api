using System.Threading.Tasks;

namespace track_expense.api.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string from, string to, string subject, string html);
    }
}
