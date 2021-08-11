namespace track_expense.api.Enums
{
    public static class EmailTemplateKeys
    {
        public static readonly string TEMPLATES_BASE_PATH = @".\EmailTemplates\";
        public static readonly string USER_VERIFY_EMAIL = "VerifyUserEmail.html";
        public static readonly string USER_VERIFY_EMAIL_SUCCESS = "UserEmailVerified.html";
        public static readonly string USER_FORGOT_PASSWORD = "UserForgotPassword.html";
        public static readonly string USER_RESET_PASSWORD_SUCCESS = "UserPasswordResetSuccess.html";
    }
}
