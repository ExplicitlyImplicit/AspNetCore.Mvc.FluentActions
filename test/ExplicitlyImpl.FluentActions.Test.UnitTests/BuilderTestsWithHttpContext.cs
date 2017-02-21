using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithHttpContext
    {
        [Fact(DisplayName = "1 HttpContext, returns string")]
        public void FluentControllerBuilder_FluentActionWithHttpContextReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHttpContext()
                    .To(httpContext =>
                    {
                        if (httpContext == null)
                        {
                            throw new Exception("HttpContext is null inside fluent action delegate.");
                        } 

                        return "Hello";
                    }),
                typeof(ControllerWithHttpContextReturnsString),
                null);
        }

        [Fact(DisplayName = "1 HttpContext, returns string async")]
        public void FluentControllerBuilder_FluentActionWithHttpContextReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHttpContext()
                    .To(async httpContext =>
                    {
                        await Task.Delay(1);
                        if (httpContext == null)
                        {
                            throw new Exception("HttpContext is null inside fluent action delegate.");
                        }

                        return "Hello";
                    }),
                typeof(ControllerWithHttpContextReturnsStringAsync),
                null);
        }
    }
}
