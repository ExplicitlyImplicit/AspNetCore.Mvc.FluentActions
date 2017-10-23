using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithResponse
    {
        [Fact(DisplayName = "1 Response, returns string")]
        public void FluentControllerBuilder_FluentActionWithResponseReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingResponse()
                    .To(response =>
                    {
                        if (response == null)
                        {
                            throw new Exception("Response is null inside fluent action delegate.");
                        } 

                        return "Hello";
                    }),
                typeof(ControllerWithResponseReturnsString),
                null);
        }

        [Fact(DisplayName = "1 Response, returns string async")]
        public void FluentControllerBuilder_FluentActionWithResponseReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingResponse()
                    .To(async response =>
                    {
                        await Task.Delay(1);
                        if (response == null)
                        {
                            throw new Exception("Response is null inside fluent action delegate.");
                        }

                        return "Hello";
                    }),
                typeof(ControllerWithResponseReturnsStringAsync),
                null);
        }
    }
}
