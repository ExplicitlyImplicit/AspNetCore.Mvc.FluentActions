using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithProperty
    {
        [Fact(DisplayName = "1 Property (Response), returns string")]
        public void FluentControllerBuilder_FluentActionWithPropertyReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingProperty<HttpResponse>("Response")
                    .To(response =>
                    {
                        if (response == null)
                        {
                            throw new Exception("Response is null inside fluent action delegate.");
                        } 

                        return "Hello";
                    }),
                typeof(ControllerWithPropertyReturnsString),
                null);
        }

        [Fact(DisplayName = "1 Property (Response), returns string async")]
        public void FluentControllerBuilder_FluentActionWithPropertyReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); })
                    .UsingProperty<HttpResponse>("Response")
                    .To(async response =>
                    {
                        await Task.Delay(1);
                        if (response == null)
                        {
                            throw new Exception("Response is null inside fluent action delegate.");
                        }

                        return "Hello";
                    }),
                typeof(ControllerWithPropertyReturnsStringAsync),
                null);
        }
    }
}
