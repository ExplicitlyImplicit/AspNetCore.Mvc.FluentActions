using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithViewBag
    {
        [Fact(DisplayName = "1 ViewBag, returns string")]
        public void FluentControllerBuilder_FluentActionWithViewBagReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingViewBag()
                    .To(viewBag =>
                    {
                        viewBag.Foo = "bar";
                        return (string)viewBag.Foo;
                    }),
                typeof(ControllerWithViewBagReturnsString),
                null);
        }

        [Fact(DisplayName = "1 ViewBag, returns string async")]
        public void FluentControllerBuilder_FluentActionWithViewBagReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingViewBag()
                    .To(async viewBag =>
                    {
                        await Task.Delay(1);
                        viewBag.Foo = "bar";
                        return (string)viewBag.Foo;
                    }),
                typeof(ControllerWithViewBagReturnsStringAsync),
                null);
        }
    }
}
