using System;
using Xunit;

namespace Merken.NetCoreBuild.Test
{
    public class UtilTests
    {
        [Fact]
        public void Test_Util()
        {
            var str = "This_Is_A_Test";
            var strArray = str.Split(new String[]{"_"}, StringSplitOptions.RemoveEmptyEntries);

            Assert.Equal("Test", strArray[3]);
        }
    }
}
