using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithTempData
    {
        [Fact(DisplayName = "1 TempData, returns string")]
        public void FluentControllerBuilder_FluentActionWithTempDataReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingTempData()
                    .To(tempData =>
                    {
                        tempData["foo"] = "bar";
                        return (string)tempData["foo"];
                    }),
                typeof(ControllerWithTempDataReturnsString),
                null);
        }
    }
}
