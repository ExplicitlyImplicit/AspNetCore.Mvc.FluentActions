using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithAuthorize
    {
        [Fact(DisplayName = "1 authorize (empty), returns string")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Authorize()
                    .To(() => "hello"),
                typeof(ControllerWith1AuthorizeReturnsString),
                null);
        }

        [Fact(DisplayName = "1 authorize (policy), returns string")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizePolicyReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Authorize(policy: "CanSayHello")
                    .To(() => "hello"),
                typeof(ControllerWith1AuthorizePolicyReturnsString),
                null);
        }

        [Fact(DisplayName = "1 authorize (roles), returns string")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizeRolesReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Authorize(roles: "Admin")
                    .To(() => "hello"),
                typeof(ControllerWith1AuthorizeRolesReturnsString),
                null);
        }

        [Fact(DisplayName = "1 authorize (authenticationSchemes), returns string")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizeAuthenticationSchemesReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Authorize(authenticationSchemes: "Scheme")
                    .To(() => "hello"),
                typeof(ControllerWith1AuthorizeAuthenticationSchemesReturnsString),
                null);
        }

        [Fact(DisplayName = "1 authorize (policy - roles - authenticationSchemes), returns string")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizePolicyRolesAuthenticationSchemesReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Authorize("CanSayHello", "Admin", "Scheme")
                    .To(() => "hello"),
                typeof(ControllerWith1AuthorizePolicyRolesAuthenticationSchemesReturnsString),
                null);
        }

        [Fact(DisplayName = "1 authorize (empty), returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizeReturnsViewResultAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Authorize()
                    .To(async () => { await Task.Delay(1); return "hello"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1AuthorizeReturnsViewResultAsync),
                null);
        }
    }
}
