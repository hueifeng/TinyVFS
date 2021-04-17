using Shouldly;
using Xunit;

namespace TinyVFS
{
    public class VirtualFilePathHelper_Tests
    {
        [Fact]
        public void NormalizePath()
        {
            VirtualFilePathHelper.NormalizePath("~/test-one/test-two/test-three.js").ShouldBe("~/test_one/test_two/test-three.js");
        }
    }
}
