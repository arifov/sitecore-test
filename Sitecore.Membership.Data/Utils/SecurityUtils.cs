using System.Text;

namespace Sitecore.Membership.Data.Utils
{
    public static class SecurityUtils
    {
        /// <summary>
        /// Basic hash implementation
        /// </summary>
        /// <param name="value">String to hash</param>
        /// <returns>SHA1 hash of input string</returns>
        public static string HashSHA1(string value)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
