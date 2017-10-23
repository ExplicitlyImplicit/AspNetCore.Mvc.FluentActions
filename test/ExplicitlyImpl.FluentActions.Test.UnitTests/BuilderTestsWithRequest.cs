using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithRequest
    {
        [Fact(DisplayName = "1 Request, returns string")]
        public void FluentControllerBuilder_FluentActionWithRequestReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingRequest()
                    .To(request =>
                    {
                        if (request == null)
                        {
                            throw new Exception("Request is null inside fluent action delegate.");
                        } 

                        return "Hello";
                    }),
                typeof(ControllerWithRequestReturnsString),
                null);
        }

        [Fact(DisplayName = "1 Request, returns string async")]
        public void FluentControllerBuilder_FluentActionWithRequestReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingRequest()
                    .To(async request =>
                    {
                        await Task.Delay(1);
                        if (request == null)
                        {
                            throw new Exception("Request is null inside fluent action delegate.");
                        }

                        return "Hello";
                    }),
                typeof(ControllerWithRequestReturnsStringAsync),
                null);
        }
    }
}
