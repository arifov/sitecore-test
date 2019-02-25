using System;
using MAP.Tasks.Helpers;
using Xunit;

namespace MAP.Tasks.Tests
{
    public class ShortGuidTests
    {
        [Theory]
        [InlineData("8f98bea0-c09d-4d2d-af96-7962726f5806", "oL6Yj53ALU2vlnlicm9YBg")]
        [InlineData("c5c370d5-f3fa-4303-93b2-b2f9a86ca2f0", "1XDDxfrzA0OTsrL5qGyi8A")]
        public void ShortGuid_ShouldGenerateCorrectValue(string guid, string shortGuid)
        {
            var sut = new ShortGuid(new Guid(guid));
            Assert.Equal(shortGuid, sut.ToString());
        }

        [Theory]
        [InlineData("oL6Yj53ALU2vlnlicm9YBg", "8f98bea0-c09d-4d2d-af96-7962726f5806")]
        [InlineData("1XDDxfrzA0OTsrL5qGyi8A", "c5c370d5-f3fa-4303-93b2-b2f9a86ca2f0")]
        public void ShortGuid_ShouldParseShortToNormal(string shortGuid, string guid)
        {
            var sut = ShortGuid.Parse(shortGuid);
            Assert.Equal(Guid.Parse(guid), sut.ToGuid());
        }
    }
}
