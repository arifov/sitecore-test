using Sitecore.Membership.Data.Database.Dto;

namespace Sitecore.Membership.Data
{
    public interface IUserService
    {
        UserDto ValidateUser(string email, string password);
        UserDto RegisterUser(string email, string password);
    }
}