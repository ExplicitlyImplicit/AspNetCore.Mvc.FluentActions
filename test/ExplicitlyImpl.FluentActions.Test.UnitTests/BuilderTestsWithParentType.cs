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

        [Fact(DisplayName = "1 parent type in config, returns string")]
        public void FluentControllerBuilder_FluentActionWithParentTypeInConfigReturnsString()
        {
            var actionCollection = FluentActionCollection.DefineActions(
                config =>
                {
                    config.InheritingFrom(typeof(BaseController));
                },
                actions =>
                {
                    actions.Add(
                        new FluentAction("/route/url", HttpMethod.Get)
                            .InheritingFrom<BaseController>()
                            .To(() => $"hello")
                    );
                }
            );

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                actionCollection.FluentActions[0],
                typeof(ControllerWithParentTypeReturnsString),
                null);
        }

        [Fact(DisplayName = "1 parent type in config")]
        public void FluentControllerBuilder_FluentActionCollectionWithParentTypeInConfig()
        {
            var actionCollection = FluentActionCollection.DefineActions(
                config =>
                {
                    config.InheritingFrom<BaseController>();
                },
                actions =>
                {
                    actions
                        .RouteGet("/users", "ListUsers")
                        .UsingService<IUserService>()
                        .To(userService => userService.ListUsers());

                    actions
                        .RoutePost("/users", "AddUser")
                        .UsingService<IUserService>()
                        .UsingBody<UserItem>()
                        .To((userService, user) => userService.AddUser(user));

                    actions
                        .RouteGet("/users/{userId}", "GetUser")
                        .UsingService<IUserService>()
                        .UsingRouteParameter<int>("userId")
                        .To((userService, userId) => userService.GetUserById(userId));

                    actions
                        .RoutePut("/users/{userId}", "UpdateUser")
                        .UsingService<IUserService>()
                        .UsingRouteParameter<int>("userId")
                        .UsingBody<UserItem>()
                        .To((userService, userId, user) => userService.UpdateUser(userId, user));

                    actions
                        .RouteDelete("/users/{userId}", "RemoveUser")
                        .UsingService<IUserService>()
                        .UsingRouteParameter<int>("userId")
                        .To((userService, userId) => userService.RemoveUser(userId));
                }
            );

            foreach (var action in actionCollection)
            {
                Assert.Equal(action.Definition.ParentType, typeof(BaseController));
            }
        }
    }
}
