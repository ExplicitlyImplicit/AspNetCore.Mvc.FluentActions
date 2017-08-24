using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithParent
    {
        [Fact(DisplayName = "1 parent (from action), returns string")]
        public void FluentControllerBuilder_FluentActionUsingParentFromActionReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .InheritingFrom<BaseController>()
                    .UsingParent()
                    .To(parent => parent.Hello()),
                typeof(ControllerWithParentReturnsString),
                null);
        }


        [Fact(DisplayName = "1 parent (from config), returns string")]
        public void FluentControllerBuilder_FluentActionUsingParentFromConfigReturnsString()
        {
            var collection = FluentActionCollection.DefineActions(config => 
            {
                config.InheritingFrom<BaseController>();
            }, actions => 
            {
                actions.RouteGet("/route/url")
                    .UsingParent<BaseController>()
                    .To(parent => parent.Hello());
            });

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                collection.FluentActions[0],
                typeof(ControllerWithParentReturnsString),
                null);
        }

        [Fact(DisplayName = "1 parent (from action), 1 query string parameter (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingParentUsingQueryStringParameterFromActionReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .InheritingFrom<BaseController>()
                    .UsingQueryStringParameter<string>("name")
                    .UsingParent()
                    .To((name, parent) => parent.Hello(name)),
                typeof(ControllerWithParentAndBodyReturnsString),
                new object[] { "Oscar" });
        }

        [Fact(DisplayName = "1 parent (from action), returns string async")]
        public void FluentControllerBuilder_FluentActionUsingParentFromActionReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .InheritingFrom<BaseController>()
                    .UsingParent()
                    .To(async parent => { await Task.Delay(1); return parent.Hello(); }),
                typeof(ControllerWithParentReturnsStringAsync),
                null);
        }

        [Fact(DisplayName = "1 parent (from action), 1 query string parameter (string), returns string async")]
        public void FluentControllerBuilder_FluentActionUsingParentUsingQueryStringParameterFromActionReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .InheritingFrom<BaseController>()
                    .UsingQueryStringParameter<string>("name")
                    .UsingParent()
                    .To(async (name, parent) => { await Task.Delay(1); return parent.Hello(name); }),
                typeof(ControllerWithParentAndBodyReturnsStringAsync),
                new object[] { "Oscar" });
        }
    }
}
