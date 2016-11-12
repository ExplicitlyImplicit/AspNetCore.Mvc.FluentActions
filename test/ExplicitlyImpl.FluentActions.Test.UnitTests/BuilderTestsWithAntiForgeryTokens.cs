using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithAntiForgeryTokens
    {
        [Fact(DisplayName = "1 anti forgery token, returns string")]
        public void FluentControllerBuilder_FluentActionWithAntiForgeryTokenReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Post)
                    .ValidateAntiForgeryToken()
                    .To(() => $"Hello World!"),
                typeof(ControllerWithAntiForgeryTokenReturnsString));
        }
    }
}
