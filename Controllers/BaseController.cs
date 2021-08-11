using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace track_expense.api.Controllers
{
    public class BaseController : Controller
    {
        #region Variables
        protected string _userName = "";
        #endregion

        #region Constructor
        public BaseController(IHttpContextAccessor context)
        {
            string jwt = context.HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();
            JwtSecurityToken _token = _handler.ReadJwtToken(jwt);
            _userName = _token.Claims.First(claim => claim.Type == "unique_name").Value;
        }
        #endregion
    }
}
