using Swissknife.Strings;
using Xunit;

namespace Swissknife.Tests.Strings
{
    public class StringExtensionsTests
    {
        [Fact]
        public void EmptyString_ShouldReturnTrue() => Assert.True("".IsNullOrEmpty());

        [Fact]
        public void NullString_ShouldReturnFalse() => Assert.True(default(string).IsNullOrEmpty());

        [Fact]
        public void FilledString_ShouldReturnFalse() => Assert.False("filled".IsNullOrEmpty());
    }
}
