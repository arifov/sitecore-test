using System;
using Xunit;

namespace Sitecore.MembershipAPI.Tests
{
    public class TokenProviderTests
    {
        [Fact]
        public void CreateToken_NullClaimsProvided_ShouldThrowAnException()
        {
            //arrange
            var tokkenProvider = new Token
            //act
            //assert
        }
    }
}
