using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Sitecore.Web.Tokenization;
using Xunit;

namespace Sitecore.Web.Tests
{
    /// <summary>
    /// For more unit test examples please have a look on MyUnitTestsFromAnotherProject folder.
    /// There are few tests from my live application based on xUnit and Moq
    /// </summary>
    public class TokenProviderTests
    {
        [Fact]
        public void CreateToken_NullClaimsProvided_ShouldThrowAnException()
        {
            //arrange
            var tokenProvider = new TokenProvider();

            //act
            void Action()
            {
                var token = tokenProvider.CreateToken(null);
            }

            //assert
            Assert.Throws<ArgumentNullException>((Action) Action);
        }

        [Fact]
        public void CreateToken_SuccessfulCreation_ShouldReturnAToken()
        {
            //arrange
            var tokenProvider = new TokenProvider();

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, "john.doe@example.com")
            };

            //act
            var token = tokenProvider.CreateToken(claims);

            //assert
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public void CreateToken_SuccessfulCreation_ShouldDecode()
        {
            //arrange
            var tokenProvider = new TokenProvider();

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, "john.doe@example.com")
            };

            //act
            var token = tokenProvider.CreateToken(claims);
            var jwt = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            
            //assert
            Assert.NotNull(jwt);
            Assert.Contains(AuthOptions.AUDIENCE, jwt.Audiences);
            Assert.Equal(AuthOptions.ISSUER, jwt.Issuer);
        }
    }
}
