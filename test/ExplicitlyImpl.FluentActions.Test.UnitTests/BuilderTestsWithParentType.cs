using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithParentType
    {
        [Fact(DisplayName = "1 parent type, returns string")]
        public void FluentControllerBuilder_FluentActionWithParentTypeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .InheritingFrom<BaseController>()
                    .To(() => $"hello"),
                typeof(ControllerWithParentTypeReturnsString),
                null);
        }
    }
}
