using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithModelState
    {
        [Fact(DisplayName = "1 model state, returns string")]
        public void FluentControllerBuilder_FluentActionWithModelStateReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Post)
                    .UsingModelState()
                    .To(modelState => $"Hello World!"),
                typeof(ControllerWithModelStateReturnsString));
        }
    }
}
