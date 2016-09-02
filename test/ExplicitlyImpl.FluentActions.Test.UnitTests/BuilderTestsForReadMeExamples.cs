using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsForReadMeExamples
    {
        [Fact(DisplayName = "README example 1: HelloWorld")]
        public void FluentControllerBuilder_FluentActionForReadMeExample1()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/", HttpMethod.Get)
                    .To(() => "Hello World!"),
                typeof(ControllerForReadMeExample1),
                null);
        }

        [Fact(DisplayName = "README example 2: Another example")]
        public void FluentControllerBuilder_FluentActionForReadMeExample2()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/users/{userId}", HttpMethod.Get)
                    .UsingService<IUserService>()
                    .UsingRouteParameter<int>("userId")
                    .To((userService, userId) => userService.GetUserById(userId))
                    .ToView("~/Views/Users/DisplayUser.cshtml"),
                typeof(ControllerForReadMeExample2));
        }
    }
}
