using System.Linq;
using Dapper;
using Sitecore.Membership.Data.Database.Entiry;

namespace Sitecore.Membership.Data.Database
{
    /// <summary>
    /// User repository. Uses Dapper to access a database
    /// </summary>
    public class UserRepository : BaseRepository
    {
        public UserRepository(string connectionString) : base(connectionString)
        {
        }

        public User FindUserByEmail(string userEmail)
        {
            using (var connection = this.GetConnection())
            {
                return connection.Query<User>(@"SELECT * FROM [Users] WHERE Email = @userEmail", new { userEmail })
                    .FirstOrDefault();
            }
        }

        public bool AddUser(User user)
        {
            using (var connection = this.GetConnection())
            {
                var userId = connection.ExecuteScalar<int?>(
                                    @"IF NOT EXISTS(SELECT * FROM [Users] WHERE Email = @Email)
									BEGIN
										INSERT INTO [Users] (Email, Password)
										VALUES(@Email, @Password)

										SELECT @@IDENTITY
									END", user);
                if (userId.HasValue)
                {
                    user.Id = userId.Value;
                    return true;
                }

                return false;
            }
        }
    }
}
