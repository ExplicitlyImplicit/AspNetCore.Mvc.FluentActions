using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithViewData
    {
        [Fact(DisplayName = "1 ViewData, returns string")]
        public void FluentControllerBuilder_FluentActionWithViewDataReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingViewData()
                    .To(viewData =>
                    {
                        viewData["foo"] = "bar";
                        return (string)viewData["foo"];
                    }),
                typeof(ControllerWithViewDataReturnsString),
                null);
        }

        [Fact(DisplayName = "1 ViewData, returns string async")]
        public void FluentControllerBuilder_FluentActionWithViewDataReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingViewData()
                    .To(async viewData =>
                    {
                        await Task.Delay(1);
                        viewData["foo"] = "bar";
                        return (string)viewData["foo"];
                    }),
                typeof(ControllerWithViewDataReturnsStringAsync),
                null);
        }
    }
}
