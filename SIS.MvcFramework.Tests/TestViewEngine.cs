using SIS.MvcFramework.ViewEngineX;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SIS.MvcFramework.Tests
{
    public class TestViewEngine
    {
        [Theory]
        [InlineData("TestWithoutCSharpCode")]
        [InlineData("UseForForeachAndIf")]
        [InlineData("UseModelData")]
        public void TestWithModel(string testFileName)
        {
            IViewEngine viewEngine = new ViewEngine();

            string viewFileName = $"ViewTests/{testFileName}.html";
            string expectedResultFileName = $"ViewTests/{testFileName}.Result.html";

            string viewContent = File.ReadAllText(viewFileName);

            string actualOutput = viewEngine.Execute<object>(viewContent, new TestViewModel() 
            {
                StringValue = "str",
                ListValues = new List<string> { "123", "val1", string.Empty }
            });
            string expectedOutput = File.ReadAllText(expectedResultFileName);

            Assert.Equal(expectedOutput.TrimEnd(), actualOutput.TrimEnd());
        }

        [Theory]
        [InlineData("TestWithoutCSharpCode")]
        [InlineData("UseForForeachAndIf")]
        public void TestWithoutModel(string testFileName)
        {
            IViewEngine viewEngine = new ViewEngine();

            string viewFileName = $"ViewTests/{testFileName}.html";
            string expectedResultFileName = $"ViewTests/{testFileName}.Result.html";

            string viewContent = File.ReadAllText(viewFileName);

            string actualOutput = viewEngine.Execute<object>(viewContent, null);
            string expectedOutput = File.ReadAllText(expectedResultFileName);

            Assert.Equal(expectedOutput.TrimEnd(), actualOutput.TrimEnd());
        }
    }
}
