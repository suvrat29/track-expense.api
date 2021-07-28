using System.Threading.Tasks;
using track_expense.api.ApiResponseModels;
using track_expense.api.ViewModels.ControllerVM;

namespace track_expense.api.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> registerUserAsync(UserRegisterVM registrationData);
        Task<bool> verifyUserAsync(UserEmailVerifyVM verificationData);
        Task<(bool, string, UserLoginResponse)> loginUserAsync(UserLoginVM userCredentials);
        Task<(bool, string)> userForgotPasswordAsync(UserForgotPasswordVM forgotPasswordData);
        Task<bool> userResetPasswordAsync(UserResetPasswordVM resetPasswordData);
        void logoutUser(string email);
    }
}
