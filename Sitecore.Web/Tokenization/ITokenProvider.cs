using System.Collections.Generic;
using System.Security.Claims;

namespace Sitecore.Web.Tokenization
{
    /// <summary>
    /// JWT token provider
    /// </summary>
    public interface ITokenProvider
    {
        string CreateToken(IEnumerable<Claim> claims);
    }
}