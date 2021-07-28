using System.Threading.Tasks;
using track_expense.api.ViewModels.ControllerVM;

namespace track_expense.api.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfileDataVM> getUserProfileDataAsync(string username);
    }
}
