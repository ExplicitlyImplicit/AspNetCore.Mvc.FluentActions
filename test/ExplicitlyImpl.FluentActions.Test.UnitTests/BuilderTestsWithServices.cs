using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithServices
    {
        [Fact(DisplayName = "1 service, returns string")]
        public void FluentControllerBuilder_FluentActionUsingServiceReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingService<IStringTestService>()
                    .To(stringTestService => stringTestService.GetTestString() + "FromAFluentAction"),
                typeof(ControllerWithStringService),
                new object[] { new StringTestService() });
        }

        [Fact(DisplayName = "1 service, returns string async")]
        public void FluentControllerBuilder_FluentActionUsingServiceReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingService<IStringTestService>()
                    .To(async stringTestService => await stringTestService.GetTestStringAsync() + "FromAFluentAction"),
                typeof(ControllerWithStringServiceAsync),
                new object[] { new StringTestService() });
        }

        [Fact(DisplayName = "1 service, returns list of users")]
        public void FluentControllerBuilder_FluentActionUsingServiceReturnsListOfUsers()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingService<IUserService>()
                    .To(userService => userService.ListUsers()),
                typeof(ControllerWithUserService),
                new object[] { new UserService() });
        }

        [Fact(DisplayName = "1 service, returns list of users async")]
        public void FluentControllerBuilder_FluentActionUsingServiceReturnsListOfUsersAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingService<IUserService>()
                    .To(userService => userService.ListUsersAsync()),
                typeof(ControllerWithUserServiceAsync),
                new object[] { new UserService() });
        }

        // No support for multiple parameters of same service
        [Fact(DisplayName = "2 services of same interface, throws")]
        public void FluentControllerBuilder_FluentActionUsingMultipleServicesOfSameInterfaceReturnsString()
        {
            Assert.Throws<Exception>(() =>
                BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .UsingService<IStringTestService>()
                        .UsingService<IStringTestService>()
                        .To((stringTestService1, stringTestService2) => 
                            stringTestService1.GetTestString() + "And" + stringTestService2.GetTestString()),
                    typeof(ControllerWithMultipleServicesOfSameInterface),
                    new object[] { new StringTestService(), new StringTestService2() })
            );
        }
    }
}
