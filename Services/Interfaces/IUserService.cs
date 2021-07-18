using System.Threading.Tasks;
using track_expense.api.ApiResponseModels;
using track_expense.api.ViewModels;

namespace track_expense.api.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> registerUserAsync(UserRegisterVM registrationData);
        Task<bool> verifyUserAsync(UserEmailVerifyVM verificationData);
        Task<(bool, string, UserLoginResponse)> loginUserAsync(UserLoginVM userCredentials);
        Task<(bool, string)> userForgotPasswordAsync(string email);
        Task<bool> userResetPasswordAsync(string email, string resetKey, string password);
        void logoutUser(string email);
    }
}
