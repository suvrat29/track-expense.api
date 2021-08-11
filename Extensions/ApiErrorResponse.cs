using System;

namespace track_expense.api.Extensions
{
    public class ApiErrorResponse : Exception
    {
        public ApiErrorResponse(string message) : base(message) { }
        public ApiErrorResponse(string message, Exception ex) : base(message, ex) { }
    }
}
