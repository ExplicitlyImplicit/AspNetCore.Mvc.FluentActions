using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithGroupByAsync
    {
        [Fact(DisplayName = "GroupBy inside single action (type comparison)")]
        public void FluentControllerBuilder_FluentActionWithGroupBy()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .GroupBy("CustomGroupName")
                    .To(() => "Hello"),
                typeof(ControllerWithGroupNameOnlyApiExplorerSettingsAttribute),
                new object[0]);
        }

        [Fact(DisplayName = "IgnoreApi inside single action (type comparison)")]
        public void FluentControllerBuilder_FluentActionWithIgnoreApi()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .IgnoreApi()
                    .To(() => "Hello"),
                typeof(ControllerWithIgnoreApiOnlyApiExplorerSettingsAttribute),
                new object[0]);
        }

        [Fact(DisplayName = "GroupBy + IgnoreApi inside single action (type comparison)")]
        public void FluentControllerBuilder_FluentActionWithGroupByAndIgnoreApi()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .GroupBy("CustomGroupName")
                    .IgnoreApi()
                    .To(() => "Hello"),
                typeof(ControllerWithApiExplorerSettingsAttribute),
                new object[0]);
        }
    }
}
