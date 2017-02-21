using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithTempData
    {
        [Fact(DisplayName = "1 TempData, returns string")]
        public void FluentControllerBuilder_FluentActionWithTempDataReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
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

        [Fact(DisplayName = "1 TempData, returns string async")]
        public void FluentControllerBuilder_FluentActionWithTempDataReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingTempData()
                    .To(async tempData =>
                    {
                        await Task.Delay(1);
                        tempData["foo"] = "bar";
                        return (string)tempData["foo"];
                    }),
                typeof(ControllerWithTempDataReturnsStringAsync),
                null);
        }
    }
}
