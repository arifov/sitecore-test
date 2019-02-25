using Sitecore.Membership.Data.Database;
using Sitecore.Membership.Data.Database.Dto;
using Sitecore.Membership.Data.Database.Entiry;
using Sitecore.Membership.Data.Utils;

namespace Sitecore.Membership.Data
{
    /// <summary>
    /// Incapsulates user registration and validation logic
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserRepository _repository;

        public UserService(string connectionString)
        {
            _repository = new UserRepository(connectionString);
        }

        public UserDto ValidateUser(string email, string password)
        {
            var user = _repository.FindUserByEmail(email);

            if (user != null && user.Password == SecurityUtils.HashSHA1(password))
            {
                return new UserDto()
                {
                    Id = user.Id,
                    Email = user.Email
                };
            }

            return null;
        }

        public UserDto RegisterUser(string email, string password)
        {
            var user = new User { Email = email, Password = SecurityUtils.HashSHA1(password) };

            var userDto = new UserDto()
            {
                Id = user.Id,
                Email = user.Email
            };

            if (!_repository.AddUser(user))
            {
                //TODO: Add message to string resources
                userDto.ErrorMessage = "User with same credentials already exists";
            };

            return userDto;
        }
    }
}
