using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Sitecore.MembershipAPI.Handlers;
using Sitecore.MembershipAPI.Models;

namespace Sitecore.MembershipAPI.Controllers
{
    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserHandler _userHandler;

        public AccountController(IConfiguration configuration)
        {
            _userHandler = new UserHandler(configuration);
        }

        // POST api/account
        [HttpPost("validateUser")]
        public UserProfile ValidateUser([FromBody] LoginRequest request)
        {
            return _userHandler.ValidateUser(request);
        }

        [HttpPost("registerUser")]
        public RegisterUserResponse Register([FromBody] LoginRequest request)
        {
            return _userHandler.RegisterUser(request);
        }
    }
}
