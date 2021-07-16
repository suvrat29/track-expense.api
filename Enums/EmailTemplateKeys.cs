namespace track_expense.api.Enums
{
    public static class EmailTemplateKeys
    {
        public const string TEMPLATES_BASE_PATH = @".\EmailTemplates\";
        public const string USER_VERIFY_EMAIL = "VerifyUserEmail.html";
        public const string USER_VERIFY_EMAIL_SUCCESS = "UserEmailVerified.html";
        public const string USER_FORGOT_PASSWORD = "UserForgotPassword.html";
        public const string USER_RESET_PASSWORD_SUCCESS = "UserPasswordResetSuccess.html";
    }
}
