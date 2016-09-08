using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithViewData
    {
        [Fact(DisplayName = "1 ViewData, returns string")]
        public void FluentControllerBuilder_FluentActionUsingFormValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
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
    }
}
