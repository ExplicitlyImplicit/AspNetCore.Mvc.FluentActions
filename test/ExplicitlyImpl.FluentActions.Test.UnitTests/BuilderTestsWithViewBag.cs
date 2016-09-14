using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithViewBag
    {
        [Fact(DisplayName = "1 ViewBag, returns string")]
        public void FluentControllerBuilder_FluentActionWithViewBagReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingViewBag()
                    .To(viewBag =>
                    {
                        viewBag.Foo = "bar";
                        return (string)viewBag.Foo;
                    }),
                typeof(ControllerWithViewDataReturnsString),
                null);
        }
    }
}
