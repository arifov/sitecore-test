namespace Sitecore.Membership
{
    public interface IUserManager
    {
        void Init(string serviceUrl);
        UserProfile Validate(string email, string password);
        ResponseData RegisterUser(string email, string password);
        void Logout(string email);
    }
}