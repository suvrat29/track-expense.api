using System.Threading.Tasks;
using track_expense.api.ViewModels;

namespace track_expense.api.TableOps.Interfaces
{
    public interface IUserModelProvider
    {
        void CreateUserAccount(UserModelVM userModel);
        Task CreateUserAccountAsync(UserModelVM userModel);
        void UpdateUserDetails(UserModelVM userModel);
        Task UpdateUserDetailsAsync(UserModelVM userModel);
        void DisableUserAccount(long userId);
        Task DisableUserAccountAsync(long userId);
        void DeleteUserAccount(long userId);
        Task DeleteUserAccountAsync(long userId);
        UserModelVM GetUserAccountById(long userId);
        Task<UserModelVM> GetUserAccountByIdAsync(long userId);
        Task<bool> ResetKeyExistsAsync(string resetKey);
        Task<bool> UserAlreadyExistsAsync(string email);
        UserModelVM GetUserAccountByEmail(string email);
        Task<UserModelVM> GetUserAccountByEmailAsync(string email);
    }
}
