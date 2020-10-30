using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithAuthorizeClass
    {
        [Fact(DisplayName = "1 authorize class (empty), returns string")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizeClassReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .AuthorizeClass()
                    .To(() => "hello"),
                typeof(ControllerWith1AuthorizeClassReturnsString),
                null);
        }

        [Fact(DisplayName = "1 authorize class (policy), returns string")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizeClassPolicyReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .AuthorizeClass(policy: "CanSayHello")
                    .To(() => "hello"),
                typeof(ControllerWith1AuthorizeClassPolicyReturnsString),
                null);
        }

        [Fact(DisplayName = "1 authorize class (roles), returns string")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizeClassRolesReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .AuthorizeClass(roles: "Admin")
                    .To(() => "hello"),
                typeof(ControllerWith1AuthorizeClassRolesReturnsString),
                null);
        }

        [Fact(DisplayName = "1 authorize class (authenticationSchemes), returns string")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizeClassAuthenticationSchemesReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .AuthorizeClass(authenticationSchemes: "Scheme")
                    .To(() => "hello"),
                typeof(ControllerWith1AuthorizeClassAuthenticationSchemesReturnsString),
                null);
        }

        [Fact(DisplayName = "1 authorize class (policy - roles - authenticationSchemes), returns string")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizeClassPolicyRolesAuthenticationSchemesReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .AuthorizeClass("CanSayHello", "Admin", "Scheme")
                    .To(() => "hello"),
                typeof(ControllerWith1AuthorizeClassPolicyRolesAuthenticationSchemesReturnsString),
                null);
        }

        [Fact(DisplayName = "1 authorize class (empty), returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionWith1AuthorizeClassReturnsViewResultAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .AuthorizeClass()
                    .To(async () => { await Task.Delay(1); return "hello"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1AuthorizeClassReturnsViewResultAsync),
                null);
        }
    }
}
