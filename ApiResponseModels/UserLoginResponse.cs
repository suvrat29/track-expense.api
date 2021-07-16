using System;

namespace track_expense.api.ApiResponseModels
{
    public class UserLoginResponse
    {
        public string email { get; set; }
        public string avatar { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public DateTime dateverified { get; set; }
    }
}
