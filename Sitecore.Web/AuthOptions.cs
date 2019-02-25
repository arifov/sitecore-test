using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Sitecore.Web
{
    /// <summary>
    /// JWT token configuration helper storage
    /// </summary>
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "http://localhost:51884/";
        const string KEY = "mysupersecret_secretkey!123";
        public const int LIFETIME = 1; //in minutes

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
