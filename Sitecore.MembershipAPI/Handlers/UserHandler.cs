using System;
using Microsoft.Extensions.Configuration;
using Sitecore.Membership.Data;
using Sitecore.MembershipAPI.Models;

namespace Sitecore.MembershipAPI.Handlers
{
    public class UserHandler
    {
        private readonly IUserService _userService;

        public UserHandler(IConfiguration configuration)
        {
            _userService = new UserService(configuration["ConnectionStrings:DefaultConnection"]);
        }

        public UserProfile ValidateUser(LoginRequest request)
        {
            try
            {
                var user = _userService.ValidateUser(request.email, request.password);
                if (user != null)
                    return new UserProfile() { UserId = user.Id, Email = user.Email };
            }
            catch (Exception ex)
            {
                //TODO: add logger
            }

            return null;
        }

        public RegisterUserResponse RegisterUser(LoginRequest request)
        {
            try
            {
                var userDto = _userService.RegisterUser(request.email, request.password);
                var profile = new UserProfile() { UserId = userDto.Id, Email = userDto.Email };
                return new RegisterUserResponse()
                {
                    UserProfile = profile,
                    Message = userDto.ErrorMessage
                };
            }
            catch (Exception ex)
            {
                //TODO: add logger
                return new RegisterUserResponse()
                {
                    Message = "Internal error " + ex.Message
                };
            }
        }
    }
}
