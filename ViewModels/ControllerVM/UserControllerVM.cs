namespace track_expense.api.ViewModels.ControllerVM
{
    public class UserControllerVM
    {
    }

    public class UserLoginVM
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class UserRegisterVM
    {
        public string email { get; set; }
        public string password { get; set; }
        public string avatar { get; set; } = "";
        public string firstname { get; set; }
        public string lastname { get; set; } = "";
    }

    public class UserEmailVerifyVM
    {
        public string email { get; set; }
        public string resetkey { get; set; }
    }

    public class UserForgotPasswordVM
    {
        public string email { get; set; }
    }

    public class UserResetPasswordVM
    {
        public string email { get; set; }
        public string resetkey { get; set; }
        public string password { get; set; }
    }
}
