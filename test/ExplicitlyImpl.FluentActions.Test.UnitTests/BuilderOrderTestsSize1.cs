using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderOrderTestsSize1
    {
        private readonly TestLogger TestLogger;

        public BuilderOrderTestsSize1(ITestOutputHelper testOutputHelper)
        {
            TestLogger = new TestLogger(testOutputHelper);
        }

        [Fact(DisplayName = "OrderTest, Size 1: [Do] throws")]
        public void FluentControllerBuilder_FluentActionOrderTestDoThrows()
        {
            Assert.Throws<FluentActionValidationException>(() => {
                BuilderTestUtils.BuildAction(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .Do(() => { /* woop woop */ }),
                    TestLogger);
            });
        }

        [Fact(DisplayName = "OrderTest, Size 1: [DoA] throws")]
        public void FluentControllerBuilder_FluentActionOrderTestDoAThrows()
        {
            Assert.Throws<FluentActionValidationException>(() => {
                BuilderTestUtils.BuildAction(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .DoAsync(async () => { await Task.Delay(1); }),
                    TestLogger);
            });
        }

        [Fact(DisplayName = "OrderTest, Size 1: [To] returns string")]
        public void FluentControllerBuilder_FluentActionOrderTestToReturnsString()
        {
            var text = "";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => { text += "To"; return "Hello"; }),
                typeof(ControllerForOrderTestsReturnsString),
                null,
                TestLogger);

            Assert.Equal("To", text);
        }

        [Fact(DisplayName = "OrderTest, Size 1: [ToA] returns string")]
        public void FluentControllerBuilder_FluentActionOrderTestToAReturnsString()
        {
            var text = "";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); text += "ToA"; return "Hello"; }),
                typeof(ControllerForOrderTestsReturnsStringAsync),
                null,
                TestLogger);

            Assert.Equal("ToA", text);
        }
    }
}
