using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithAllowAnonymous
    {
        [Fact(DisplayName = "1 AllowAnonymous, returns string")]
        public void FluentControllerBuilder_FluentActionWith1AllowAnonymousReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .AllowAnonymous()
                    .To(() => "hello"),
                typeof(ControllerWith1AllowAnonymousReturnsString),
                null);
        }

        [Fact(DisplayName = "1 AllowAnonymous, returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionWith1AllowAnonymousReturnsViewResultAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .AllowAnonymous()
                    .To(async () => { await Task.Delay(1); return "hello"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1AllowAnonymousReturnsViewResultAsync),
                null);
        }
    }
}
